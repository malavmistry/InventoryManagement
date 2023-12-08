using Inventory.Data;
using Inventory.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class ItemService
    {
        private readonly DatabaseContext _context;
        public ItemService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<bool> AddItems(Items item)
        {
            var items = await GetAllItems();
            if (items?.Any(x => x.UPC == item.UPC) ?? false)
                throw new ApplicationException($"Item with UPC-{item.UPC} already exists.");
            var result = await _context.AddItemAsync(item);
            await _context.GetAllAsync<Items>();
            return result;
        }
        public async Task<bool> UpdateItem(Transaction trans)
        {
            var item = (await GetAllItems()).FirstOrDefault(x => x.UPC == trans.ItemId);
            if (item == null)
                throw new ApplicationException($"Item with UPC-{item.UPC} not found");

            double price = 0;
            switch (trans.Type)
            {
                case TransType.Sale:
                    price = (item.SaleQty * item.SalePrice) + trans.TotalPrice;
                    item.RemainingQty = item.RemainingQty - trans.Qty;
                    item.SaleQty = item.SaleQty + trans.Qty;
                    item.SalePrice = Math.Ceiling((price / item.SaleQty) * 100) / 100;
                    break;

                case TransType.Purchase:
                    price = (item.PurchasedQty * item.CostPrice) + trans.TotalPrice;
                    item.RemainingQty = item.RemainingQty + trans.Qty;
                    item.PurchasedQty = item.PurchasedQty + trans.Qty;
                    item.CostPrice = Math.Ceiling((price / item.PurchasedQty) * 100) / 100;
                    break;
            }
            item.Diff = item.SalePrice - item.CostPrice;
            item.TotalDiff = item.Diff * item.SaleQty;

            var result = await UpdateItem(item);

            if (!result)
                throw new ApplicationException($"Item with UPC-{item.UPC} not updated");

            return result;
        }

        public async Task<bool> UpdateItem(Items item)
        {
            var result = await _context.UpdateItemAsync(item);

            if (!result)
                throw new ApplicationException($"Item with UPC-{item.UPC} not updated");

            return result;
        }

        public async Task<List<Items>> GetAllItems()
        {
            var result = await _context.GetAllAsync<Items>();
            return result;
        }
        public async Task<Items> GetItemByUPC(string UPC)
        {
            var result = await _context.GetFilteredAsync<Items>(x=> x.UPC == UPC);
            return result?.FirstOrDefault();
        }

        public async Task<bool> DeleteItem(Items item)
        {
            var result = await _context.DeleteItemAsync<Items>(item);
            return result;
        }
    }
}
