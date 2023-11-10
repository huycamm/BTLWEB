using Microsoft.AspNetCore.Mvc;
using ShoesStore.Models;
using ShoesStore.Models.ModelDTOs;
using ShoesStore.ViewModels;
using X.PagedList;

namespace ShoesStore.Controllers
{
	public class CartController : Controller
	{
        Qlbangiaynhom7Context db = new Qlbangiaynhom7Context();

        /// <summary>
        /// lấy dữ liệu và gửi cho trang Cart
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        //[Authentication]
        public IActionResult Index()
        {
            List<ChiTietGioHangGiayModel> lstShoesInCart = new List<ChiTietGioHangGiayModel>();
            var giaycart = db.ChiTietGioHangs.Where(x => x.MaGioHang == UserContext.MaGioHang).ToList();

            foreach (var item in giaycart)
            {
                lstShoesInCart.Add(new ChiTietGioHangGiayModel()
                {
                    ChiTietGioHang = item,
                    Giay = db.Giays.Find(item.MaGiay)
                });

            }

            return View(lstShoesInCart);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(string shoesName, int shoesSize, int quantity)
        {

            string maGiay = db.Giays.FirstOrDefault(x => x.TenGiay == shoesName && x.KichCo == shoesSize).MaGiay;

            db.ChiTietGioHangs.Update(new ChiTietGioHang()
            {
                MaGioHang = UserContext.MaGioHang,
                MaGiay = maGiay,
                SoLuong = quantity
            });
            db.SaveChanges();

            List<ChiTietGioHangGiayModel> lstShoesInCart = new List<ChiTietGioHangGiayModel>();
            var giayCart = db.ChiTietGioHangs.Where(x => x.MaGioHang == UserContext.MaGioHang).ToList();

            foreach (var item in giayCart)
            {
                lstShoesInCart.Add(new ChiTietGioHangGiayModel()
                {
                    ChiTietGioHang = item,
                    Giay = db.Giays.Find(item.MaGiay)
                });
            }
            return PartialView("_CartDetail", lstShoesInCart);
        }


        /// <summary>
        /// Thêm sản phầm vào giỏ hàng
        /// </summary>
        /// <param name="maGioHang"></param>
        /// <param name="maGiay"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(string maGioHang, string maGiay, int soLuong = 1)
        {
            try
            {
                if (maGioHang == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var item = db.ChiTietGioHangs.FirstOrDefault(x => x.MaGioHang == maGioHang && x.MaGiay == maGiay);

                if (item != null)
                {
                    item.SoLuong += soLuong;
                    db.ChiTietGioHangs.Update(item);
                }
                else
                {
                    ChiTietGioHang chiTietGioHang = new ChiTietGioHang()
                    {
                        MaGioHang = maGioHang,
                        MaGiay = maGiay,
                        SoLuong = soLuong
                    };
                    db.ChiTietGioHangs.Add(chiTietGioHang);
                }
                UserContext.SoSanPham += soLuong;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult RemoveItem(string shoesName, int shoesSize)
        {
            string maGiay = db.Giays.FirstOrDefault(x => x.TenGiay == shoesName && x.KichCo == shoesSize).MaGiay;
            var chiTiet =  db.ChiTietGioHangs.Find(UserContext.MaGioHang, maGiay);

            if (chiTiet != null)
            {
                db.ChiTietGioHangs.Remove(chiTiet);
                db.SaveChanges();
            }

            List<ChiTietGioHangGiayModel> lstShoesInCart = new List<ChiTietGioHangGiayModel>();
            var giayCart = db.ChiTietGioHangs.Where(x => x.MaGioHang == UserContext.MaGioHang).ToList();

            foreach (var item in giayCart)
            {
                lstShoesInCart.Add(new ChiTietGioHangGiayModel()
                {
                    ChiTietGioHang = item,
                    Giay = db.Giays.Find(item.MaGiay)
                });
            }
            return PartialView("_CartDetail", lstShoesInCart);
        }
    }
}
