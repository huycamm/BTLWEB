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
    public class LoaiGiaysController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public LoaiGiaysController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }


        // GET: Admin/LoaiGiays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiGiays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai")] LoaiGiay loaiGiay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiGiay);
                await _context.SaveChangesAsync();
                return RedirectToAction("TypeProducts", "Admin");
            }
            return View(loaiGiay);
        }

        // GET: Admin/LoaiGiays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.LoaiGiays == null)
            {
                return NotFound();
            }

            var loaiGiay = await _context.LoaiGiays.FindAsync(id);
            if (loaiGiay == null)
            {
                return NotFound();
            }
            return View(loaiGiay);
        }

        // POST: Admin/LoaiGiays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLoai,TenLoai")] LoaiGiay loaiGiay)
        {
            if (id != loaiGiay.MaLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiGiay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiGiayExists(loaiGiay.MaLoai))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("TypeProducts", "Admin");
            }
            return View(loaiGiay);
        }

        // GET: Admin/LoaiGiays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.LoaiGiays == null)
            {
                return NotFound();
            }

            var loaiGiay = await _context.LoaiGiays
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiGiay == null)
            {
                return NotFound();
            }

            return View(loaiGiay);
        }

        // POST: Admin/LoaiGiays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.LoaiGiays == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.LoaiGiays'  is null.");
            }
            var loaiGiay = await _context.LoaiGiays.FindAsync(id);
            if (loaiGiay != null)
            {
                _context.LoaiGiays.Remove(loaiGiay);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("TypeProducts", "Admin");
        }

        private bool LoaiGiayExists(string id)
        {
          return (_context.LoaiGiays?.Any(e => e.MaLoai == id)).GetValueOrDefault();
        }
    }
}
