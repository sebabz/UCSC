using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class OrdenSalidasController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: OrdenSalida
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrdenSalida()
        {
            ViewBag.epp = new SelectList(db.EPP, "id_epp", "nombre");
            ViewBag.carrera = new SelectList(db.Carrera, "id_carrera", "nombre_carrera");
            return View();
        }

        [HttpPost]
        public JsonResult OrdenSalida(OrdenSalida orden, List<DetalleSalida> detalles)
        {
            orden.fecha_orden = DateTime.Now;
            orden.user_entrega = 1015;
            orden.id_estado = 5;
                        
            db.OrdenSalida.Add(orden);
            db.SaveChanges();


            var idDet = orden.id_orden;
            foreach (var item in detalles)
            {

                item.id_orden = idDet;
                db.DetalleSalida.Add(item);
                
                ActualizaStock((int)item.id_epp, item.cantidad);
                db.SaveChanges();
            }

            return Json("");
        }

        public void ActualizaStock(int id_epp, int cantidad)
        {

            var epp = db.EPP.Find(id_epp);

            epp.cantidad = epp.cantidad - cantidad;
            db.Entry(epp).State = EntityState.Modified;
        }

        public ActionResult ListadoSalidas()
        {
            var ordenes = db.OrdenSalida.ToList();
            return View(ordenes);
        }

        public ActionResult DetalleOrdenSalida(int id)
        {
            var detalle = db.OrdenSalida.Find(id);
            return View(detalle);
        }

            
        public ActionResult Saberestado() //index 
        {


            return View();
        }


        public ActionResult Parchalbiu(int? id)
        {
            var ordencita = db.OrdenSalida.Find(id);
            //ViewBag.ordencita = new SelectList(db.Estado, "id_estado", "nombre", ordencita.id_estado);
             return PartialView("_PopupEstadoConsulta", ordencita);
            //return PartialView("_PopupEstadoConsulta", ordencita);
        }

        [HttpPost]
        public ActionResult Saberestado(int estadoreal)
        {
            var estado = db.OrdenSalida.FirstOrDefault(u => u.id_orden.Equals(estadoreal));
    
            return View("Parchalbiu", estado);
        }



        
        public ActionResult Delete(int id)
        {
            var ordensalida = db.OrdenSalida.Find(id);
            return PartialView("_Delete", ordensalida);
        }

        public ActionResult Eliminar(int id)
        {
            try
            {
                var ordenSal = db.OrdenSalida.OrderBy(o => o.id_orden).Include(o => o.DetalleSalida).First();

                db.OrdenSalida.Remove(ordenSal);

                db.SaveChanges();
                return Json("");
            }
            catch (Exception ex)
            {
                return Json("No se puede eliminar esta orden");
                throw;
            }






        }


    }
}