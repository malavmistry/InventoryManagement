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
        public int Qty { get; set; }

    }
}
