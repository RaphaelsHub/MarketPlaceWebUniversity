﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.ModelAccessLayer.Model
{
    public class CartData
    {
        public decimal SumPrice { get; set; }
        public decimal DeliveryPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public List<Tuple<Product, int>> productList;

        public CartData()
        {
            productList = new List<Tuple<Product, int>>();
        }
    }
}