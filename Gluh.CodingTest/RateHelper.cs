using Gluh.CodingTest.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gluh.CodingTest
{
    public static class RateHelper
    {
        public static decimal GetShippingPriceRate(List<ShippingPriceRate> shippingPriceRates, SalesOrder salesOrder)
        {
            decimal totalPrice = salesOrder.Lines.Sum(a => a.Price);
            ShippingPriceRate rate = null;
            foreach (ShippingPriceRate shippingPriceRate in shippingPriceRates)
            {
                if (totalPrice >= shippingPriceRate.PriceMin &&
                    (!shippingPriceRate.PriceMax.HasValue || (shippingPriceRate.PriceMax.HasValue && totalPrice <= shippingPriceRate.PriceMax)))
                {
                    rate = shippingPriceRate;
                    break;
                }
            }
            return rate != null ? rate.Rate : 0.0m;
        }
        public static decimal GetShippingWeightRate(List<ShippingWeightRate> shippingWeightRates, SalesOrder salesOrder)
        {
            decimal totalWeight = GetTotalWeightOfOrder(salesOrder);
            ShippingWeightRate rate = null;
            foreach (ShippingWeightRate shippingWeightRate in shippingWeightRates)
            {
                if (totalWeight >= shippingWeightRate.WeightMin &&
                    (!shippingWeightRate.WeightMax.HasValue ||
                    (shippingWeightRate.WeightMax.HasValue && totalWeight <= shippingWeightRate.WeightMax)))
                {
                    rate = shippingWeightRate;
                    break;
                }
            }
            return rate != null ? rate.Rate : 0.0m;

        }
        public static decimal GetShippingApiRate(List<ShippingAPIRate> shippingAPIRates, SalesOrder salesOrder)
        {
            decimal totalWeight = GetTotalWeightOfOrder(salesOrder);
            
            ShippingAPIRate rate = null;
            foreach (ShippingAPIRate shippingApiRate in shippingAPIRates)
            {
                if (totalWeight >= shippingApiRate.WeightMin &&
                    (!shippingApiRate.WeightMax.HasValue ||
                    (shippingApiRate.WeightMax.HasValue && totalWeight <= shippingApiRate.WeightMax)))
                {
                    rate = shippingApiRate;
                    break;
                }
            }
            decimal apiRate = 0;
            if (rate != null)
            {
                apiRate = new ShippingApiClient().GetRate(totalWeight);
                if (rate.RateAdjustmentPercent.HasValue)
                {
                    apiRate = apiRate * (1 - (rate.RateAdjustmentPercent.Value / 100));
                }
                if (rate.RateAdjustmentPrice.HasValue)
                {
                    apiRate -= rate.RateAdjustmentPrice.Value;
                }
                if (apiRate < 0) apiRate = 0;

            }
            return apiRate;
        }
        public static decimal GetTotalWeightOfOrder(SalesOrder salesOrder)
        {
            decimal totalWeight = salesOrder.Lines
                    .Where(a => a.Product.Type == ProductType.Physical)
                    .Sum(a => a.Quantity * a.Product.Weight);
            return totalWeight;
        }
    }
}
