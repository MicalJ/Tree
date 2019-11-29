using System;

namespace Tree.Database.DbModels
{
    public class BaseEntity<T> : EntityId<T>
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
