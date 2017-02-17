using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webChaskibook.Models
{
    public class UtilModels
    {
        public static int idProd = 0;
        public static bool FiltrarProdOrder(ProductOrderModels oProd)
        {
            return idProd == oProd.Id;
        }
    }
}