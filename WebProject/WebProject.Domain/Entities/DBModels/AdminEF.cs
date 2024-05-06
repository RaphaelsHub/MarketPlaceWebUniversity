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
    public class AdminEF
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderEF> Orders { get; set; }
        public ICollection<ProductDataEF> OwnProducts { get; set; }

        [ForeignKey("UserId")]
        public virtual UserEF AdminUser { get; set; }
    }
}
