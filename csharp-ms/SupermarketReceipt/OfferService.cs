using System;
using System.Collections.Generic;

namespace SupermarketReceipt;

public class OfferService : IOfferService
{
    private readonly Dictionary<Product, Offer> _offers;

    public OfferService(Dictionary<Product, Offer> offers)
    {
        _offers = offers;
    }

    public IEnumerable<Discount> CalculateDiscounts(ISupermarketCatalog catalog, ShoppingCart shoppingCart)
    {
        foreach (var p in shoppingCart.ProductQuantities.Keys)
        {
            var quantity = shoppingCart.ProductQuantities[p];
            var quantityAsInt = (int) quantity;
            if (_offers.ContainsKey(p))
            {
                var offer = _offers[p];
                var unitPrice = catalog.GetUnitPrice(p);
                Discount discount = null;
                var x = 1;
                if (offer.OfferType == SpecialOfferType.ThreeForTwo)
                {
                    x = 3;
                }
                else if (offer.OfferType == SpecialOfferType.TwoForAmount)
                {
                    x = 2;
                    if (quantityAsInt >= 2)
                    {
                        var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
                        var discountN = unitPrice * quantity - total;
                        discount = new Discount(p, "2 for €" + offer.Argument, -discountN);
                    }
                }

                if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
                var numberOfXs = quantityAsInt / x;

                if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
                {
                    discount = new(p, "3 for the price of 2", Convert.ToInt32(Math.Floor(quantity / 3)) * -unitPrice);
                }

                if (offer.OfferType == SpecialOfferType.PercentDiscount) discount = new Discount(p, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
                if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
                {
                    var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                    discount = new Discount(p, x + " for " + offer.Argument, -discountTotal);
                }

                if (discount != null)
                    yield return discount;
            }
        }
    }
}