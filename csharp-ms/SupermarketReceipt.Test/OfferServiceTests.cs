using System.Collections.Generic;
using NUnit.Framework;

namespace SupermarketReceipt.Test;

[TestFixture]
public class OfferServiceTests
{
    [Test]
    public void Given_an_empty_catalog_cart_and_offers_list_When_calculating_discounts_Then_return_no_discounts()
    {
        var offers = new Dictionary<Product, Offer>();
        var emptyCatalog = new FakeCatalog();
        var emptyCart = new ShoppingCart();
        var discounts = new OfferService(offers).CalculateDiscounts(emptyCatalog, emptyCart);
        Assert.That(discounts, Is.Empty);
    }

    [Test]
    public void
        Given_a_cart_with_items_but_the_catalog_and_offers_list_are_empty_When_calculating_discounts_Then_return_no_discounts()
    {
        var offers = new Dictionary<Product, Offer>();
        var emptyCatalog = new FakeCatalog();
        var cart = new ShoppingCart();
        cart.AddItemQuantity(new Product("Sausage", ProductUnit.Kilo), 1);

        var discounts = new OfferService(offers).CalculateDiscounts(emptyCatalog, cart);
        Assert.That(discounts, Is.Empty);
    }

    [TestCase(10.0, "10% off", -1)]
    [TestCase(2.5, "2.5% off", -0.25)]
    [TestCase(50.0, "50% off", -5)]
    public void
        Given_a_cart_with_item_with_has_a_percentage_discount_offer_and_a_price_in_the_catalog_When_calculating_discounts_Then_return_the_discounts(
            double percentageDiscount, string description, double discountAmount)
    {
        var sausage = new Product("Sausage", ProductUnit.Kilo);

        var offers = new Dictionary<Product, Offer>
            {{sausage, new Offer(SpecialOfferType.PercentDiscount, sausage, percentageDiscount)}};
        var catalog = new FakeCatalog();
        catalog.AddProduct(sausage, 10);
        var cart = new ShoppingCart();
        cart.AddItemQuantity(sausage, 1);

        var discounts = new OfferService(offers).CalculateDiscounts(catalog, cart);
        CollectionAssert.AreEquivalent(new List<Discount> {new(sausage, description, discountAmount) }, discounts);
    }

    [TestCase(10)]
    [TestCase(312.1)]
    [TestCase(44.54)]
    public void Given_a_cart_with_three_items_that_qualify_for_3_for_the_price_of_2_When_calculating_discounts_Then_return_the_price_of_one_as_a_discount(double price)
    {
        var sausage = new Product("Sausage", ProductUnit.Kilo);

        var offers = new Dictionary<Product, Offer>
            {{sausage, new Offer(SpecialOfferType.ThreeForTwo, sausage, 9929313)}};
        var catalog = new FakeCatalog();
        catalog.AddProduct(sausage, price);
        var cart = new ShoppingCart();
        cart.AddItemQuantity(sausage, 3);

        var discounts = new OfferService(offers).CalculateDiscounts(catalog, cart);
        CollectionAssert.AreEquivalent(new List<Discount> { new(sausage, "3 for 2", -price) }, discounts);
    }

    [TestCase(10, -20)]
    [TestCase(312.1, -624.2)]
    [TestCase(44.54, -89.08)]
    public void Given_a_cart_with_seven_items_that_qualify_for_3_for_the_price_of_2_When_calculating_discounts_Then_return_the_price_of_one_as_a_discount(double price, double expectedDiscount)
    {
        var sausage = new Product("Sausage", ProductUnit.Kilo);

        var offers = new Dictionary<Product, Offer>
            {{sausage, new Offer(SpecialOfferType.ThreeForTwo, sausage, 9929313)}};
        var catalog = new FakeCatalog();
        catalog.AddProduct(sausage, price);
        var cart = new ShoppingCart();
        cart.AddItemQuantity(sausage, 8.7);

        var discounts = new OfferService(offers).CalculateDiscounts(catalog, cart);
        CollectionAssert.AreEquivalent(new List<Discount> { new(sausage, "3 for 2", expectedDiscount) }, discounts);
    }


    //needs a test for:
    // TwoForAmount,
    // FiveForAmount
}