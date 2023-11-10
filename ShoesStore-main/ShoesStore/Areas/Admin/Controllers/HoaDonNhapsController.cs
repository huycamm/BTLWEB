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
    public class HoaDonNhapsController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public HoaDonNhapsController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }

        // GET: Admin/HoaDonNhaps
        public async Task<IActionResult> Index()
        {
            var qlbangiaynhom7Context = _context.HoaDonNhaps.Include(h => h.MaNccNavigation).Include(h => h.MaNvNavigation);
            return View(await qlbangiaynhom7Context.ToListAsync());
        }

        // GET: Admin/HoaDonNhaps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.HoaDonNhaps == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.MaNccNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.MaHdn == id);
            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return View(hoaDonNhap);
        }

        // GET: Admin/HoaDonNhaps/Create
        public IActionResult Create()
        {
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "HoTenNv");
            return View();
        }

        // POST: Admin/HoaDonNhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHdn,MaNv,MaNcc,NgayNhap,TongTien")] HoaDonNhap hoaDonNhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDonNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction("HDN", "Admin");
            }
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hoaDonNhap.MaNcc);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonNhap.MaNv);
            return View(hoaDonNhap);
        }

        // GET: Admin/HoaDonNhaps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.HoaDonNhaps == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps.FindAsync(id);
            if (hoaDonNhap == null)
            {
                return NotFound();
            }
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc", hoaDonNhap.MaNcc);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "HoTenNv", hoaDonNhap.MaNv);
            return View(hoaDonNhap);
        }

        // POST: Admin/HoaDonNhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHdn,MaNv,MaNcc,NgayNhap,TongTien")] HoaDonNhap hoaDonNhap)
        {
            if (id != hoaDonNhap.MaHdn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonNhapExists(hoaDonNhap.MaHdn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HDN", "Admin");
            }
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hoaDonNhap.MaNcc);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonNhap.MaNv);
            return View(hoaDonNhap);
        }

        // GET: Admin/HoaDonNhaps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.HoaDonNhaps == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.MaNccNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.MaHdn == id);
            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return View(hoaDonNhap);
        }

        // POST: Admin/HoaDonNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.HoaDonNhaps == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.HoaDonNhaps'  is null.");
            }
            var hoaDonNhap = await _context.HoaDonNhaps.FindAsync(id);
            if (hoaDonNhap != null)
            {
                _context.HoaDonNhaps.Remove(hoaDonNhap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HDN", "Admin");
        }

        private bool HoaDonNhapExists(string id)
        {
          return (_context.HoaDonNhaps?.Any(e => e.MaHdn == id)).GetValueOrDefault();
        }
    }
}
