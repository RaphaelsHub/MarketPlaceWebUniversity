using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Domain.Entities.DBModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebProject.Domain;
using System.Data.Entity;


namespace WebProject.Domain.Entities.User
{
    public class UserEF
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime RegTime { get; set; }
        // Навигационное свойство для указания на заказы пользователя
        public ICollection<CartItemEF> CartItems { get; set; } //текущие товары будущего заказа
        public ICollection<OrderEF> Orders { get; set; } //уже выполненые заказы
    }
}
