using GlobalBankApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
namespace GlobalBankApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ContaBancaria> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
    }
}