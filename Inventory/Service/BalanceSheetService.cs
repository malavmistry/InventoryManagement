using Inventory.Data;
using Inventory.Model;
using System;
using System.Collections.Generic;

namespace Inventory.Service
{
    public class BalanceSheetService
    {
        private readonly DatabaseContext _context;
        private readonly ItemService _itemService;
        private static List<BalanceSheet> _balanceSheet;
        public BalanceSheetService(DatabaseContext context, ItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }
        public async Task<List<BalanceSheet>> GetBalanceSheetAsync(bool refreshRequired = false)
        {
            if(_balanceSheet == null || _balanceSheet.Count()<1 || refreshRequired)
                _balanceSheet = await _context.GetAllAsync<BalanceSheet>();
            return _balanceSheet;
        }

        public async Task<bool> AddSheetTransaction(Transaction trans)
        {
            BalanceSheet item = new BalanceSheet();
            item.Date = trans.Date;
            item.TransType = trans.Type;
            item.Itemid = trans.ItemId;
            item.SellerId = trans.SellerId;
            item.Price = trans.Price;
            item.TotalPrice = trans.TotalPrice;
            item.Qty = trans.Qty;
            var lastItem = (await _itemService.GetAllItems()).FirstOrDefault(x=> x.UPC == trans.ItemId);
            double price = 0;
            switch (trans.Type)
            {
                case TransType.Sale:
                    price = (lastItem.SaleQty * lastItem.SalePrice) + item.TotalPrice;
                    lastItem.RemainingQty = lastItem.RemainingQty - trans.Qty;
                    lastItem.SaleQty = lastItem.SaleQty + trans.Qty;
                    lastItem.SalePrice = Math.Round(price / lastItem.SaleQty);

                    break;

                case TransType.Purchase:
                    price = (lastItem.PurchasedQty * lastItem.CostPrice) + item.TotalPrice;
                    lastItem.RemainingQty = lastItem.RemainingQty + trans.Qty;
                    lastItem.PurchasedQty = lastItem.PurchasedQty + trans.Qty;
                    lastItem.CostPrice = Math.Round(price / lastItem.PurchasedQty);
                    break;
            }
            lastItem.Diff = lastItem.SalePrice - lastItem.CostPrice;
            lastItem.TotalDiff = lastItem.Diff * lastItem.SaleQty;

            item.RemainingQty = lastItem.RemainingQty;
            var result = await _context.AddItemAsync<BalanceSheet>(item);
            if (!result)
                throw new Exception("Something went wrong adding transction.");
            await _itemService.UpdateItem(lastItem);
            await GetBalanceSheetAsync(true);
            return result;
        }
    }
}
