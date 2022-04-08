using System.ComponentModel.DataAnnotations;

namespace Service.Account
{
    public class UserSignInResource
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
