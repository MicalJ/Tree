namespace Tree.Database.DbModels
{
    public class Node : BaseEntity<int>
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
