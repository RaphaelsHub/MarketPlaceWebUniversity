using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebProject.ModelAccessLayer.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        
        //Доделать
        public List<Category> AllCategories { get; set; }

        [Required(ErrorMessage = "ShortDescription is required.")]
        [StringLength(100, MinimumLength = 14, ErrorMessage = "Short Description must be at least 14 characters long")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "FullDescription is required.")]
        [StringLength(10000, MinimumLength = 20, ErrorMessage = "Full Description must be at least 20 characters long")]
        public string FullDescription { get; set; }

        //[Required(ErrorMessage = "PhotoUrl is required.")]
        public List<string> PhotoUrl { get; set; }// Создаем новый экземпляр при создании объекта Product

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public int Amount { get; set; }

        public Product() 
        {
            AllCategories = new List<Category>();
            PhotoUrl = new List<string>();
        }
    }
}
