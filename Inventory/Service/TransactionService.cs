using Inventory.Data;
using Inventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class TransactionService
    {
        private readonly DatabaseContext _context;

        private readonly BalanceSheetService _balanceSheet;

        public TransactionService(DatabaseContext context, BalanceSheetService balanceSheet)
        {
            _context = context;
            _balanceSheet = balanceSheet;
        }

        public async Task<List<Transaction>> GetTransaction() {
            var result = await _context.GetAllAsync<Transaction>();

            return result;
        }

        public async Task<Transaction> AddTransaction(Transaction trans)
        {
            var res = await _context.AddItemAsync(trans);
            if (!res)
                throw new Exception("Something went wrong adding transction.");
            var result = await _context.GetAllAsync<Transaction>();

            await _balanceSheet.AddSheetTransaction(trans);

            return (await _context.GetFilteredAsync<Transaction>(x=> x.Id == trans.Id))
                                  .FirstOrDefault();
        }
    }
}
