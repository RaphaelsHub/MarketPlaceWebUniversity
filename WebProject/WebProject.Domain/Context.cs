using System.Data.Entity;
using System.Data.SqlClient;
using WebProject.Domain.DB;


namespace WebProject.Domain
{
    public class Context : DbContext
    {
        private static readonly string connectionString = BuildConnectionString();


        public DbSet<UserDataEF> Users { get; set; }
        public DbSet<CartItemDataEF> CartItems { get; set; }
        public DbSet<OrderDataEF> Orders { get; set; }
        public DbSet<ProductDataEF> Products { get; set; }
        public DbSet<CategoryDataEF> Category { get; set; }

        public Context() : base(connectionString) //подключение к mdl3.bayracraft.co.in
        {

        }

        private static string BuildConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = "webserverdb.database.windows.net",
                InitialCatalog = "WebDB",
                UserID = "sandrelion",
                Password = "VQfgcgZnEAj2pKapLPJT",
                ConnectTimeout = 30,
                Encrypt = true,
                TrustServerCertificate = true,
                ApplicationIntent = ApplicationIntent.ReadWrite,
                MultiSubnetFailover = false,
                Pooling = true
            }.ConnectionString;
        }
    }
}
