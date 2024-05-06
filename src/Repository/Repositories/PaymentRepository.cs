using Core.Entities;
using Core.Repositories;
using Repository.Data;

namespace Repository.Repositories
{
    public class PaymentRepository(AppDbContext db) : GenaricRepository<Payment>(db) , IPaymentRepository
    {
    }
}
