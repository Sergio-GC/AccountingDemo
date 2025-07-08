namespace DTO
{
    public class SiblingRelationship
    {
        public int FromKidId { get; set; }
        public int ToKidId { get; set; }

        public virtual Kid FromKid { get; set; }
        public virtual Kid ToKid { get; set; }
    }
}
