using System;
using System.Linq;
using System.Collections.Generic;
using Gluh.CodingTest.Database;

namespace Gluh.CodingTest
{
    public class ShippingCalculator
    {

        private List<ShippingPriceRate> _priceRates;
        private List<ShippingWeightRate> _weightRates;
        private List<ShippingAPIRate> _apiRates;

        public ShippingCalculator()
        {
            _priceRates = new List<ShippingPriceRate>
            {
                new ShippingPriceRate { PriceMin = 0, PriceMax = 50, Rate = 5.50m },
                new ShippingPriceRate { PriceMin = 0, PriceMax = 100, Rate = 0.00m },
                new ShippingPriceRate { PriceMin = 100, PriceMax = 500, Rate = 10.00m },
                new ShippingPriceRate { PriceMin = 1000, PriceMax = null, Rate = 15.00m },
            };
            _weightRates = new List<ShippingWeightRate>
            {
                new ShippingWeightRate { WeighMin = 1, WeighMax = 5, Rate = 10.00m },
                new ShippingWeightRate { WeighMin = 5.01m, WeighMax = 10.00m, Rate = 7.00m },
                new ShippingWeightRate { WeighMin = 10.00m, WeighMax = null, Rate = 20.00m }
            };
            _apiRates = new List<ShippingAPIRate>
            {
                new ShippingAPIRate { WeighMin = 10, WeighMax = 30, RateAdjustmentPrice = 5m },
                new ShippingAPIRate { WeighMin = 30, WeighMax = 35, RateAdjustmentPercent = 7.5m },
                new ShippingAPIRate { WeighMin = 35, WeighMax = null }
            };
        }

        /// <summary>
        /// Calculates the shipping price for a sales order
        /// ### Complete this method
        /// </summary>
        public decimal Calculate(SalesOrder salesOrder)
        {
            throw new NotImplementedException();
        }
    }
}
