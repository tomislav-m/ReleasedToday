using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReleasedToday.Data;
using ReleasedToday.Models;

namespace ReleasedToday.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(string id)
        {
            var album = await _context.Albums.SingleOrDefaultAsync(a => a.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        [HttpPost]
        public IActionResult Edit([Bind("Id,Artist,Name,ReleaseDate,ImageMega,ImageLarge,ImageExtraLarge")] Album album)
        {
            if (album == null)
            {
                return NotFound();
            }

            _context.Update(album);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home", "");
        }
    }
}