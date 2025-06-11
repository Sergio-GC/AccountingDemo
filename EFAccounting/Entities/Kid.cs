using System.Text.Json.Serialization;

namespace EFAccounting.Entities
{
    public class Kid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual List<Kid>? SiblingTo { get; set; } = new List<Kid>();
        [JsonIgnore]
        public virtual List<Kid>? SiblingFrom { get; set; } = new List<Kid>();
    }
}
