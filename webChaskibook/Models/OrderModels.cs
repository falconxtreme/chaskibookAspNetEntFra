using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webChaskibook.Models
{
    public class OrderModels
    {
        public int Id { get; set; }
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public string FechaDiaEntrega { get; set; }
        public string HoraEntrega { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioSinIGV { get; set; }
        public decimal IGV { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioFinal { get; set; }
        public decimal TotalPrecioCosto
        {
            get
            {
                decimal precio = 0;
                if(lobeProducto!=null)
                {
                    foreach (ProductOrderModels oProd in lobeProducto)
                    {
                        precio += (oProd.cantidadPedida * oProd.PrecioCosto);
                    }
                }
               
                return precio;
            }
        }
        public decimal TotalPrecioVenta { get
            {
                decimal precio = 0;
                if (lobeProducto != null)
                {
                    foreach (ProductOrderModels oProd in lobeProducto)
                    {
                        precio += (oProd.cantidadPedida * oProd.PrecioVenta);
                    }
                }
                
                return precio;
            }
        }
        public decimal TotalPrecioVentaConDsctoPublico { get
            {
                return (TotalPrecioCosto + ((TotalPrecioVenta - TotalPrecioCosto) * 6/10));
            }
        }

        public decimal TotalPrecioVentaConDsctoFamiliar
        {
            get
            {
                return (TotalPrecioCosto + ((TotalPrecioVenta - TotalPrecioCosto) * 4 / 10));
            }
        }
        
        public List<ProductOrderModels> lobeProducto { get; set; }
    }
}