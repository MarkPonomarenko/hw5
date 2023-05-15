using hw6.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace hw6.Data.Entities
{
    public class Category : IDatabaseEntity
    {
        [Key]
        [Column("categ_id")]
        public int Id { get; set; }
        [Column("categ_name")]
        public string Name { get; set; }
        public ICollection<Expense>? Expenses {get; set; } 
    }
}
