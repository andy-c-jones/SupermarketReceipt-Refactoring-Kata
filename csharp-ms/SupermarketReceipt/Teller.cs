using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly ISupermarketCatalog _catalog;
        private readonly IOfferService _offerService;

        public Teller(ISupermarketCatalog catalog, IOfferService offerService)
        {
            _catalog = catalog;
            _offerService = offerService;
        }

        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            foreach (var pq in productQuantities)
            {
                var p = pq.Product;
                var quantity = pq.Quantity;
                var unitPrice = _catalog.GetUnitPrice(p);
                var price = quantity * unitPrice;
                receipt.AddProduct(p, quantity, unitPrice, price);
            }

            var discounts = _offerService.CalculateDiscounts(_catalog, theCart);
            foreach (var discount in discounts)
            {
                receipt.AddDiscount(discount);
            }

            return receipt;
        }
    }
}