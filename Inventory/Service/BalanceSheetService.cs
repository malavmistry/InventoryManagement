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
            var lastItem = _balanceSheet?.OrderByDescending(x => x.Id)
                                        .FirstOrDefault(x=> x.Itemid == trans.ItemId);

            switch (trans.Type)
            {
                case TransType.Sale:
                    item.RemainingQty = (lastItem?.RemainingQty ?? 0) - trans.Qty;
                    break;

                case TransType.Purchase:
                    item.RemainingQty = (lastItem?.RemainingQty ?? 0) + trans.Qty;
                    break;
            }
            var result = await _context.AddItemAsync<BalanceSheet>(item);
            if (!result)
                throw new Exception("Something went wrong adding transction.");
            await _itemService.UpdateItemQty(item.Itemid, item.RemainingQty);
            await GetBalanceSheetAsync(true);
            return result;
        }
    }
}
