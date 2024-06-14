namespace Se130RPGGame.Data.Models.DTO.User
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public ICollection<int>? Roles { get; set; }
    }
}
