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

        [TestMethod]
        public void GetShippingWeightRate_UnderWeightMin_RateFree()
        {
            decimal testingRate = 80;
            decimal weightMin = 10;
            decimal weightMax = 20;
            List<ShippingWeightRate> weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeightMin = weightMin, WeightMax = weightMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical,Weight=5 };
            salesOrder.Lines.Add(line);

            Assert.AreEqual(0, RateHelper.GetShippingWeightRate(weightRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingWeightRate_WeightMin_CorrectRate()
        {
            decimal testingRate = 80;
            decimal weightMin = 10;
            decimal weightMax = 20;
            List<ShippingWeightRate> weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeightMin = weightMin, WeightMax = weightMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = weightMin };
            line.Quantity = 1;
            salesOrder.Lines.Add(line);

            Assert.AreEqual(testingRate, RateHelper.GetShippingWeightRate(weightRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingWeightRate_WeightMax_CorrectRate()
        {
            decimal testingRate = 80;
            decimal weightMin = 10;
            decimal weightMax = 20;
            List<ShippingWeightRate> weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeightMin = weightMin, WeightMax = weightMax, Rate = testingRate }
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = weightMax };
            line.Quantity = 1;
            salesOrder.Lines.Add(line);

            Assert.AreEqual(testingRate, RateHelper.GetShippingWeightRate(weightRates, salesOrder));
        }

        [TestMethod]
        public void GetShippingApiRate_UnderWeightMin_RateFree()
        {
            decimal weightMin = 10;
            decimal weightMax = 20;
            List<ShippingAPIRate> apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = weightMin, WeightMax = weightMax}
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = 5 };
            salesOrder.Lines.Add(line);

            Assert.AreEqual(0, RateHelper.GetShippingApiRate(apiRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingApiRate_WeightMin_CorrectRate()
        {
            decimal weightMin = 10;
            decimal weightMax = 20;
            decimal adjustmentRate = 5;
            decimal adjustmentPercentage = 5;
            List<ShippingAPIRate> apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = weightMin, WeightMax = weightMax,RateAdjustmentPrice=adjustmentRate,RateAdjustmentPercent=adjustmentPercentage}
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = weightMin };
            line.Quantity = 1;
            salesOrder.Lines.Add(line);

            decimal testingRate = new ShippingApiClient().GetRate(weightMin);
            testingRate = testingRate - testingRate * adjustmentPercentage / 100;
            testingRate = testingRate - adjustmentRate;
            

            Assert.AreEqual(testingRate, RateHelper.GetShippingApiRate(apiRates, salesOrder));
        }
        [TestMethod]
        public void GetShippingApiRate_WeightMax_CorrectRate()
        {
            decimal weightMin = 10;
            decimal weightMax = 20;
            decimal adjustmentRate = 5;
            decimal adjustmentPercentage = 5;
            List<ShippingAPIRate> apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = weightMin, WeightMax = weightMax,RateAdjustmentPrice=adjustmentRate,RateAdjustmentPercent=adjustmentPercentage}
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = weightMax };
            line.Quantity = 1;
            salesOrder.Lines.Add(line);

            decimal testingRate = new ShippingApiClient().GetRate(weightMax);
            testingRate = testingRate - testingRate * adjustmentPercentage / 100;
            testingRate = testingRate - adjustmentRate;
            

            Assert.AreEqual(testingRate, RateHelper.GetShippingApiRate(apiRates, salesOrder));
        }

        [TestMethod]
        public void GetShippingApiRate_LowWeight_AdjustToBeFree()
        {
            decimal weightMin = 0;
            decimal weightMax = 10;
            decimal adjustmentRate = 9999;//make it big to make sure the return rate will be adjusted down to negative
            List<ShippingAPIRate> apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = weightMin, WeightMax = weightMax,RateAdjustmentPrice=adjustmentRate}
            };
            SalesOrderLine line = new SalesOrderLine();
            line.Product = new Product() { Type = ProductType.Physical, Weight = 1 };
            line.Quantity = 1;
            salesOrder.Lines.Add(line);

            //decimal testingRate = new ShippingApiClient().GetRate(weightMax);
            //testingRate = testingRate - testingRate * adjustmentPercentage / 100;
            //testingRate = testingRate - adjustmentRate;
            


            Assert.AreEqual(0, RateHelper.GetShippingApiRate(apiRates, salesOrder));
        }

    }
}
