using AlHafiz.AppDbContext;
using AlHafiz.Enums;
using AlHafiz.Models;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.SeedData
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Check if database already has data
            if (context.Customers.Any())
            {
                return; // Database has been seeded already
            }

            // Seed data
            await SeedCustomers(context);
            await SeedItems(context);
            await SeedBanks(context);
            await SeedExpenseHeads(context);
            await SeedVouchers(context);
            await SeedCashTransactions(context);
        }

        private static async Task SeedCustomers(ApplicationDbContext context)
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Ahmed Trading Co.", CreatedAt = DateTime.Now.AddDays(-60) },
                new Customer { Name = "Malik Enterprises", CreatedAt = DateTime.Now.AddDays(-58) },
                new Customer { Name = "Khan Suppliers", CreatedAt = DateTime.Now.AddDays(-55) },
                new Customer { Name = "Sohail Brothers", CreatedAt = DateTime.Now.AddDays(-50) },
                new Customer { Name = "Iqbal & Sons", CreatedAt = DateTime.Now.AddDays(-45) },
                new Customer { Name = "Javed Traders", CreatedAt = DateTime.Now.AddDays(-40) },
                new Customer { Name = "Shahid Imports", CreatedAt = DateTime.Now.AddDays(-38) },
                new Customer { Name = "Rehman Exports", CreatedAt = DateTime.Now.AddDays(-35) },
                new Customer { Name = "Younis Agricultural", CreatedAt = DateTime.Now.AddDays(-30) },
                new Customer { Name = "Rizwan Wholesalers", CreatedAt = DateTime.Now.AddDays(-28) },
                new Customer { Name = "Arshad & Co.", CreatedAt = DateTime.Now.AddDays(-25) },
                new Customer { Name = "Zubair Trading", CreatedAt = DateTime.Now.AddDays(-20) },
                new Customer { Name = "Tariq Industries", CreatedAt = DateTime.Now.AddDays(-15) },
                new Customer { Name = "Waseem Retailers", CreatedAt = DateTime.Now.AddDays(-10) },
                new Customer { Name = "Nasir Supplies", CreatedAt = DateTime.Now.AddDays(-5) }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedItems(ApplicationDbContext context)
        {
            var items = new List<Item>
            {
                new Item { Name = "Rice Basmati", CreatedAt = DateTime.Now.AddDays(-60) },
                new Item { Name = "Wheat Flour", CreatedAt = DateTime.Now.AddDays(-58) },
                new Item { Name = "Sugar", CreatedAt = DateTime.Now.AddDays(-55) },
                new Item { Name = "Lentils (Daal Chana)", CreatedAt = DateTime.Now.AddDays(-50) },
                new Item { Name = "Lentils (Daal Masoor)", CreatedAt = DateTime.Now.AddDays(-45) },
                new Item { Name = "Lentils (Daal Moong)", CreatedAt = DateTime.Now.AddDays(-40) },
                new Item { Name = "Cooking Oil", CreatedAt = DateTime.Now.AddDays(-38) },
                new Item { Name = "Ghee", CreatedAt = DateTime.Now.AddDays(-35) },
                new Item { Name = "Salt", CreatedAt = DateTime.Now.AddDays(-30) },
                new Item { Name = "Red Chili Powder", CreatedAt = DateTime.Now.AddDays(-28) },
                new Item { Name = "Turmeric Powder", CreatedAt = DateTime.Now.AddDays(-25) },
                new Item { Name = "Cumin Seeds", CreatedAt = DateTime.Now.AddDays(-20) },
                new Item { Name = "Coriander Seeds", CreatedAt = DateTime.Now.AddDays(-15) },
                new Item { Name = "Black Pepper", CreatedAt = DateTime.Now.AddDays(-10) },
                new Item { Name = "Cardamom", CreatedAt = DateTime.Now.AddDays(-5) }
            };

            await context.Items.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        private static async Task SeedBanks(ApplicationDbContext context)
        {
            var banks = new List<Bank>
            {
                new Bank { Name = "Meezan Bank", CreatedAt = DateTime.Now.AddDays(-60) },
                new Bank { Name = "UBL", CreatedAt = DateTime.Now.AddDays(-58) },
                new Bank { Name = "MCB", CreatedAt = DateTime.Now.AddDays(-55) },
                new Bank { Name = "HBL", CreatedAt = DateTime.Now.AddDays(-50) },
                new Bank { Name = "Bank Alfalah", CreatedAt = DateTime.Now.AddDays(-45) },
                new Bank { Name = "Faysal Bank", CreatedAt = DateTime.Now.AddDays(-40) },
                new Bank { Name = "Bank Al Habib", CreatedAt = DateTime.Now.AddDays(-38) },
                new Bank { Name = "Allied Bank", CreatedAt = DateTime.Now.AddDays(-35) },
                new Bank { Name = "Dubai Islamic Bank", CreatedAt = DateTime.Now.AddDays(-30) },
                new Bank { Name = "Askari Bank", CreatedAt = DateTime.Now.AddDays(-28) },
                new Bank { Name = "Soneri Bank", CreatedAt = DateTime.Now.AddDays(-25) },
                new Bank { Name = "Bank of Punjab", CreatedAt = DateTime.Now.AddDays(-20) },
                new Bank { Name = "JS Bank", CreatedAt = DateTime.Now.AddDays(-15) },
                new Bank { Name = "Summit Bank", CreatedAt = DateTime.Now.AddDays(-10) },
                new Bank { Name = "Silk Bank", CreatedAt = DateTime.Now.AddDays(-5) }
            };

            await context.Banks.AddRangeAsync(banks);
            await context.SaveChangesAsync();
        }

        private static async Task SeedExpenseHeads(ApplicationDbContext context)
        {
            var expenseHeads = new List<ExpenseHead>
            {
                new ExpenseHead { Name = "Khana (Food)", CreatedAt = DateTime.Now.AddDays(-60) },
                new ExpenseHead { Name = "Fuel", CreatedAt = DateTime.Now.AddDays(-58) },
                new ExpenseHead { Name = "Salary", CreatedAt = DateTime.Now.AddDays(-55) },
                new ExpenseHead { Name = "Electricity Bill", CreatedAt = DateTime.Now.AddDays(-50) },
                new ExpenseHead { Name = "Gas Bill", CreatedAt = DateTime.Now.AddDays(-45) },
                new ExpenseHead { Name = "Water Bill", CreatedAt = DateTime.Now.AddDays(-40) },
                new ExpenseHead { Name = "Internet", CreatedAt = DateTime.Now.AddDays(-38) },
                new ExpenseHead { Name = "Rent", CreatedAt = DateTime.Now.AddDays(-35) },
                new ExpenseHead { Name = "Maintenance", CreatedAt = DateTime.Now.AddDays(-30) },
                new ExpenseHead { Name = "Transportation", CreatedAt = DateTime.Now.AddDays(-28) },
                new ExpenseHead { Name = "Office Supplies", CreatedAt = DateTime.Now.AddDays(-25) },
                new ExpenseHead { Name = "Cleaning", CreatedAt = DateTime.Now.AddDays(-20) },
                new ExpenseHead { Name = "Security", CreatedAt = DateTime.Now.AddDays(-15) },
                new ExpenseHead { Name = "Miscellaneous", CreatedAt = DateTime.Now.AddDays(-10) },
                new ExpenseHead { Name = "Taxes", CreatedAt = DateTime.Now.AddDays(-5) }
            };

            await context.ExpenseHeads.AddRangeAsync(expenseHeads);
            await context.SaveChangesAsync();
        }

        private static async Task SeedVouchers(ApplicationDbContext context)
        {
            var random = new Random();
            var customers = await context.Customers.ToListAsync();
            var items = await context.Items.ToListAsync();
            var banks = await context.Banks.ToListAsync();
            var expenseHeads = await context.ExpenseHeads.ToListAsync();

            // Generate Purchase Vouchers (15)
            var purchaseVouchers = new List<Voucher>();
            for (int i = 0; i < 15; i++)
            {
                var customer = customers[random.Next(0, customers.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var purchaseVoucher = new Voucher
                {
                    VoucherType = VoucherType.Purchase,
                    CustomerId = customer.Id,
                    Customer = customer,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Purchase payment to {customer.Name}",
                    GariNo = $"G-{random.Next(1000, 9999)}",
                    Details = $"Purchase from {customer.Name}",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60)),
                    VoucherItems = new List<VoucherItem>()
                };

                // Add 1-3 items to each purchase voucher
                var itemCount = random.Next(1, 4);
                var selectedItems = items.OrderBy(x => Guid.NewGuid()).Take(itemCount).ToList();

                foreach (var item in selectedItems)
                {
                    var weight = random.Next(100, 1000);
                    var kat = random.Next(5, 20);
                    var netWeight = weight - kat;
                    var desiMan = Math.Round(netWeight * 0.026, 3); // Convert to desi man
                    var rate = random.Next(200, 500);
                    var amount = Math.Round(desiMan * rate, 2);

                    purchaseVoucher.VoucherItems.Add(new VoucherItem
                    {
                        ItemId = item.Id,
                        Item = item,
                        Weight = weight,
                        Kat = kat,
                        NetWeight = netWeight,
                        DesiMan = (decimal)desiMan,
                        Rate = rate,
                        Amount = (decimal)amount,
                        CreatedAt = purchaseVoucher.CreatedAt
                    });
                }

                purchaseVouchers.Add(purchaseVoucher);
            }

            await context.Vouchers.AddRangeAsync(purchaseVouchers);
            await context.SaveChangesAsync();

            // Create initial stock based on purchases
            foreach (var voucher in purchaseVouchers)
            {
                foreach (var item in voucher.VoucherItems)
                {
                    var existingStock = await context.Stocks.FirstOrDefaultAsync(s => s.ItemId == item.ItemId);
                    if (existingStock != null)
                    {
                        existingStock.Quantity += item.NetWeight;
                        existingStock.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        await context.Stocks.AddAsync(new Stock
                        {
                            ItemId = item.ItemId,
                            Item = item.Item,
                            Quantity = item.NetWeight,
                            CreatedAt = DateTime.Now
                        });
                    }
                }
            }

            await context.SaveChangesAsync();

            // Generate Sale Vouchers (15)
            var saleVouchers = new List<Voucher>();
            for (int i = 0; i < 15; i++)
            {
                var customer = customers[random.Next(0, customers.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var saleVoucher = new Voucher
                {
                    VoucherType = VoucherType.Sale,
                    CustomerId = customer.Id,
                    Customer = customer,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Sale payment from {customer.Name}",
                    GariNo = $"G-{random.Next(1000, 9999)}",
                    Details = $"Sale to {customer.Name}",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60)),
                    VoucherItems = new List<VoucherItem>()
                };

                // Add 1-2 items to each sale voucher with stock check
                var itemCount = random.Next(1, 3);
                var availableItems = await context.Stocks
                    .Where(s => s.Quantity > 50) // Only items with stock
                    .Select(s => s.ItemId)
                    .ToListAsync();

                if (availableItems.Count == 0)
                    continue;

                // Take random items from available stock
                var selectedItemIds = availableItems
                    .OrderBy(x => Guid.NewGuid())
                    .Take(Math.Min(itemCount, availableItems.Count))
                    .ToList();

                foreach (var itemId in selectedItemIds)
                {
                    var stock = await context.Stocks
                        .Include(s => s.Item)
                        .FirstOrDefaultAsync(s => s.ItemId == itemId);

                    if (stock == null || stock.Quantity <= 0)
                        continue;

                    // FIX: Ensure we don't sell more than we have and minValue <= maxValue
                    // Calculate a valid weight range
                    var maxPossibleWeight = Math.Max((int)stock.Quantity - 10, 10);  // Ensure positive
                    var minWeight = Math.Min(10, maxPossibleWeight);  // Minimum 10kg or less if that's all we have
                    var maxWeight = Math.Min(maxPossibleWeight, 500); // Cap at 500kg

                    // Ensure minWeight <= maxWeight
                    if (minWeight > maxWeight)
                        minWeight = maxWeight;

                    var weight = random.Next(minWeight, maxWeight + 1); // +1 because Next is exclusive of maxValue
                    var kat = Math.Min(random.Next(1, 10), weight / 2); // Ensure Kat doesn't exceed half of weight
                    var netWeight = weight - kat;
                    var desiMan = Math.Round(netWeight * 0.026, 3); // Convert to desi man
                    var rate = random.Next(300, 600); // Higher rate for sale
                    var amount = Math.Round(desiMan * rate, 2);

                    saleVoucher.VoucherItems.Add(new VoucherItem
                    {
                        ItemId = itemId,
                        Item = stock.Item,
                        Weight = weight,
                        Kat = kat,
                        NetWeight = netWeight,
                        DesiMan = (decimal)desiMan,
                        Rate = rate,
                        Amount = (decimal)amount,
                        CreatedAt = saleVoucher.CreatedAt
                    });

                    // Update stock quantity
                    stock.Quantity -= netWeight;
                    stock.UpdatedAt = DateTime.Now;
                }

                if (saleVoucher.VoucherItems.Count > 0)
                {
                    saleVouchers.Add(saleVoucher);
                }
            }

            await context.Vouchers.AddRangeAsync(saleVouchers);
            await context.SaveChangesAsync();

            // Generate Expense Vouchers (15)
            var expenseVouchers = new List<Voucher>();
            for (int i = 0; i < 15; i++)
            {
                var expenseHead = expenseHeads[random.Next(0, expenseHeads.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var expenseVoucher = new Voucher
                {
                    VoucherType = VoucherType.Expense,
                    ExpenseHeadId = expenseHead.Id,
                    ExpenseHead = expenseHead,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Expense payment for {expenseHead.Name}",
                    Amount = random.Next(1000, 10000),
                    Details = $"Expense for {expenseHead.Name}",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
                };

                expenseVouchers.Add(expenseVoucher);
            }

            await context.Vouchers.AddRangeAsync(expenseVouchers);
            await context.SaveChangesAsync();

            // Generate Hazri Vouchers (15)
            var hazriVouchers = new List<Voucher>();
            for (int i = 0; i < 15; i++)
            {
                var expenseHead = expenseHeads.FirstOrDefault(e => e.Name == "Salary") ?? expenseHeads[random.Next(0, expenseHeads.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var hazriVoucher = new Voucher
                {
                    VoucherType = VoucherType.Hazri,
                    ExpenseHeadId = expenseHead.Id,
                    ExpenseHead = expenseHead,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Hazri payment for {expenseHead.Name}",
                    Amount = random.Next(500, 3000),
                    Details = $"Hazri for {expenseHead.Name}",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
                };

                hazriVouchers.Add(hazriVoucher);
            }

            await context.Vouchers.AddRangeAsync(hazriVouchers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCashTransactions(ApplicationDbContext context)
        {
            var random = new Random();
            var customers = await context.Customers.ToListAsync();
            var banks = await context.Banks.ToListAsync();

            // Generate Cash Paid Transactions (15)
            var cashPaidTransactions = new List<CashTransaction>();
            for (int i = 0; i < 15; i++)
            {
                var customer = customers[random.Next(0, customers.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var cashPaid = new CashTransaction
                {
                    CustomerId = customer.Id,
                    Customer = customer,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Cash paid to {customer.Name}",
                    Amount = random.Next(5000, 50000),
                    IsCashReceived = false,
                    Details = $"Payment to {customer.Name} for outstanding dues",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
                };

                cashPaidTransactions.Add(cashPaid);
            }

            await context.CashTransactions.AddRangeAsync(cashPaidTransactions);
            await context.SaveChangesAsync();

            // Generate Cash Received Transactions (15)
            var cashReceivedTransactions = new List<CashTransaction>();
            for (int i = 0; i < 15; i++)
            {
                var customer = customers[random.Next(0, customers.Count)];
                var paymentType = (PaymentType)random.Next(1, 4);
                var bank = paymentType == PaymentType.Bank ? banks[random.Next(0, banks.Count)] : null;

                var cashReceived = new CashTransaction
                {
                    CustomerId = customer.Id,
                    Customer = customer,
                    PaymentType = paymentType,
                    BankId = paymentType == PaymentType.Bank ? bank?.Id : null,
                    Bank = paymentType == PaymentType.Bank ? bank : null,
                    PaymentDetails = $"Cash received from {customer.Name}",
                    Amount = random.Next(5000, 50000),
                    IsCashReceived = true,
                    Details = $"Payment from {customer.Name} for outstanding sales",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
                };

                cashReceivedTransactions.Add(cashReceived);
            }

            await context.CashTransactions.AddRangeAsync(cashReceivedTransactions);
            await context.SaveChangesAsync();
        }
    }
}
