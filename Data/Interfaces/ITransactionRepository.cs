using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<int> CreateTransactionAsync(Transaction transaction);

        IEnumerable<Transaction> Transactions { get; }

        Task<Transaction> GetPurchaseByIdAsync(int purchaseId);

        Task UpdatePurchaseAsync(Transaction purchase);

        Task DeletePurchasesAsync();
    }
}
