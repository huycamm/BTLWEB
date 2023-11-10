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
    
    public class ChiTietHdbsController : Controller
    {
        private readonly Qlbangiaynhom7Context _context;

        public ChiTietHdbsController(Qlbangiaynhom7Context context)
        {
            _context = context;
        }
        public IActionResult ChiTietHDB(String maHD)
        {
			List<ChiTietHdb> temp = _context.ChiTietHdbs.Where(x => x.MaHdb == maHD).ToList();
			return View(temp);
        }
		public IActionResult XoaCTHDB(String maHD, String maGiay)
		{
			_context.Remove(_context.ChiTietHdbs.Find(maHD, maGiay));
			_context.SaveChanges();
			return RedirectToAction("ChiTietHDB", "ChiTietHdbs", new { maHD = maHD });
		}

		// GET: Admin/ChiTietHdbs
		public async Task<IActionResult> Index()
        {
            var qlbangiaynhom7Context = _context.ChiTietHdbs.Include(c => c.MaGiayNavigation).Include(c => c.MaHdbNavigation);
            return View(await qlbangiaynhom7Context.ToListAsync());
        }

        // GET: Admin/ChiTietHdbs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ChiTietHdbs == null)
            {
                return NotFound();
            }

            var chiTietHdb = await _context.ChiTietHdbs
                .Include(c => c.MaGiayNavigation)
                .Include(c => c.MaHdbNavigation)
                .FirstOrDefaultAsync(m => m.MaHdb == id);
            if (chiTietHdb == null)
            {
                return NotFound();
            }

            return View(chiTietHdb);
        }

        // GET: Admin/ChiTietHdbs/Create
        public IActionResult Create()
        {
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay");
            ViewData["MaHdb"] = new SelectList(_context.HoaDonBans, "MaHdb", "MaHdb");
            return View();
        }

        // POST: Admin/ChiTietHdbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHdb,MaGiay,SoLuong,KhuyenMai")] ChiTietHdb chiTietHdb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietHdb);
                await _context.SaveChangesAsync();
                return RedirectToAction("HDB", "Admin");
            }
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdb.MaGiay);
            ViewData["MaHdb"] = new SelectList(_context.HoaDonBans, "MaHdb", "MaHdb", chiTietHdb.MaHdb);
            return View(chiTietHdb);
        }

        // GET: Admin/ChiTietHdbs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ChiTietHdbs == null)
            {
                return NotFound();
            }

            var chiTietHdb = await _context.ChiTietHdbs.FindAsync(id);
            if (chiTietHdb == null)
            {
                return NotFound();
            }
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdb.MaGiay);
            ViewData["MaHdb"] = new SelectList(_context.HoaDonBans, "MaHdb", "MaHdb", chiTietHdb.MaHdb);
            return View(chiTietHdb);
        }

        // POST: Admin/ChiTietHdbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHdb,MaGiay,SoLuong,KhuyenMai")] ChiTietHdb chiTietHdb)
        {
            if (id != chiTietHdb.MaHdb)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHdb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHdbExists(chiTietHdb.MaHdb))
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
            ViewData["MaGiay"] = new SelectList(_context.Giays, "MaGiay", "MaGiay", chiTietHdb.MaGiay);
            ViewData["MaHdb"] = new SelectList(_context.HoaDonBans, "MaHdb", "MaHdb", chiTietHdb.MaHdb);
            return View(chiTietHdb);
        }

        // GET: Admin/ChiTietHdbs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ChiTietHdbs == null)
            {
                return NotFound();
            }

            var chiTietHdb = await _context.ChiTietHdbs
                .Include(c => c.MaGiayNavigation)
                .Include(c => c.MaHdbNavigation)
                .FirstOrDefaultAsync(m => m.MaHdb == id);
            if (chiTietHdb == null)
            {
                return NotFound();
            }

            return View(chiTietHdb);
        }

		// POST: Admin/ChiTietHdbs/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( string id, string id1)
        {
            if (_context.ChiTietHdbs == null)
            {
                return Problem("Entity set 'Qlbangiaynhom7Context.ChiTietHdbs'  is null.");
            }
            var chiTietHdb = await _context.ChiTietHdbs.FindAsync(id1, id1);
            if (chiTietHdb != null)
            {
                _context.ChiTietHdbs.Remove(chiTietHdb);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HDB", "Admin");
        }

        private bool ChiTietHdbExists(string id)
        {
          return (_context.ChiTietHdbs?.Any(e => e.MaHdb == id)).GetValueOrDefault();
        }
    }
}
