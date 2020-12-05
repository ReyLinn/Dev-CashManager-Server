﻿using CashManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Services
{
    interface IProductService
    {
        public Product GetProductByReference(string reference);
    }
}
