using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gluh.CodingTest;
using Gluh.CodingTest.Database;
using System.Collections.Generic;

namespace ShipCalculator.Tests
{
    [TestClass]
    public class ShippingCalculatorTests
    {
        const decimal PRICE_RATE_MIN = 5.5m;
        const decimal PRICE_RATE_MAX = 15m;
        const decimal WEIGHT_RATE_MIN = 10m;
        ShippingCalculator shippingCalculator;
        [TestInitialize]
        public void SetUp() 
        {
            List<ShippingPriceRate> priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = 0, PriceMax = 50, Rate = PRICE_RATE_MIN },
                new ShippingPriceRate { PriceMin = 50, PriceMax = 100, Rate = 8.50m },
                new ShippingPriceRate { PriceMin = 100, PriceMax = 500, Rate = 10.00m },
                new ShippingPriceRate { PriceMin = 1000, PriceMax = null, Rate = PRICE_RATE_MAX },
            };
            List<ShippingWeightRate> weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeightMin = 1, WeightMax = 5, Rate = WEIGHT_RATE_MIN },
                new ShippingWeightRate { WeightMin = 5.01m, WeightMax = 10.00m, Rate = 7.00m },
                new ShippingWeightRate { WeightMin = 10.00m, WeightMax = null, Rate = 20.00m }
            };
            List<ShippingAPIRate> apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = 10, WeightMax = 30, RateAdjustmentPrice = 5m },
                new ShippingAPIRate { WeightMin = 30, WeightMax = 35, RateAdjustmentPercent = 7.5m },
                new ShippingAPIRate { WeightMin = 35, WeightMax = null }
            };
            shippingCalculator = new ShippingCalculator(priceRates,weightRates,apiRates);
            
        }
        [TestMethod]
        public void Calculate_EmptySaleOrder_ExceptionThrown()
        {
            SalesOrder salesOrder = new SalesOrder();
            Exception exception=Assert.ThrowsException<Exception>(()=>shippingCalculator.Calculate(salesOrder), "Sale order is empty");
            //Assert.AreEqual("Sale order is empty", exception.Message);
        }
        [TestMethod]
        public void Calculate_NoWeightPriceMin_PriceRateMin()
        {
            SalesOrder salesOrder = new SalesOrder();
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() {Type=ProductType.NonPhysical,Weight=1.0m};//make it non physical=> no weight
            line.Quantity = 1;
            line.Price = 50;
            salesOrder.Lines = new List<SalesOrderLine>();
            salesOrder.Lines.Add(line);

            decimal shippingRate = shippingCalculator.Calculate(salesOrder);
            Assert.AreEqual(PRICE_RATE_MIN, shippingRate);
        }
        [TestMethod]
        public void Calculate_NoWeightPriceMax_PriceRateMax()
        {
            SalesOrder salesOrder = new SalesOrder();
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.NonPhysical, Weight = 1.0m };//make it non physical=> no weight
            line.Quantity = 1;
            line.Price = 9999;
            salesOrder.Lines = new List<SalesOrderLine>();
            salesOrder.Lines.Add(line);

            decimal shippingRate = shippingCalculator.Calculate(salesOrder);
            Assert.AreEqual(PRICE_RATE_MAX, shippingRate);
        }
        [TestMethod]
        public void Calculate_WeightMin_WeightRateMin()
        {
            SalesOrder salesOrder = new SalesOrder();
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = 1.0m };//make it physical=> has weight
            line.Quantity = 1;
            line.Price = 0;
            salesOrder.Lines = new List<SalesOrderLine>();
            salesOrder.Lines.Add(line);

            decimal shippingRate = shippingCalculator.Calculate(salesOrder);
            Assert.AreEqual(WEIGHT_RATE_MIN, shippingRate);
        }

        [TestMethod]
        public void Calculate_Weight100_ApiRateFor100()
        {
            decimal testWeight = 100;
            SalesOrder salesOrder = new SalesOrder();
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = testWeight };//make it physical=> has weight
            line.Quantity = 1;
            line.Price = 50;
            salesOrder.Lines = new List<SalesOrderLine>();
            salesOrder.Lines.Add(line);

            decimal apiRate = new ShippingApiClient().GetRate(testWeight);//should have no adjustment


            decimal shippingRate = shippingCalculator.Calculate(salesOrder);
            Assert.AreEqual(apiRate, shippingRate);
        }

        [TestMethod]
        public void Calculate_DefaultConstructorWeight100_ApiRateFor100()
        {
            shippingCalculator = new ShippingCalculator();
            decimal testWeight = 100;
            SalesOrder salesOrder = new SalesOrder();
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = testWeight };//make it physical=> has weight
            line.Quantity = 1;
            line.Price = 50;
            salesOrder.Lines = new List<SalesOrderLine>();
            salesOrder.Lines.Add(line);

            decimal apiRate = new ShippingApiClient().GetRate(testWeight);//should have no adjustment


            decimal shippingRate = shippingCalculator.Calculate(salesOrder);
            Assert.AreEqual(apiRate, shippingRate);
        }

    }
}
