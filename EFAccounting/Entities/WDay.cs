namespace EFAccounting.Entities
{
    public class WDay
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly? Arrival { get; set; }
        public TimeOnly? Departure { get; set; }

        public virtual required Kid Kid { get; set; }
        public virtual required Price Price { get; set; }
    }
}
