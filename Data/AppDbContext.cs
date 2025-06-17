using Microsoft.EntityFrameworkCore;
using PedidoConsumer.Models;

namespace PedidoConsumer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Pedido> Pedidos { get; set; }

    }
}