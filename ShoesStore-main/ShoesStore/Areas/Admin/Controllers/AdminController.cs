using Microsoft.AspNetCore.Mvc;
using ShoesStore.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoesStore.Models.ModelDTOs;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using ShoesStore.Models.ProcedureModels;
using ShoesStore.Models.Authentication;

namespace ShoesStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        Qlbangiaynhom7Context db = new Qlbangiaynhom7Context();
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

		[Route("")]
        [Route("Index")]
        [AuthenticationAdmin]
        public IActionResult Index()
		{
            AdminContext.SoTienBan30NgayGanNhat = db.ProcTienBan30Ngays.FromSql($"Exec TongTienBan30Ngay").ToList().ElementAt(0);
            AdminContext.SoTienNhap30NgayGanNhat = db.ProcTienNhap30Ngays.FromSql($"Exec TongTienNhap30Ngay").ToList().ElementAt(0);
            AdminContext.SoHDB30NgayGanNhat = db.ProcTongHDB30Ngays.FromSql($"Exec TongHDB30Ngay").ToList().ElementAt(0);
            AdminContext.SoTienBanTrongNam = db.Set<ProcTongTienBanHangThang>().FromSqlRaw("EXEC TongTienBanHangThang {0}", 2023).ToList();

            var lstNhanVien = db.NhanViens.OrderBy(x => x.MaNv).ToList();
			return View(lstNhanVien);
		}

        [HttpPost]
        public IActionResult GetChartData(int year)
        {
            var chartData = db.Set<ProcTongTienBanHangThang>().FromSqlRaw("EXEC TongTienBanHangThang {0}", year).ToList();
            AdminContext.SoTienBanTrongNam = chartData;

            return PartialView("_IndexChart", chartData);
        }


        [Route("Products")]
        public IActionResult Products()
        {
            var lstGiay = db.Giays.OrderBy(x => x.MaGiay).ToList();
            return View(lstGiay);
        }
        [Route("TypeProducts")]
        public IActionResult TypeProducts()
        {
            var lstTP = db.LoaiGiays.OrderBy(x => x.MaLoai).ToList();
            return View(lstTP);
        }
        [Route("HDN")]
        public IActionResult HDN()
        {
            var lstHDN = db.HoaDonNhaps.OrderBy(x => x.MaHdn).ToList();
            return View(lstHDN);
        }
        [Route("HDB")]
        public IActionResult HDB()
        {
            var lstHDB = db.HoaDonBans.OrderBy(x => x.MaHdb).ToList();
            return View(lstHDB);
        }      
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Password")]
        public IActionResult Password()
        {
            return View();
        }
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
