using hw6.Data.Entities;
using hw6.Interfaces;

namespace hw6.Models
{
    public class NewExpenseViewModel : IDatabaseEntity
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string? Comment { get; set; }
        public int CategoryId { get; set; } 
        public List<Category>? AvailableCategories { get; set; }
    }
}
