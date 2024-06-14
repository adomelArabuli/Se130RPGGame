using System.ComponentModel.DataAnnotations;

namespace Se130RPGGame.Data.Models.DTO.User
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
