using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class OrdenEntradasController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: OrdenEntradas
        public ActionResult OrdenEntrada()
        {
            ViewBag.epp = new SelectList(db.EPP, "id_epp", "nombre");
            return View();
        }

        [HttpPost]
        public JsonResult OrdenEntrada(OrdenEntrada orden, List<DetalleEntrada> detalles)
        {
            
            orden.fecha = DateTime.Now;
            orden.id_usuario = 1015;
            db.OrdenEntrada.Add(orden);
            db.SaveChanges();
            
            var idDet = orden.id_orden;
            
            foreach (var item in detalles)
            {
               
                item.id_orden = idDet;
                db.DetalleEntrada.Add(item);
                
                ActualizaStock(item.id_epp, item.cantidad);
                db.SaveChanges();
            }
            return Json("");
        }
        public void ActualizaStock(int id_epp, int cantidad)
        {
           
            var epp = db.EPP.Find(id_epp);
            
            epp.cantidad = epp.cantidad + cantidad;
            db.Entry(epp).State = EntityState.Modified;
        }


        [HttpPost]
        public ActionResult EPPExiste(string nombre)
        {
            var q = db.EPP.FirstOrDefault(p => p.nombre.ToLower().Equals(nombre.ToLower()));
            if(q != null)
            {
                return Json(new { q.nombre, q.id_epp },JsonRequestBehavior.AllowGet);
            }
            return Json("");
        }

        public ActionResult Index ()
        {
            var ordenes = db.OrdenEntrada.ToList();
            return View(ordenes);
        }

       
        public ActionResult DetalleOrden(int id)
        {
            //Obtiene el id orden
            var detalle = db.OrdenEntrada.Find(id);

           
            return View(detalle);
        }

        
        public ActionResult Delete(int id)
        {
            var ordenentrada = db.OrdenEntrada.Find(id);
            return PartialView("_Delete", ordenentrada);
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var ordenEnt = db.OrdenEntrada.OrderBy(o => o.id_orden).Include(o => o.DetalleEntrada).First();

                db.OrdenEntrada.Remove(ordenEnt);

                db.SaveChanges();
                return Json("");
            }
            catch (Exception)
            {
                return Json("No se puede eliminar esta orden");
                throw;
            }
            




            
        }

            //Obtener el idDetalle para eliminar
            


        //editar detalle de entrada
        public ActionResult Edit(int? id)
        {
            ViewBag.epp = new SelectList(db.EPP, "id_epp", "nombre");
            //var detalleEntrada = db.DetalleEntrada.Where(d => d.id_detalle == id);
            var detalleEntrada = db.DetalleEntrada.Find(id);


            return PartialView("_Edit", detalleEntrada);
        }
        [HttpPost]
        public ActionResult Edit(DetalleEntrada detalleEntrada)
        {

            db.Entry(detalleEntrada).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }

        public ActionResult EditarDetalles (int? id)
        {
            var detalleentrada = db.DetalleEntrada.Find(id);
            ViewBag.epp = new SelectList(db.EPP, "id_epp", "nombre", detalleentrada.id_epp);
            return PartialView("_EditarDetalles", detalleentrada);
        }

        [HttpPost]
        public ActionResult EditarDetalles(DetalleEntrada detalleentrada)
        {
            try
            {
                db.Entry(detalleentrada).State = EntityState.Modified;
                db.SaveChanges();
                return Json("");
            }
            catch (Exception ex)
            {
                return Json("No se ha podido editar");
                throw;
            }
            
            
        }


    }
}