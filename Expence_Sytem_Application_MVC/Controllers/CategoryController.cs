﻿using Expence_Sytem_Application_MVC.DataAcces;
using Expence_Sytem_Application_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expence_Sytem_Application_MVC.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ExpenceDbContext _context;

        public CategoryController(ExpenceDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            if (_context.Categories != null)
            {
                var categories = await _context.Categories.ToListAsync();
                return View(categories);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }
        }



        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category());
            else
                return View(_context.Categories.Find(id));

        }

        //POST: Category/AddOrEdit
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    _context.Add(category);
                else
                    _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
