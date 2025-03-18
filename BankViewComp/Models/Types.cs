using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankViewComp.Models
{
    [Table(nameof(Types))]
    public class Types
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Tid { get; set; }

        [Required, MaxLength(50), Display(Name = "Account type")]
        public string AccType { get; set; }

        public IList<Customer>? Customers { get; set; }
    }
}
