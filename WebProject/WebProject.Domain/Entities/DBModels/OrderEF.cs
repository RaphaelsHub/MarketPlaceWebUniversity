using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Domain.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebProject.Domain;
using System.Data.Entity;

namespace WebProject.Domain.Entities.DBModels
{
    public class OrderEF
    {
        [Key]
        public int Order_Id { get; set; }
        public int UserId { get; set; } // Внешний ключ для связи с таблицей Users
        public int AdminId { get; set; } // Внешний ключ для связи с таблицей Users
        public DateTime OrderDate { get; set; }
        public ICollection<CartItemEF> CartItems { get; set; }

        [ForeignKey("UserId")]
        public virtual UserEF User { get; set; } // Навигационное свойство для доступа к связанному пользователю

        [ForeignKey("AdminId")]
        public virtual AdminEF Admin { get; set; } // Навигационное свойство для доступа к связанному владельцу

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public string StatusDelivery { get; set; }
    }
}
