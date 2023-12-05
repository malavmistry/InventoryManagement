using SQLite;

namespace Inventory.Model
{
    public class BalanceSheet
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TransType TransType { get; set; }
        public string Itemid { get; set; }
        public string SellerId { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public double TotalPrice { get; set; }
        public int RemainingQty { get; set; }
    }
    public enum TransType
    {
        None,
        Sale,
        Purchase
    }
}
