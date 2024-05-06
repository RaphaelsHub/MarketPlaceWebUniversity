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
    public class CartItemEF
    {
        [Key]
        public int CartItemId { get; set; }
        public int UserId { get; set; } // Внешний ключ для связи с таблицей пользователей
        public int ProductId { get; set; } // Внешний ключ для связи с таблицей товаров


        [ForeignKey("ProductId")]
        public virtual ProductDataEF Product { get; set; } // Навигационное свойство для доступа к связанному товару
        public int Quantity { get; set; } // Количество конкретного товара

        [ForeignKey("UserId")]
        public virtual UserEF User { get; set; } // Навигационное свойство для доступа к связанному пользователю

    }
}
