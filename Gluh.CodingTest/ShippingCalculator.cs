using System;
using System.Linq;
using System.Collections.Generic;
using Gluh.CodingTest.Database;

namespace Gluh.CodingTest
{
    public class ShippingCalculator
    {
        /// <summary>
        /// List of assumptions:
        /// * If sales order are empty => throw exception.
        /// * if product type is not physical, there is no weight for that product.
        /// * SalesOrderLine Price is the total price of all products in that line, not a single product price.
        /// * RateAdjustmentPrice for API is the discounted rate after getting rate from API.
        /// * RateAdjustmentPercentage for API is the discounted rate percentage after getting rate from API.
        /// * API Rate can have both adjustment price and adjustment percentage
        /// * Optimal rate is the maximum of all 3 kinds of rate after all calculations.
        /// </summary>
        private List<ShippingPriceRate> _priceRates;
        private List<ShippingWeightRate> _weightRates;
        private List<ShippingAPIRate> _apiRates;

        public ShippingCalculator()
        {
            _priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = 0, PriceMax = 50, Rate = 5.50m },
                //update min to 50
                new ShippingPriceRate { PriceMin = 50, PriceMax = 100, Rate = 0.00m },
                new ShippingPriceRate { PriceMin = 100, PriceMax = 500, Rate = 10.00m },
                new ShippingPriceRate { PriceMin = 1000, PriceMax = null, Rate = 15.00m },
            };
            _weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeightMin = 1, WeightMax = 5, Rate = 10.00m },
                new ShippingWeightRate { WeightMin = 5.01m, WeightMax = 10.00m, Rate = 7.00m },
                new ShippingWeightRate { WeightMin = 10.00m, WeightMax = null, Rate = 20.00m }
            };
            _apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeightMin = 10, WeightMax = 30, RateAdjustmentPrice = 5m },
                new ShippingAPIRate { WeightMin = 30, WeightMax = 35, RateAdjustmentPercent = 7.5m },
                new ShippingAPIRate { WeightMin = 35, WeightMax = null }
            };
        }
        public ShippingCalculator(List<ShippingPriceRate> priceRates,
            List<ShippingWeightRate> shippingWeightRates,
            List<ShippingAPIRate> shippingAPIRates)
        {
            _priceRates = priceRates;
            _weightRates = shippingWeightRates;
            _apiRates = shippingAPIRates;
        }

        /// <summary>
        /// Calculates the shipping price for a sales order
        /// ### Complete this method
        /// </summary>
        public decimal Calculate(SalesOrder salesOrder)
        {
            if (salesOrder.Lines == null || salesOrder.Lines.Count == 0) throw new Exception("Sale order is empty");
            List<decimal> rates = new List<decimal>();
            rates.Add(RateHelper.GetShippingPriceRate(_priceRates,salesOrder));
            rates.Add(RateHelper.GetShippingWeightRate(_weightRates, salesOrder));
            rates.Add(RateHelper.GetShippingApiRate(_apiRates, salesOrder));
            return rates.Max();
        }
    }
    
}
