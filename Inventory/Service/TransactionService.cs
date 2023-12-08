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

        private readonly ItemService _itemService;

        public TransactionService(DatabaseContext context, ItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }

        public async Task<List<Transaction>> GetTransaction() {
            var result = await _context.GetAllAsync<Transaction>();

            return result;
        }

        public async Task<Transaction> GetTransactionByItemUPC(string itemUPC)
        {
            var result = await _context.GetFilteredAsync<Transaction>(x => x.ItemId == itemUPC);

            return result?.FirstOrDefault();
        }

        public async Task<Transaction> AddTransaction(Transaction trans)
        {
            var itemUpdated = await _itemService.UpdateItem(trans);
            if(!itemUpdated)
                throw new Exception("Something went wrong adding transction.");

            var lastItem = await _itemService.GetItemByUPC(trans.ItemId);
            trans.RemainingQty = lastItem.RemainingQty;

            var result = await _context.AddItemAsync<Transaction>(trans);
            if (!result)
                throw new Exception("Something went wrong adding transction.");

            return (await _context.GetFilteredAsync<Transaction>(x => x.Id == trans.Id))?.FirstOrDefault();
        }
    }
}
