using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(Transaction transaction);

        IEnumerable<Transaction> Purchases { get; }

        Task<Transaction> GetPurchaseByIdAsync(int purchaseId);

        Task UpdatePurchaseAsync(Transaction purchase);

        Task DeletePurchasesAsync();
    }
}
