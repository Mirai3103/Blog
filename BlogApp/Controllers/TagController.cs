using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.DAL;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
    [Authorize(Roles = nameof(Models.User.Role.ADMIN) + "," + nameof(Models.User.Role.CREATOR))]
    public class TagController : Controller
    {
        private readonly BlogContext _context;

        public TagController(BlogContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> AdminIndex()
        {
            return _context.Tags != null ?
                        View(await _context.Tags.Include(t => t.Posts).ToListAsync()) :
                        Problem("Entity set 'BlogContext.Tags'  is null.");
        }
        [HttpGet("Admin/[Controller]/[action]")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("Admin/[Controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Tag tag)
        {
            tag.Slug = BlogApp.Utils.Helper.Slugify(tag.Name);
            ModelState.Remove("Slug");
            var isExisted = TagExists(tag.Slug);
            if (isExisted)
            {
                ModelState.AddModelError("Name", "Vui lòng thử tên khác");
            }
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(tag);
        }
        [HttpGet("Admin/[Controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost("Admin/[Controller]/[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Slug,Name")] Tag tag)
        {
            if (id != tag.Slug)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Slug))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(tag);
        }



        [HttpGet("Admin/[Controller]/[action]/{id}")]
        [Authorize(Roles = nameof(Models.User.Role.ADMIN) + "," + nameof(Models.User.Role.CREATOR))]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Tags == null)
            {
                return Problem("Entity set 'BlogContext.Tags'  is null.");
            }
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        private bool TagExists(string id)
        {
            return (_context.Tags?.Any(e => e.Slug == id)).GetValueOrDefault();
        }
    }
}
