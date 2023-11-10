using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Models;
using ShoesStore.Models.ModelDTOs;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

namespace ShoesStore.Controllers
{
	public class AccessController : Controller
	{
        Qlbangiaynhom7Context db = new Qlbangiaynhom7Context();

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("TaiKhoan") == null)
            {
                return View();
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(Tuser user)
        {
            if (HttpContext.Session.GetString("TaiKhoan") == null)
            {
                var obj = db.Tusers.FirstOrDefault(x => x.TaiKhoan == user.TaiKhoan && x.MatKhau == Encrypt(user.TaiKhoan, user.MatKhau));
                if (obj != null)
                {
                    HttpContext.Session.SetString("TaiKhoan", obj.TaiKhoan.ToString());
                    HttpContext.Session.SetInt32("Role", obj.Role);

                    if (obj.Role == 0)
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                    else
                    {
                        UserContext.IsLogin = true;
                        UserContext.TaiKhoan = obj.TaiKhoan;
                        var query1 = from Tuser in db.Tusers
                                     join KhachHang in db.KhachHangs
                                     on UserContext.TaiKhoan equals KhachHang.TaiKhoan
                                     select KhachHang.MaKh;
                        var kh = query1.ToList();
                        UserContext.MaKH = kh.ElementAt(0);

                        var query2 = from KhachHang in db.KhachHangs
                                     join GioHang in db.GioHangs
                                     on UserContext.MaKH equals GioHang.MaKh
                                     select GioHang.MaGioHang;
                        UserContext.MaGioHang = query2.ToList().ElementAt(0);
                        UserContext.SoSanPham = (int)db.ChiTietGioHangs.Where(c => c.MaGioHang == UserContext.MaGioHang).Sum(c => c.SoLuong);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Incorrect username or password !";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(Tuser user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Tusers.FirstOrDefault(s => s.TaiKhoan == user.TaiKhoan);
                if (check == null)
                {
                    var nameParam = new SqlParameter("@TaiKhoan", user.TaiKhoan);
                    var emailParam = new SqlParameter("@MatKhau", Encrypt(user.TaiKhoan, user.MatKhau));
                    var parameters = new DbParameter[] { nameParam, emailParam };

                    db.Database.ExecuteSqlRaw("EXEC InsertUser @TaiKhoan, @MatKhau", parameters);
                    return RedirectToAction("Login", "Access");
                }
                else
                {
                    TempData["ErrorMessage"] = "Username already exists !";
                    return View();
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("TaiKhoan");
            UserContext.IsLogin = false;
            UserContext.TaiKhoan = "";
            UserContext.MaKH = "";
            UserContext.MaGioHang = "";
            return RedirectToAction("Login", "Access");
        }


        private string Encrypt(string key, string clearText)
        {
            string encryptionKey = key + "jqk";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        private string Decrypt(string key, string cipherText)
        {
            string encryptionKey = key + "jqk";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }
}
