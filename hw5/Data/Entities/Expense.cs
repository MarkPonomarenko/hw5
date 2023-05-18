using hw6.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hw6.Data.Entities
{
    public class Expense : IDatabaseEntity
    {
        [Key]
        [Column("exp_id")]
        public int Id { get; set; }
        [Column("exp_cost")]
        public decimal Cost { get; set; }
        [Column("exp_comment")]
        public string? Comment { get; set; }
        [Column("exp_datetime")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Column("exp_categ")]
        public int CategoryId { get; set; }
    }
}
