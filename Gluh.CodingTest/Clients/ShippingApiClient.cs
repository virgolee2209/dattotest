using System;
using System.Collections.Generic;
using System.Text;

namespace Gluh.CodingTest
{
    /// <summary>
    /// Simulates calculating a shipping rate from a 3rd party API
    /// Hale: removed the first 2 params for easier calling as they are not used inside function anyway
    /// </summary>
    public class ShippingApiClient
    {
        //public decimal GetRate(decimal postalCodeFrom, decimal postalCodeTo, decimal weight)
        public decimal GetRate(decimal weight)
        {
            if (weight <= 5)
            {
                return weight * 2.5m;
            }
            else if (weight <= 10)
            {
                return weight * 1.5m;
            }
            else if (weight <= 20)
            {
                return weight * 1.25m;
            }
            else
            {
                return weight * 1.15m;
            }
        }
    }
}
