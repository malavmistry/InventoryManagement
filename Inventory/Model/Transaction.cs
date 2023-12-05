using Microsoft.VisualBasic;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SellerId { get; set; }
        public string ItemId { get; set; }
        public TransType Type { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
    }
}
