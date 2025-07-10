using AlHafiz.DTOs;
using AlHafiz.Enums;
using AlHafiz.Models;
using AlHafiz.Services.IRepository.Base;

namespace AlHafiz.Services.IRepository
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<Voucher> GetVoucherWithDetailsAsync(int id);
        Task<IEnumerable<Voucher>> GetVouchersWithDetailsAsync();
        Task<IEnumerable<Voucher>> FilterVouchersAsync(VoucherFilterDto filter);
        Task<Voucher> CreateVoucherWithItemsAsync(Voucher voucher, IEnumerable<VoucherItem> voucherItems);
        Task<Voucher> UpdateVoucherWithItemsAsync(Voucher voucher, IEnumerable<VoucherItem> voucherItems);
        Task<IEnumerable<Voucher>> FilterVouchersByPaymentTypeAndDateAsync(PaymentType paymentType, DateTime? fromDate, DateTime? toDate);
        Task SetItemRateForCustomerAsync(int customerId, int itemId, decimal rate);

    }
}
