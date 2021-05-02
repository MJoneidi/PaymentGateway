using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime ModifiedDate { get; set; }
    }
}