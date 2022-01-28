using System.Collections.Generic;

namespace SupermarketReceipt;

public interface IOfferService
{
    IEnumerable<Discount> CalculateDiscounts(ISupermarketCatalog catalog, ShoppingCart shoppingCart);
}