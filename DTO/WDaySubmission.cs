namespace DTO
{
    public class WDaySubmission
    {
        public DateOnly Date {  get; set; }
        public int PriceId { get; set; }
        public List<int> KidsIds { get; set; }
        public List<TimeOnly> Arrivals { get; set; }
        public List<TimeOnly> Departures {  get; set; }
    }
}
