using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
