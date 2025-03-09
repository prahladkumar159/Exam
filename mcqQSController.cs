using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Data;
using OnlineExam.Models;
using Microsoft.AspNetCore.Http;


namespace OnlineExam.Controllers
{
    public class mcqQSController : Controller
    {
        private readonly OnlineExamContext _context;
        private  object SessionName;
        public mcqQSController(OnlineExamContext context)
        {
            _context = context;
        }

        // GET: mcqQS
        public async Task<IActionResult> Index()
        {
            return View(await _context.mcqQS.ToListAsync());
        }

        // GET: mcqQS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mcqQS = await _context.mcqQS
                .FirstOrDefaultAsync(m => m.id == id);
            if (mcqQS == null)
            {
                return NotFound();
            }

            return View(mcqQS);
        }

        public async Task<IActionResult> Exam(int? ids)
        {
            if (ids == null)
            {
                ids = 1;
            }

            var mcqQS = await _context.mcqQS
                .FirstOrDefaultAsync(m => m.id == ids);
            mcqQS.ans = "1";
            if (mcqQS != null)
            {
                HttpContext.Session.SetString(SessionName, mcqQS);
            }
            if (mcqQS == null)
            {
                return NotFound();
            }
            TempData["id"] = ids;
            return View(mcqQS);
        }
        [HttpPost]
        public IActionResult Next(mcqQS mcqQS)
        {
            var id=Convert.ToInt32(TempData["id"])+1;
            return RedirectToAction("Exam",new { ids = id });
        }
        public IActionResult Previous()
        {
            var id =Convert.ToInt32(TempData["id"]);
            return RedirectToAction("Exam", new { ids = id-1 });
        }
        // GET: mcqQS/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: mcqQS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,course,qsId,qsNo,qs,op1,op2,op3,op4,ans,tag,eTime")] mcqQS mcqQS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mcqQS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mcqQS);
        }

        // GET: mcqQS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mcqQS = await _context.mcqQS.FindAsync(id);
            if (mcqQS == null)
            {
                return NotFound();
            }
            return View(mcqQS);
        }

        // POST: mcqQS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,course,qsId,qsNo,qs,op1,op2,op3,op4,ans,tag,eTime")] mcqQS mcqQS)
        {
            if (id != mcqQS.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mcqQS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mcqQSExists(mcqQS.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mcqQS);
        }

        // GET: mcqQS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mcqQS = await _context.mcqQS
                .FirstOrDefaultAsync(m => m.id == id);
            if (mcqQS == null)
            {
                return NotFound();
            }

            return View(mcqQS);
        }

        // POST: mcqQS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mcqQS = await _context.mcqQS.FindAsync(id);
            if (mcqQS != null)
            {
                _context.mcqQS.Remove(mcqQS);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mcqQSExists(int id)
        {
            return _context.mcqQS.Any(e => e.id == id);
        }
    }
}
