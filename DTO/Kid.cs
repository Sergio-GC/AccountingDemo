namespace DTO
{
    public class Kid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual List<Kid>? SiblingTo { get; set; }
        public virtual List<Kid>? SiblingFrom { get; set; }
    }
}
