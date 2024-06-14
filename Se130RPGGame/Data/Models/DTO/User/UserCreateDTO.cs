namespace Se130RPGGame.Data.Models.DTO.User
{
    public class UserCreateDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<int>? Roles { get; set; }
    }
}
