namespace Se130RPGGame.Data.Models.DTO.User
{
    public class UserDetailsDTO
    {
        public UserDetailsDTO()
        {
            RoleNames = new List<string>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<string> RoleNames { get; set; }
    }
}
