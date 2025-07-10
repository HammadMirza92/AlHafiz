using AlHafiz.Models;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.AppDbContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
         
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<ExpenseHead> ExpenseHeads { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherItem> VoucherItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }
        public DbSet<BalanceTransaction> BalanceTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Customer)
                .WithMany(c => c.Vouchers)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Bank)
                .WithMany(b => b.Vouchers)
                .HasForeignKey(v => v.BankId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.ExpenseHead)
                .WithMany(e => e.Vouchers)
                .HasForeignKey(v => v.ExpenseHeadId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<VoucherItem>()
                .HasOne(vi => vi.Voucher)
                .WithMany(v => v.VoucherItems)
                .HasForeignKey(vi => vi.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<VoucherItem>()
                .HasOne(vi => vi.Item)
                .WithMany(i => i.VoucherItems)
                .HasForeignKey(vi => vi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Item)
                .WithMany(i => i.Stocks)
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<CashTransaction>()
                .HasOne(ct => ct.Customer)
                .WithMany(c => c.CashTransactions)
                .HasForeignKey(ct => ct.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<CashTransaction>()
                .HasOne(ct => ct.Bank)
                .WithMany(b => b.CashTransactions)
                .HasForeignKey(ct => ct.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Stocks)
                .WithOne(s => s.Item)
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Vouchers)
                .WithOne(v => v.Customer)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // For Customers and CashTransactions relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CashTransactions)
                .WithOne(ct => ct.Customer)
                .HasForeignKey(ct => ct.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
