namespace DTO
{
    public class InvoiceSummary
    {
        public DateOnly Date {  get; set; }
        public string KidName { get; set; }
        public string Label {  get; set; }
        public float Hours {  get; set; }
        public float Amount {  get; set; }
        public float TotalHours {  get; set; }
        public float TotalAmount {  get; set; }
    }
}
