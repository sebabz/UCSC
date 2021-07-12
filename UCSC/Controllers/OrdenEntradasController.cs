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
            var detalle = db.OrdenEntrada.Find(id);
            return View(detalle);
        }

    }
}