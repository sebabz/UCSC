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

            //Obtiene el objeto del modelo de orden entrada
            var ordenentrada = db.OrdenEntrada.Find(id);

            //Buscar el id detalle dependiendo del id orden
            //Select * from detalle_entrada where id_orden = id
            //Trae un listado (Iqueryable)
            var listadoConjuntosDetalleEntrada = db.DetalleEntrada.Where(s => s.id_orden == id);

            //esta variable se crea para capturar el id_detalle
            int datoParaEliminar = 0;

            //Ciclo para recorrer el listado de objetos detalles obtenidos por la consulta "Select * from..."
            foreach (var fila in listadoConjuntosDetalleEntrada)
            {
                if (fila.id_orden == id)
                {
                    datoParaEliminar = fila.id_detalle;
                    break;
                }
            }

            //Obtener el idDetalle para eliminar
            var idDetalle = db.DetalleEntrada.Find(datoParaEliminar);

            //Elimina el objeto detalle salida
            try
            {
                db.DetalleEntrada.Remove(idDetalle);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            try
            {

                if (ordenentrada != null)
                {
                    db.OrdenEntrada.Remove(ordenentrada);
                    db.SaveChanges();
                    return Json("");
                }
            }
            catch (Exception)
            {
                return Json("No se ha podido eliminar la Orden");
            }
            return Json("No se puede eliminar esta orden");
        }


        //editar detalle de entrada
        public ActionResult Edit(int? id)
        {
            ViewBag.epp = new SelectList(db.EPP, "id_epp", "nombre");
            var detalleEntrada = db.DetalleEntrada.Where(d => d.id_detalle == id);
            //var detalleEntrada = db.DetalleEntrada.Find(id);

           
            return PartialView("_Edit", detalleEntrada);
        }
        [HttpPost]
        public ActionResult Edit(DetalleEntrada detalleEntrada)
        {

            db.Entry(detalleEntrada).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }





    }
}