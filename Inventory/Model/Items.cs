using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Items
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RemainingQty { get; set; }
        public int PurchasedQty { get; set; }
        public int SaleQty { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double Diff { get; set; }
        public double TotalDiff { get; set; }

    }
}
