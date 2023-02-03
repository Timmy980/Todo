using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Todo.Data;
using Todo.Models;

namespace Todo.Pages.TodoItems
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Todo.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(Todo.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TodoItem TodoItem { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            TodoItem.UserId = userId;
            TodoItem.CreatedAt = DateTime.UtcNow;

            ModelState.SetModelValue("TodoItem.UserId", new ValueProviderResult(userId, CultureInfo.InvariantCulture));
            ModelState["TodoItem.UserId"].ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid || _context.TodoItem == null || TodoItem == null)
            {
                return Page();
            }

            _context.TodoItem.Add(TodoItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
