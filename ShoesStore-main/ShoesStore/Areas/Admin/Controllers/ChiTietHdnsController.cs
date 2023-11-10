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
    public class ChiTietHdnsController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public ChiTietHdnsController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }

        // GET: Admin/ChiTietHdns
        public async Task<IActionResult> Index()
        {
            var qlbangiaynhom7Context = _context.ChiTietHdns.Include(c => c.MaGiayNavigation).Include(c => c.MaHdnNavigation);
            return View(await qlbangiaynhom7Context.ToListAsync());
        }

        // GET: Admin/ChiTietHdns/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ChiTietHdns == null)
            {
                return NotFound();
            }

            var chiTietHdn = await _context.ChiTietHdns
                .Include(c => c.MaGiayNavigation)
                .Include(c => c.MaHdnNavigation)
                .FirstOrDefaultAsync(m => m.MaHdn == id);
            if (chiTietHdn == null)
            {
                return NotFound();
            }

            return View(chiTietHdn);
        }

        // GET: Admin/ChiTietHdns/Create
        public IActionResult Create()
        {
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay");
            ViewData["MaHdn"] = new SelectList(_context.HoaDonNhaps, "MaHdn", "MaHdn");
            return View();
        }

        // POST: Admin/ChiTietHdns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHdn,MaGiay,SoLuong,KhuyenMai")] ChiTietHdn chiTietHdn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietHdn);
                await _context.SaveChangesAsync();
                return RedirectToAction("HDN", "Admin");
            }
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdn.MaGiay);
            ViewData["MaHdn"] = new SelectList(_context.HoaDonNhaps, "MaHdn", "MaHdn", chiTietHdn.MaHdn);
            return View(chiTietHdn);
        }

        // GET: Admin/ChiTietHdns/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ChiTietHdns == null)
            {
                return NotFound();
            }

            var chiTietHdn = await _context.ChiTietHdns.FindAsync(id);
            if (chiTietHdn == null)
            {
                return NotFound();
            }
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdn.MaGiay);
            ViewData["MaHdn"] = new SelectList(_context.HoaDonNhaps, "MaHdn", "MaHdn", chiTietHdn.MaHdn);
            return View(chiTietHdn);
        }

        // POST: Admin/ChiTietHdns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHdn,MaGiay,SoLuong,KhuyenMai")] ChiTietHdn chiTietHdn)
        {
            if (id != chiTietHdn.MaHdn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHdn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHdnExists(chiTietHdn.MaHdn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HDN","Admin");
            }
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdn.MaGiay);
            ViewData["MaHdn"] = new SelectList(_context.HoaDonNhaps, "MaHdn", "MaHdn", chiTietHdn.MaHdn);
            return View(chiTietHdn);
        }

        // GET: Admin/ChiTietHdns/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ChiTietHdns == null)
            {
                return NotFound();
            }

            var chiTietHdn = await _context.ChiTietHdns
                .Include(c => c.MaGiayNavigation)
                .Include(c => c.MaHdnNavigation)
                .FirstOrDefaultAsync(m => m.MaHdn == id);
            if (chiTietHdn == null)
            {
                return NotFound();
            }

            return View(chiTietHdn);
        }

        // POST: Admin/ChiTietHdns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ChiTietHdns == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.ChiTietHdns'  is null.");
            }
            var chiTietHdn = await _context.ChiTietHdns.FindAsync(id);
            if (chiTietHdn != null)
            {
                _context.ChiTietHdns.Remove(chiTietHdn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HDN", "Admin");
        }

        private bool ChiTietHdnExists(string id)
        {
          return (_context.ChiTietHdns?.Any(e => e.MaHdn == id)).GetValueOrDefault();
        }
    }
}
