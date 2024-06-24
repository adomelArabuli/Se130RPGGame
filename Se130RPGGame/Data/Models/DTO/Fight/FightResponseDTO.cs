namespace Se130RPGGame.Data.Models.DTO.Fight
{
    public class FightResponseDTO
    {
        public ICollection<string> Log { get; set; } = new List<string>();
    }
}
