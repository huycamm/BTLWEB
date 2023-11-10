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
    public class NhanViensController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public NhanViensController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }     

        // GET: Admin/NhanViens/Create
        public IActionResult Create()
        {
            ViewData["TaiKhoan"] = new SelectList(_context.Tusers, "TaiKhoan", "TaiKhoan");
            return View();
        }

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNv,TaiKhoan,GioiTinh,HoTenNv,NgaySinh,SoDienThoai,ChucVu,Luong,TinhTrangCongViec,AnhDaiDien")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            ViewData["TaiKhoan"] = new SelectList(_context.Tusers, "TaiKhoan", "TaiKhoan", nhanVien.TaiKhoan);
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewData["TaiKhoan"] = new SelectList(_context.Tusers, "TaiKhoan", "TaiKhoan", nhanVien.TaiKhoan);
            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNv,TaiKhoan,GioiTinh,HoTenNv,NgaySinh,SoDienThoai,ChucVu,Luong,TinhTrangCongViec,AnhDaiDien")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.MaNv))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Admin");
            }
            ViewData["TaiKhoan"] = new SelectList(_context.Tusers, "TaiKhoan", "TaiKhoan", nhanVien.TaiKhoan);
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.TaiKhoanNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.NhanViens'  is null.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool NhanVienExists(string id)
        {
          return (_context.NhanViens?.Any(e => e.MaNv == id)).GetValueOrDefault();
        }
    }
}
