namespace Se130RPGGame.Data.Models.DTO.Auth
{
    public class AuthResultDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Confirmed { get; set; }
    }
}
