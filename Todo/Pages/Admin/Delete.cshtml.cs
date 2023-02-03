using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly Todo.Data.ApplicationDbContext _context;

        public DeleteModel(Todo.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public TodoItem TodoItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }

            var todoitem = await _context.TodoItem.FirstOrDefaultAsync(m => m.Id == id);

            if (todoitem == null)
            {
                return NotFound();
            }
            else 
            {
                TodoItem = todoitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }
            var todoitem = await _context.TodoItem.FindAsync(id);

            if (todoitem != null)
            {
                TodoItem = todoitem;
                _context.TodoItem.Remove(TodoItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
