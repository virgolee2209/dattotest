using System;
using System.Collections.Generic;
using System.Text;

namespace Gluh.CodingTest.Database
{
    public class SalesOrderLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
        /// <summary>
        /// Total Price of Order Line
        /// </summary>
        public decimal Price { get; set; }
    }
}