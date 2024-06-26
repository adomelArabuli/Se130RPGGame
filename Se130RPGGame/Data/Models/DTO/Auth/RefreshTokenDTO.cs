using System.ComponentModel.DataAnnotations;

namespace Se130RPGGame.Data.Models.DTO.Auth
{
    public class RefreshTokenDTO
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
