using System.Collections.Generic;

namespace ReadAllCopyBinary.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public List<int?> GenreIds { get; set; }
        public int? Episodes { get; set; }
        public double? Rating { get; set; }
        public int? Members { get; set; }
    }
}