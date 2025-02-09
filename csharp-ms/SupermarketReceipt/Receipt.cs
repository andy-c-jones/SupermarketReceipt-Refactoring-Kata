using System.Collections.Generic;

namespace SupermarketReceipt
{
    public record ReceiptItem(Product Product, double Quantity, double Price, double TotalPrice);

    public class Receipt
    {
        private readonly List<Discount> _discounts = new();
        private readonly List<ReceiptItem> _items = new();

        public double GetTotalPrice()
        {
            var total = 0.0;
            foreach (var item in _items) total += item.TotalPrice;
            foreach (var discount in _discounts) total += discount.DiscountAmount;
            return total;
        }

        public void AddProduct(Product p, double quantity, double price, double totalPrice)
        {
            _items.Add(new ReceiptItem(p, quantity, price, totalPrice));
        }

        public List<ReceiptItem> GetItems()
        {
            return new List<ReceiptItem>(_items);
        }

        public void AddDiscount(Discount discount)
        {
            _discounts.Add(discount);
        }

        public List<Discount> GetDiscounts()
        {
            return _discounts;
        }
    }
}