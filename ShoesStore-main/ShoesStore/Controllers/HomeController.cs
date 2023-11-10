using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoesStore.Models;
using ShoesStore.Models.ModelDTOs;
using ShoesStore.Models.ProcedureModels;
using ShoesStore.ViewModels;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using X.PagedList;

namespace ShoesStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Qlbangiaynhom7Context db = new Qlbangiaynhom7Context();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lấy và gửi giũ liệu sang trang Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var lstGiay = db.Giays.GroupBy(x => x.TenGiay)
                        .Select(group => group.First())
                        .ToList();

            List<GiayDTO> lstShoesDTO = new List<GiayDTO>();

            foreach (var item in lstGiay)
            {
                GiayDTO temp = new GiayDTO
                {
                    MaGiay = item.MaGiay,
                    TenLoai = db.LoaiGiays.Find(item.MaLoai).TenLoai,
                    TenGiay = item.TenGiay,
                    KichCo = item.KichCo,
                    MauSac = item.MauSac,
                    GiaGoc = item.GiaGoc,
                    GiaBan = item.GiaBan,
                    PhanTramGiam = item.PhanTramGiam,
                    DanhGia = item.DanhGia,
                    AnhDaiDien = item.AnhDaiDien,
                    SoLuong = item.SoLuong
                };
                lstShoesDTO.Add(temp);
            }
            return View(lstShoesDTO);
        }

        /// <summary>
        /// Phân trang và lọc nhiều điều kiện. Thực hiện hàm này khi nhấn paging
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startPrice"></param>
        /// <param name="limitPrice"></param>
        /// <param name="occasionCheckboxes"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult Shop(string? keyword, int? startPrice, int? limitPrice, List<string>? occasionCheckboxes, int page = 1)
        {
            int pageNumber = page;
            int pageSize = 3;
            var lstGiay = db.Giays.GroupBy(x => x.TenGiay)
            .Select(group => group.First())
            .ToList();
            var lstShoesDTO = GetListProduct(lstGiay);

            var temp = lstShoesDTO;

            if (occasionCheckboxes.Count > 0)
            {
                lstShoesDTO = lstShoesDTO.Where(x => occasionCheckboxes.Any(occasion => x.TenLoai.Contains(occasion))).ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.TenGiay.ToLower().Contains(keyword.ToLower())).ToList();
            }
            if (startPrice.HasValue)
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.GiaBan >= startPrice).ToList();
            }
            if (limitPrice.HasValue)
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.GiaBan <= limitPrice).ToList();
            }

            ViewBag.pageSize = pageSize;
            ViewBag.keyword = keyword;
            ViewBag.startPrice = startPrice;
            ViewBag.limitPrice = limitPrice;
            ViewBag.occasionCheckboxes = occasionCheckboxes;

            PagedList<GiayDTO> lst = new PagedList<GiayDTO>(lstShoesDTO, pageNumber, pageSize);

            return View(lst);
        }

        /// <summary>
        /// API Phân trang và lọc theo nhiều điều kiện
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startPrice"></param>
        /// <param name="limitPrice"></param>
        /// <param name="occasionCheckboxes"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetShoesByFilter(string? keyword, int? startPrice, int? limitPrice, List<string>? occasionCheckboxes, int page = 1)
        {
            int pageNumber = page;
            int pageSize = 3;
            var lstGiay = db.Giays.GroupBy(x => x.TenGiay)
                        .Select(group => group.First())
                        .ToList();
            var lstShoesDTO = GetListProduct(lstGiay);

            var temp = lstShoesDTO;

            if (occasionCheckboxes.Count > 0)
            {
                lstShoesDTO = lstShoesDTO.Where(x => occasionCheckboxes.Any(occasion => x.TenLoai.Contains(occasion))).ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.TenGiay.ToLower().Contains(keyword.ToLower())).ToList();
            }
            if (startPrice.HasValue)
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.GiaBan >= startPrice).ToList();
            }
            if (limitPrice.HasValue)
            {
                lstShoesDTO = lstShoesDTO.Where(x => x.GiaBan <= limitPrice).ToList();
            }
            
            ViewBag.pageSize = pageSize;
            ViewBag.keyword = keyword;
            ViewBag.startPrice = startPrice;
            ViewBag.limitPrice = limitPrice;
            ViewBag.occasionCheckboxes = occasionCheckboxes;

            PagedList<GiayDTO> lst = new PagedList<GiayDTO>(lstShoesDTO, pageNumber, pageSize);
            return PartialView("_ShoesFiltered", lst);
        }

        /// <summary>
        /// Chuyển từ kiểu List<Giay> sang  List<GiayDTO>
        /// </summary>
        /// <param name="lstShoes">Danh sách Giay</param>
        /// <returns>1 danh sách GiayDTO</returns>
        private List<GiayDTO> GetListProduct(List<Giay> lstShoes)
        {
            List<GiayDTO> lstShoesDTO = new List<GiayDTO>();

            foreach (var item in lstShoes)
            {
                GiayDTO temp = GiayToGiayDTO(item);
                lstShoesDTO.Add(temp);
            }
            return lstShoesDTO;
        }

        /// <summary>
        /// Chuyển dữ liệu 1 chiếc giày sang trang detail giày
        /// </summary>
        /// <param name="tenGiay"></param>
        /// <returns></returns>
        public IActionResult Single(string tenGiay)
        {
            var giays = db.Giays.Where(x=>x.TenGiay == tenGiay).OrderBy(x=>x.MaGiay);
            if (giays == null)
            {
                return RedirectToAction("Index");
            }

            List<GiayDTO> lstShoesDTO = new List<GiayDTO>();
            foreach (var item in giays)
            {
                GiayDTO temp = GiayToGiayDTO(item);
                lstShoesDTO.Add(temp);
            }

            return View(lstShoesDTO);
        }

        /// <summary>
        /// Lấy thông tin giày theo mã
        /// </summary>
        /// <param name="maGiay"></param>
        /// <returns>dữ liệu giày dưới dạng JSON</returns>
        [HttpGet]
        public ActionResult GetShoesByID(string maGiay)
        {
            return Json(db.Giays.Find(maGiay));
        }
        /// <summary>
        /// Chuyển 1 đối tượng Giay sang GiayDTO
        /// </summary>
        /// <param name="giay"></param>
        /// <returns></returns>
        private GiayDTO GiayToGiayDTO(Giay giay)
        {
            GiayDTO temp = new GiayDTO();
            using( var context = new Qlbangiaynhom7Context())
            {

                temp.MaGiay = giay.MaGiay;
                temp.TenLoai = context.LoaiGiays.First(x => x.MaLoai == giay.MaLoai).TenLoai;
                temp.TenGiay = giay.TenGiay;
                temp.KichCo = giay.KichCo;
                temp.MauSac = giay.MauSac;
                temp.GiaGoc = giay.GiaGoc;
                temp.GiaBan = giay.GiaBan;
                temp.PhanTramGiam = giay.PhanTramGiam;
                temp.DanhGia = giay.DanhGia;
                temp.AnhDaiDien = giay.AnhDaiDien;
                temp.SoLuong = giay.SoLuong;
            }
            return temp;
        }

        
        

        //[Authentication]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceAnOrder(string fullName, string phoneNumber, string address)
        {
            //Cập nhật thông tin khách hàng
            var customer = db.KhachHangs.Find(UserContext.MaKH);
            if(customer != null)
            {
                customer.TenKh = fullName;
                customer.SoDienThoai = phoneNumber;
                customer.DiaChi = address;
                db.KhachHangs.Update(customer);
                db.SaveChanges();

                //Tạo hóa đơn bán
                string maHDB = db.Set<ProcPlaceAnOrder>().FromSqlRaw("EXEC proc_HoaDonBan_PlaceAnOrder {0}", UserContext.MaKH)
                                          .ToList().ElementAt(0).ID;

                //chuyển dữ liệu từ giỏ hàng sang hóa đơn bán
                List<ChiTietGioHangGiayModel> lstShoesInCart = new List<ChiTietGioHangGiayModel>();
                var giaycart = db.ChiTietGioHangs.Where(x => x.MaGioHang == UserContext.MaGioHang).ToList();

                foreach (var item in giaycart)
                {
                    db.ChiTietHdbs.Add(new ChiTietHdb
                    {
                        MaHdb = maHDB,
                        MaGiay = db.Giays.Find(item.MaGiay).MaGiay,
                        SoLuong = item.SoLuong,
                        KhuyenMai = 0
                    });
                    db.SaveChanges();
                }

                //Clear gio hang
                db.Database.ExecuteSqlRaw($"DELETE FROM chitietgiohang WHERE MaGioHang = '{UserContext.MaGioHang}'");
                UserContext.SoSanPham = 0;
            }
            return RedirectToAction("Index");
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        //public IActionResult Blog()
        //{
        //    return View();
        //}
        //public IActionResult BlogSingle()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}