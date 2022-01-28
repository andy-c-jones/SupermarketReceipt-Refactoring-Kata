using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SupermarketReceipt;

namespace Supermarket.Test
{
    public class SupermarketTest
    {
        [Test]
        public void TenPercentDiscount()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);
            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog, new OfferService());
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.AreEqual(4.975, receipt.GetTotalPrice());
            Assert.AreEqual(new List<Discount>(), receipt.GetDiscounts());
            Assert.AreEqual(1, receipt.GetItems().Count);
            var receiptItem = receipt.GetItems()[0];
            Assert.AreEqual(apples, receiptItem.Product);
            Assert.AreEqual(1.99, receiptItem.Price);
            Assert.AreEqual(2.5 * 1.99, receiptItem.TotalPrice);
            Assert.AreEqual(2.5, receiptItem.Quantity);
        }


        [Test]
        public void SingleDiscountOfTenPercent_ShouldDiscountItemByTenPercent()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(toothbrush, 1);

            var expectedDiscount = new List<Discount> {new(toothbrush, "10% off", -0.099)};

            var offerService = new Mock<IOfferService>();
            offerService
                .Setup(os => os.CalculateDiscounts(It.Is<Dictionary<Product, Offer>>(
                    offers => offers[toothbrush].Argument == 10.0
                              && offers[toothbrush].OfferType == SpecialOfferType.TenPercentDiscount), catalog, cart))
                .Returns(expectedDiscount);

            var teller = new Teller(catalog, offerService.Object);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            CollectionAssert.AreEquivalent(expectedDiscount, receipt.GetDiscounts());
        }
    }
}