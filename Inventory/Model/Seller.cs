using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Seller
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
