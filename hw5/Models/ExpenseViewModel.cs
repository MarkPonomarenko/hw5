using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using hw6.Interfaces;

namespace hw6.Models
{
    public class ExpenseViewModel : IDatabaseEntity
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
        public string? CategoryName { get; set; }
    }
}
