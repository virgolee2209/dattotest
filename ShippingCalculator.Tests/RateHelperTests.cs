using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gluh.CodingTest;
using Gluh.CodingTest.Database;
using System.Collections.Generic;

namespace ShipCalculator.Tests
{
    [TestClass]
    public class RateHelperTests
    {
        SalesOrder salesOrder ;
        [TestInitialize]
        public void SetUp() 
        {
            salesOrder = new SalesOrder();
            salesOrder.Lines = new List<SalesOrderLine>();
        }
        [TestMethod]
        public void GetShippingPriceRate_UnderPriceMin_RateFree()
        {
            decimal testingRate = 80;
            decimal priceMin = 10;
            decimal priceMax = 50;
            List<ShippingPriceRate> priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = priceMin, PriceMax = priceMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.NonPhysical};
            line.Quantity = 1;
            line.Price = 5;
            salesOrder.Lines.Add(line);

            Assert.AreEqual(0, RateHelper.GetShippingPriceRate(priceRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingPriceRate_PriceMin_CorrectRate()
        {
            decimal testingRate = 80;
            decimal priceMin = 10;
            decimal priceMax = 50;
            List<ShippingPriceRate> priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = priceMin, PriceMax = priceMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.NonPhysical };
            line.Quantity = 1;
            line.Price = priceMin;
            salesOrder.Lines.Add(line);

            Assert.AreEqual(testingRate, RateHelper.GetShippingPriceRate(priceRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingPriceRate_PriceMax_CorrectRate()
        {
            decimal testingRate = 80;
            decimal priceMin = 10;
            decimal priceMax = 50;
            List<ShippingPriceRate> priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = priceMin, PriceMax = priceMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.NonPhysical };
            line.Quantity = 1;
            line.Price = priceMax;
            salesOrder.Lines.Add(line);

            Assert.AreEqual(testingRate, RateHelper.GetShippingPriceRate(priceRates, salesOrder));
        }

    }
}
