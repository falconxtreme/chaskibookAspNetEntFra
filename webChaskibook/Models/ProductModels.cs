using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webChaskibook.Models
{
    public class ProductModels
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public string UrlImagen { get; set; }
        public int Stock { get; set; }
        public bool Enabled { get; set; }
    }
}