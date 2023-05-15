using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using hw6.Interfaces;

namespace hw6.Models
{
    public class CategoryViewModel : IDatabaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
