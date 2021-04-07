using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventsProject.Models;
using EventsProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EventsProject.Pages
{
    [Authorize]
    public class JoinEventModel : PageModel
    {
        private readonly EventContext _context;
        private readonly UserManager<EventsUser> _userManager;

        public JoinEventModel(EventContext context, UserManager<EventsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        [BindProperty]
        public Event Event { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);


            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            
            Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (Event == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(e => e.JoinedEvents)
                .FirstOrDefaultAsync();

            if (!user.JoinedEvents.Contains(Event))
            {
                user.JoinedEvents.Add(Event);
                await _context.SaveChangesAsync();
            }


            return Page();

        }

    }
}

