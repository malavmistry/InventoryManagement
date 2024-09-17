using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Report
    {
        public int QtySold { get; set; } = 0;
        public int QtyPurchased { get; set; } = 0;
        public double CostPrice { get; set; } = 0;
        public double Revenue { get; set; } = 0;
        public double Profit { get; set; } = 0;
    }
}
