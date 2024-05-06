using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Domain.Entities.DBModels;
using WebProject.Domain.Entities.User;

using System.Data.Entity;

namespace WebProject.Domain
{
    public class Context : DbContext
    {
        public DbSet<UserEF> Users { get; set; }
        public DbSet<AdminEF> Admins { get; set; }
        public DbSet<OrderEF> Orders { get; set; }
        public DbSet<CartItemEF> CartItems { get; set; }
        public DbSet<ProductDataEF> Products { get; set; }
        public DbSet<CategoryTypeEF> CategoryTypes { get; set; }
        public Context() : base(Constants.SqlConnectionSQLServer) //подключение к mdl3.bayracraft.co.in
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // базовый OnModelCreating

            // добавить методы для инициилизации SuperAdmin
        }
    }
}
