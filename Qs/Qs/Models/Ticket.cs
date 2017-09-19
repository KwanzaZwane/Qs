namespace Qs.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Ticket #")]
        public int TicketNumber { get; set; }

        [Required]
        [StringLength(128)]
        public string BranchId { get; set; }

        [Display(Name = "Function")]
        public int FunctionTypeId { get; set; }

        [Display(Name = "Ticket Issued")]
        public DateTime DateTimeIssued { get; set; }

        [Display(Name = "Ticket Accepted")]
        public DateTime? DateTimeHelped { get; set; }

        [Display(Name = "Ticket Closed")]
        public DateTime? DateTimeEnd { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual FunctionType FunctionType { get; set; }
    }
}
