using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecretSantaMVC.Data;
using SecretSantaMVC.Models;

namespace SecretSantaMVC.Controllers
{
    public class CommentModelsController : Controller
    {
        private readonly SecretSantaMVCContext _context;

        public CommentModelsController(SecretSantaMVCContext context)
        {
            // Uses dependency injection to inject database context into controller
            _context = context;
        }

        // GET: CommentModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.CommentModel.ToListAsync());
        }

        // GET: CommentModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // GET: CommentModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CommentModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatePublished,Title,Author,Text")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentModel);
        }

        // GET: CommentModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }
            return View(commentModel);
        }

        // POST: CommentModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatePublished,Title,Author,Text")] CommentModel commentModel)
        {
            if (id != commentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentModelExists(commentModel.Id))
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
            return View(commentModel);
        }

        // GET: CommentModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // POST: CommentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentModel = await _context.CommentModel.FindAsync(id);
            _context.CommentModel.Remove(commentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentModelExists(int id)
        {
            return _context.CommentModel.Any(e => e.Id == id);
        }
    }
}
