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
            orden.user_pide = 1015;
            orden.user_entrega = 1015;
            orden.id_estado = 5;
                        
            db.OrdenSalida.Add(orden);
            db.SaveChanges();


            var idDet = orden.id_orden;
            foreach (var item in detalles)
            {

                item.id_orden = idDet;
                db.DetalleSalida.Add(item);

                ActualizaStock(item.id_epp, item.cantidad);
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

    
        
        
       
    
        public ActionResult EstadoConsulta(int? id)
        {

            var consulta = db.OrdenSalida.Find(id);
            //var ordenes = db.OrdenSalida.Where(s=> s.id_orden == id);
            return View();
        }

        
        public ActionResult PopupEstadoConsulta(int? id_pedido)
        {
            OrdenSalida orden = db.OrdenSalida.Find(id_pedido);
            return PartialView("_PopupEstadoConsulta", orden);
            
        }




    }
}