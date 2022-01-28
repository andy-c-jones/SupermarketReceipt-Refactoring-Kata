using System.Collections.Generic;

namespace SupermarketReceipt;

public interface IOfferService
{
    IEnumerable<Discount> CalculateDiscounts(Dictionary<Product, Offer> offers, ISupermarketCatalog catalog, ShoppingCart shoppingCart);
}