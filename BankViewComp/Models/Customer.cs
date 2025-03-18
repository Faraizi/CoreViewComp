using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankViewComp.Models
{
    [Table(nameof(Customer))]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required, MaxLength(50), Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "DATE"), Display(Name = "Date Created")]
        public DateTime DateCreate { get; set; }

        [Required, DataType(DataType.Currency), Display(Name = "Deposit")]
        public decimal Deposit { get; set; }

        [Required, Display(Name = "Is Active")]
        public bool IsAvailable { get; set; }

        [Required, DataType(DataType.ImageUrl), Display(Name = "Profile Photo")]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(Type))]
        public int Tid { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        public Types? Type { get; set; }
        public Account? Account { get; set; }
    }
}
