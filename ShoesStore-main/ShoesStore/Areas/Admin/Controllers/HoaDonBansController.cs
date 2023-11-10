using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Models;
using ShoesStore.Models.Authentication;

namespace ShoesStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationAdmin]
    public class HoaDonBansController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public HoaDonBansController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }

        // GET: Admin/HoaDonBans/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "TenKh");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "HoTenNv");
            return View();
        }

        // POST: Admin/HoaDonBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHdb,MaNv,MaKh,NgayBan,TrangThai,PhuongThucThanhToan,TongTien,GhiChu")] HoaDonBan hoaDonBan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDonBan);
                await _context.SaveChangesAsync();
                return RedirectToAction("HDB", "Admin");
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDonBan.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonBan.MaNv);
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.HoaDonBans == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBans.FindAsync(id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "TenKh", hoaDonBan.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "HoTenNv", hoaDonBan.MaNv);
            return View(hoaDonBan);
        }

        // POST: Admin/HoaDonBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHdb,MaNv,MaKh,NgayBan,TrangThai,PhuongThucThanhToan,TongTien,GhiChu")] HoaDonBan hoaDonBan)
        {
            if (id != hoaDonBan.MaHdb)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonBanExists(hoaDonBan.MaHdb))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HDB", "Admin");
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDonBan.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonBan.MaNv);
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.HoaDonBans == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBans
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.MaHdb == id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }

            return View(hoaDonBan);
        }

        // POST: Admin/HoaDonBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.HoaDonBans == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.HoaDonBans'  is null.");
            }
            var hoaDonBan = await _context.HoaDonBans.FindAsync(id);
            if (hoaDonBan != null)
            {
                _context.HoaDonBans.Remove(hoaDonBan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HDB", "Admin");
        }

        private bool HoaDonBanExists(string id)
        {
          return (_context.HoaDonBans?.Any(e => e.MaHdb == id)).GetValueOrDefault();
        }
    }
}
