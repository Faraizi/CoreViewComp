using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BankViewComp.Models
{

        [Table(nameof(Account))]
        public class Account
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int AccountId { get; set; }

            [Required, MaxLength(50), Display(Name = "Branch Name")]
            public string Branch { get; set; }

            [AllowNull]
            public List<Customer>? Customers { get; set; }
        }
    
}
