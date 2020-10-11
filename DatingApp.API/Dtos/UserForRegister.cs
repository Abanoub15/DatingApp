using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegister
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [StringLength(10,MinimumLength=2,ErrorMessage="You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}