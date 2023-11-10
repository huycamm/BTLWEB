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
    public class GiaysController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public GiaysController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }

        // GET: Admin/Giays/Create
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.LoaiGiays, "MaLoai", "TenLoai");
            ViewBag.MaLoai = new SelectList(_context.LoaiGiays.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        // POST: Admin/Giays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGiay,MaLoai,TenGiay,KichCo,MauSac,SoLuong,GiaNhap,GiaGoc,GiaBan,PhanTramGiam,TinhTrang,DanhGia,AnhDaiDien")] Giay giay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giay);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products", "Admin");
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiGiays, "MaLoai", "MaLoai", giay.MaLoai);
            return View(giay);
        }

        // GET: Admin/Giays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Giays == null)
            {
                return NotFound();
            }

            var giay = await _context.Giays.FindAsync(id);
            if (giay == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiGiays, "MaLoai", "TenLoai", giay.MaLoai);
            return View(giay);
        }

        // POST: Admin/Giays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaGiay,MaLoai,TenGiay,KichCo,MauSac,SoLuong,GiaNhap,GiaGoc,GiaBan,PhanTramGiam,TinhTrang,DanhGia,AnhDaiDien")] Giay giay)
        {
            if (id != giay.MaGiay)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiayExists(giay.MaGiay))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Products", "Admin");
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiGiays, "MaLoai", "MaLoai", giay.MaLoai);
            return View(giay);
        }

        // GET: Admin/Giays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Giays == null)
            {
                return NotFound();
            }

            var giay = await _context.Giays
                .Include(g => g.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaGiay == id);
            if (giay == null)
            {
                return NotFound();
            }

            return View(giay);
        }

        // POST: Admin/Giays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Giays == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.Giays'  is null.");
            }
            var giay = await _context.Giays.FindAsync(id);
            if (giay != null)
            {
                _context.Giays.Remove(giay);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Products", "Admin");
        }

        private bool GiayExists(string id)
        {
          return (_context.Giays?.Any(e => e.MaGiay == id)).GetValueOrDefault();
        }
    }
}
