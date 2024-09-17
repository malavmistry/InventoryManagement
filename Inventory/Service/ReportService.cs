using Inventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class ReportService
    {
        private readonly TransactionService _transactionService;
        private readonly ItemService _itemService;

        public ReportService(TransactionService transactionService, ItemService itemService)
        {
            _transactionService = transactionService;
            _itemService = itemService;
        }

        public async Task<Report> GetGenericReport()
        {
            Report report = new Report();
            var transactions = await _transactionService.GetTransaction();
            var items = await _itemService.GetAllItems();
            foreach (var transaction in transactions)
            {
                switch (transaction.Type)
                {
                    case TransType.Sale:
                        report.QtySold += transaction.Qty;
                        report.Revenue += transaction.TotalPrice;
                        break;
                    case TransType.Purchase:
                        report.QtyPurchased += transaction.Qty;
                        report.CostPrice += transaction.TotalPrice;
                        break;
                }
            }
            report.Profit = items.Sum(x=> x.TotalDiff);
            return report;
        }
    }
}
