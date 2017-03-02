using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webChaskibook.Models;

namespace webChaskibook.Controllers
{
    public class ProductModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string strIdSesion = "";

        // GET: ProductModels
        public ActionResult Index(string strBus="" , string idSesion="")
        {
            List<ProductModels> lobeProds=null;
            if (strBus == "")
            {
                if (idSesion==null || idSesion == "")
                {
                    idSesion = Guid.NewGuid().ToString();
                }
                lobeProds = db.ProductModels.ToList();
                return View(lobeProds);
            }
            else
            {
                if (idSesion == null || idSesion == "")
                {
                    idSesion = Guid.NewGuid().ToString();
                }
                lobeProds = db.ProductModels
                            .Where(p => p.Nombre.ToUpper().Contains(strBus)).ToList();
                //foreach (ProductModels oProd in lobeProds)
                //{
                //    oProd.IdSesion = idSesion;
                //}
                return View(lobeProds);
            }
            
        }

        // GET: ProductModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModels productModels = db.ProductModels.Find(id);
            if (productModels == null)
            {
                return HttpNotFound();
            }
            return View(productModels);
        }

        // GET: ProductModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductModels/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,PrecioCosto,PrecioVenta,UrlImagen,Stock,Enabled")] ProductModels productModels)
        {
            if (ModelState.IsValid)
            {
                db.ProductModels.Add(productModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productModels);
        }

        // GET: ProductModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModels productModels = db.ProductModels.Find(id);
            if (productModels == null)
            {
                return HttpNotFound();
            }
            return View(productModels);
        }

        // POST: ProductModels/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,PrecioCosto,PrecioVenta,UrlImagen,Stock,Enabled")] ProductModels productModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productModels);
        }

        // GET: ProductModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModels productModels = db.ProductModels.Find(id);
            if (productModels == null)
            {
                return HttpNotFound();
            }
            return View(productModels);
        }

        // POST: ProductModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductModels productModels = db.ProductModels.Find(id);
            db.ProductModels.Remove(productModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: ProductModels
        public ActionResult Buscar(string busqueda)
        {
            busqueda = busqueda.ToUpper();
            Session["busqueda"] = busqueda;
            //return View(prods.ToList());
            return RedirectToAction("Index", "ProductModels", new { strBus= busqueda });
            //return RedirectToAction("Index",  prods.ToList());
        }

        // GET: ProductModels/Delete/5
        public ActionResult Agregar(int? hdfId, string hdfIdSesion = "", string desAdicional="" , int cantidad=1)
        {
            if (hdfId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModels productModels = db.ProductModels.Find(hdfId);
            if (hdfIdSesion == null)
            {
                return HttpNotFound();
            }
            //List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session[hdfIdSesion + "lobeProdsPedido"];
            List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
            ProductOrderModels oProd;
            if (lobeProdsPedido!=null && lobeProdsPedido.Count > 0)
            {
                UtilModels.idProd = productModels.Id;
                oProd = lobeProdsPedido.Find(UtilModels.FiltrarProdOrder);
                if (oProd != null)
                {
                    oProd.cantidadPedida += cantidad;
                    oProd.DescripcionAdicional += "-" + desAdicional;
                }
                else
                {
                    oProd = new ProductOrderModels()
                    {
                        Id = productModels.Id,
                        IdProducto = productModels.Id,
                        Nombre =productModels.Nombre,
                        DescripcionAdicional= desAdicional,
                        PrecioCosto =productModels.PrecioCosto,
                        PrecioVenta = productModels.PrecioVenta,
                        UrlImagen = productModels.UrlImagen,
                        cantidadPedida =cantidad
                    };
                    lobeProdsPedido.Add(oProd);
                }
            }else
            {
                lobeProdsPedido = new List<ProductOrderModels>();
                oProd = new ProductOrderModels()
                {
                    Id = productModels.Id,
                    IdProducto = productModels.Id,
                    Nombre = productModels.Nombre,
                    DescripcionAdicional = desAdicional,
                    PrecioCosto = productModels.PrecioCosto,
                    PrecioVenta = productModels.PrecioVenta,
                    UrlImagen = productModels.UrlImagen,
                    cantidadPedida = cantidad
                };
                lobeProdsPedido.Add(oProd);
            }
            //Session[hdfIdSesion + "lobeProdsPedido"] = lobeProdsPedido;
            Session[ "lobeProdsPedido"] = lobeProdsPedido;
            string strBus = (string) Session["busqueda"];
            return RedirectToAction("Index", "ProductModels", new { strBus = strBus, idSesion= hdfIdSesion });
        }

        public ActionResult LimpiarPedido(string idSesion = "")
        {
            if (idSesion == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["lobeProdsPedido"] = null;
            Session["orderModels"] = null;
            Session["lobeProdNoExistente"] = null;
            return RedirectToAction("Index", "ProductModels", new { strBus = "", idSesion = idSesion });
        }

        public ActionResult VerPedido(string idSesion = "")
        {
            if (idSesion == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session[idSesion + "lobeProdsPedido"];
            List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
            if (lobeProdsPedido == null)
            {
                return HttpNotFound();
            }
            OrderModels order = new OrderModels();
            order.lobeProducto = lobeProdsPedido;
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult QuitarProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
            if (lobeProdsPedido == null)
            {
                return HttpNotFound();
            }
            int indiceProd = -1;
            UtilModels.idProd = id.Value;
            indiceProd = lobeProdsPedido.FindIndex(UtilModels.FiltrarProdOrder);
            if (indiceProd>=0) lobeProdsPedido.RemoveAt(indiceProd);
            return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
        }

        public ActionResult GenerarPedido(OrderModels orderModels)
        {
            List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
            orderModels.lobeProducto = lobeProdsPedido;
            orderModels.PrecioCosto = orderModels.TotalPrecioCosto;
            orderModels.PrecioVenta = orderModels.TotalPrecioVenta;
            orderModels.PrecioFinal = orderModels.TotalPrecioVentaConDsctoFamiliar;
            decimal dIGV = 0;
            if (ConfigurationManager.AppSettings["IGV"].ToString() != null)
            {
                decimal.TryParse(ConfigurationManager.AppSettings["IGV"].ToString(),out dIGV);
            }
            orderModels.PrecioSinIGV = Math.Round(orderModels.PrecioFinal / (1+dIGV),2,MidpointRounding.AwayFromZero);
            orderModels.IGV = Math.Round( orderModels.PrecioFinal - orderModels.PrecioSinIGV, 2, MidpointRounding.AwayFromZero);
            if (ModelState.IsValid)
            {
                db.OrderModels.Add(orderModels);
                db.SaveChanges();
                lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
                if (lobeProdsPedido != null)
                {
                    ProductModels oProdUpd;
                    foreach(ProductOrderModels oProd in lobeProdsPedido)
                    {
                        oProdUpd = db.ProductModels.Find(oProd.IdProducto);
                        if (oProdUpd != null)
                        {
                            oProdUpd.Stock -= oProd.cantidadPedida;
                            db.Entry(oProdUpd).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    OrderModels oNew = db.OrderModels.ToList().Last();
                    if (oNew != null)
                    {
                        orderModels.Id = oNew.Id;
                    }
                }
                Session["orderModels"] = orderModels;
                return RedirectToAction("GenerarPDF", "ProductModels", new { idSesion = "" });
            }
            return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
        }

        public ActionResult GenerarPDF(string idSesion="")
        {
            try
            {
                LocalReport lr = new LocalReport();
                List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
                OrderModels order;
                order = (OrderModels)Session["orderModels"];
                if (order == null)
                {
                    order = new OrderModels();
                    order.lobeProducto = lobeProdsPedido;
                }
                
                List<OrderModels> lobeOrder = new List<OrderModels>();
                lobeOrder.Add(order);

                string rutaRep = ConfigurationManager.AppSettings["RutaReporteSolicitud"].ToString();
                lr.EnableExternalImages = true;
                lr.ReportPath = rutaRep;

                lr.DataSources.Add(new ReportDataSource("DataSet1",lobeOrder));
                lr.DataSources.Add(new ReportDataSource("DataSet2", lobeProdsPedido));

                string reportType;
                string format = "PDF";
                if (format == null || format == "" || format == "jpeg")
                {
                    reportType = "Image";
                    format = "jpeg";
                }
                else
                {
                    reportType = format;
                }
                string mimeType;
                string encoding;
                string fileNameExtension;
                //string deviceInfo;
                string deviceInfo = "<DeviceInfo>" +
                                "   <OutputFormat>" + format + "</OutputFormat>" +
                                "  <PageWidth>9in</PageWidth>" +
                                "  <PageHeight>12in</PageHeight>" +
                                //"  <MarginTop>0.5in</MarginTop>" +
                                "  <MarginLeft>0.75in</MarginLeft>" +
                                //"  <MarginRight>1in</MarginRight>" +
                                //"  <MarginBottom>0.5in</MarginBottom>" +
                                "</DeviceInfo>";
                Warning[] warnings = null;
                string[] streams = null;
                byte[] renderedBytes = null;

                renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarReciboFicticioOPedido(string IdCliente = "", string NombreCliente = "", string Direccion = "",
            string FechaDiaEntrega = "", string HoraEntrega = "", string DescripcionVenta="", decimal PrecioTotalVenta=0, 
            bool EsPedido = false, bool EsCotizacion= false)
        {
            try
            {
                if (EsCotizacion)
                {
                    return null;
                }
                else if(EsPedido)
                {
                    OrderModels oOrder = new OrderModels()
                    {
                        IdCliente = IdCliente,
                        NombreCliente = NombreCliente,
                        Direccion = Direccion,
                        FechaDiaEntrega = FechaDiaEntrega,
                        HoraEntrega = HoraEntrega
                    };
                    return GenerarPedido(oOrder);
                }
                else
                {
                    return GenerarPDFReciboFicticio(IdCliente,NombreCliente, Direccion, FechaDiaEntrega, 
                        HoraEntrega, DescripcionVenta, PrecioTotalVenta);
                }
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }

        }

        public ActionResult GenerarPDFReciboFicticio(string IdCliente = "", string NombreCliente = "", string Direccion = "",
            string FechaDiaEntrega = "", string HoraEntrega = "", string DescripcionVenta = "", decimal PrecioTotalVenta = 0)
        {
            LocalReport lr = new LocalReport();
           

            string rutaRep = ConfigurationManager.AppSettings["RutaReporteRecibo"].ToString();
            lr.EnableExternalImages = true;
            lr.ReportPath = rutaRep;

            List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
            lr.DataSources.Add(new ReportDataSource("DataSet1", lobeProdsPedido));
            lr.SetParameters(new ReportParameter("NroPedido", IdCliente));
            lr.SetParameters(new ReportParameter("NombreCliente", NombreCliente));
            lr.SetParameters(new ReportParameter("DireccionCliente", Direccion));
            lr.SetParameters(new ReportParameter("DiaFechaEntrega", FechaDiaEntrega));
            lr.SetParameters(new ReportParameter("HoraEntrega", HoraEntrega));
            lr.SetParameters(new ReportParameter("DescripcionVenta", DescripcionVenta));
            lr.SetParameters(new ReportParameter("PrecioVentaTotal", PrecioTotalVenta.ToString()));

            string reportType;
            string format = "PDF";
            if (format == null || format == "" || format == "jpeg")
            {
                reportType = "Image";
                format = "jpeg";
            }
            else
            {
                reportType = format;
            }
            string mimeType;
            string encoding;
            string fileNameExtension;
            //string deviceInfo;
            string deviceInfo = "<DeviceInfo>" +
                            "   <OutputFormat>" + format + "</OutputFormat>" +
                            "  <PageWidth>9in</PageWidth>" +
                            "  <PageHeight>12in</PageHeight>" +
                            //"  <MarginTop>0.5in</MarginTop>" +
                            "  <MarginLeft>0.75in</MarginLeft>" +
                            //"  <MarginRight>1in</MarginRight>" +
                            //"  <MarginBottom>0.5in</MarginBottom>" +
                            "</DeviceInfo>";
            Warning[] warnings = null;
            string[] streams = null;
            byte[] renderedBytes = null;

            renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public ActionResult VerificarStock(string idSesion = "")
        {
            try
            {
                LocalReport lr = new LocalReport();
                List<ProductOrderModels> lobeProdsPedido = (List<ProductOrderModels>)Session["lobeProdsPedido"];
                lobeProdsPedido = FiltrarProdsSinStock(lobeProdsPedido);
                if (lobeProdsPedido!=null && lobeProdsPedido.Count > 0)
                {
                    OrderModels order;
                    order = (OrderModels)Session["orderModels"];
                    if (order == null)
                    {
                        order = new OrderModels();
                        order.lobeProducto = lobeProdsPedido;
                    }

                    List<OrderModels> lobeOrder = new List<OrderModels>();
                    lobeOrder.Add(order);

                    string rutaRep = ConfigurationManager.AppSettings["RutaReportePedido"].ToString();
                    lr.EnableExternalImages = true;
                    lr.ReportPath = rutaRep;

                    lr.DataSources.Add(new ReportDataSource("DataSet1", lobeOrder));
                    lr.DataSources.Add(new ReportDataSource("DataSet2", lobeProdsPedido));

                    string reportType;
                    string format = "PDF";
                    if (format == null || format == "" || format == "jpeg")
                    {
                        reportType = "Image";
                        format = "jpeg";
                    }
                    else
                    {
                        reportType = format;
                    }
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    //string deviceInfo;
                    string deviceInfo = "<DeviceInfo>" +
                                    "   <OutputFormat>" + format + "</OutputFormat>" +
                                    "  <PageWidth>9in</PageWidth>" +
                                    "  <PageHeight>12in</PageHeight>" +
                                    //"  <MarginTop>0.5in</MarginTop>" +
                                    "  <MarginLeft>0.75in</MarginLeft>" +
                                    //"  <MarginRight>1in</MarginRight>" +
                                    //"  <MarginBottom>0.5in</MarginBottom>" +
                                    "</DeviceInfo>";
                    Warning[] warnings = null;
                    string[] streams = null;
                    byte[] renderedBytes = null;

                    renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    return File(renderedBytes, mimeType);
                }
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }
            catch(Exception ex)
            {
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }
        }

        private List<ProductOrderModels> FiltrarProdsSinStock(List<ProductOrderModels> lobeProd)
        {
            if (lobeProd != null)
            {
                List<ProductOrderModels> lobeProdFiltrada = new List<ProductOrderModels>();
                ProductModels oProdOri;
                ProductOrderModels oProdNuevo;
                foreach(ProductOrderModels oProd in lobeProd)
                {
                    oProdOri = db.ProductModels.Find(oProd.IdProducto);
                    if (oProd.cantidadPedida > oProdOri.Stock)
                    {
                        oProdNuevo = new ProductOrderModels() {cantidadPedida= oProd.cantidadPedida- oProdOri.Stock, Id=0,
                            IdProducto =oProd.IdProducto, Nombre=oProd.Nombre, PrecioCosto=oProd.PrecioCosto,
                            PrecioVenta = oProd.PrecioVenta
                        };
                        lobeProdFiltrada.Add(oProdNuevo);
                    }
                }
                return lobeProdFiltrada;
            }
            return null;
        }

        // GET: ProductModels
        public ActionResult AgregarNota(string prodNoExistente)
        {
            List<ProductOrderModels> lobeProdNoExistente =(List<ProductOrderModels>) Session["lobeProdNoExistente"];
            ProductOrderModels oProd = new ProductOrderModels()
            {
                Id = 0,
                IdProducto=0,
                Nombre = prodNoExistente,
                cantidadPedida = 0,
                UrlImagen = "",
                PrecioCosto = 0,
                PrecioVenta = 0
            };
            if (lobeProdNoExistente != null)
            {
                lobeProdNoExistente.Add(oProd);
            }else
            {
                lobeProdNoExistente = new List<ProductOrderModels>();
                lobeProdNoExistente.Add(oProd);
            }
            Session["lobeProdNoExistente"] = lobeProdNoExistente;
            return RedirectToAction("Index", "ProductModels", new { strBus = "" });
            //return RedirectToAction("Index",  prods.ToList());
        }

        public ActionResult ProductosNoExistentesPDF(string idSesion = "")
        {
            try
            {
                LocalReport lr = new LocalReport();
                List<ProductOrderModels> lobeProdNoExistente = (List<ProductOrderModels>)Session["lobeProdNoExistente"];
                if (lobeProdNoExistente != null && lobeProdNoExistente.Count > 0)
                {
                    OrderModels order;
                    order = (OrderModels)Session["orderModels"];
                    if (order == null)
                    {
                        order = new OrderModels();
                        order.lobeProducto = lobeProdNoExistente;
                    }

                    List<OrderModels> lobeOrder = new List<OrderModels>();
                    lobeOrder.Add(order);

                    string rutaRep = ConfigurationManager.AppSettings["RutaReportePedido"].ToString();
                    lr.EnableExternalImages = true;
                    lr.ReportPath = rutaRep;

                    lr.DataSources.Add(new ReportDataSource("DataSet1", lobeOrder));
                    lr.DataSources.Add(new ReportDataSource("DataSet2", lobeProdNoExistente));

                    string reportType;
                    string format = "PDF";
                    if (format == null || format == "" || format == "jpeg")
                    {
                        reportType = "Image";
                        format = "jpeg";
                    }
                    else
                    {
                        reportType = format;
                    }
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    //string deviceInfo;
                    string deviceInfo = "<DeviceInfo>" +
                                    "   <OutputFormat>" + format + "</OutputFormat>" +
                                    "  <PageWidth>9in</PageWidth>" +
                                    "  <PageHeight>12in</PageHeight>" +
                                    //"  <MarginTop>0.5in</MarginTop>" +
                                    "  <MarginLeft>0.75in</MarginLeft>" +
                                    //"  <MarginRight>1in</MarginRight>" +
                                    //"  <MarginBottom>0.5in</MarginBottom>" +
                                    "</DeviceInfo>";
                    Warning[] warnings = null;
                    string[] streams = null;
                    byte[] renderedBytes = null;

                    renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                    return File(renderedBytes, mimeType);
                }
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("VerPedido", "ProductModels", new { idSesion = "" });
            }
        }
    }
}
