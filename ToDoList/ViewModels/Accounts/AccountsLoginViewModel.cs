using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels.Accounts
{
    public class AccountsLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Password must contains more then 3 symbols")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
