using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new();

        public Dictionary<Product, double> ProductQuantities { get; } = new();

        public List<ProductQuantity> GetItems() => _items;

        public void AddItem(Product product)
        {
            AddItemQuantity(product, 1.0);
        }

        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
            if (ProductQuantities.ContainsKey(product))
            {
                var newAmount = ProductQuantities[product] + quantity;
                ProductQuantities[product] = newAmount;
            }
            else
            {
                ProductQuantities.Add(product, quantity);
            }
        }
    }
}