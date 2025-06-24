using AlHafiz.DTOs;
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
    }
}
