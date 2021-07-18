using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class EPPController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();


        // GET: EPP
        public ActionResult Index()
        {

            var EPP = db.EPP.ToList();
            return View(EPP);
        }


        public ActionResult Create()
        {
            ViewBag.categoria = new SelectList(db.Categoria, "id_categoria", "nombre");
            //ViewBag.usuario = new SelectList(db.Usuario, "id_usuario", "nombre");

            return View();
        }

        [HttpPost]
        public ActionResult Create(EPP epp)
        {
            try
            {
                db.EPP.Add(epp);
                db.SaveChanges();
                return Json("");
            }

            catch (Exception)
            {
                return Json("Ha ocurrido un error, intentelo mas tarde.");
            }
        }

        public ActionResult Edit(int? id)
        {
           
            var epp = db.EPP.Find(id);
            
            ViewBag.Usuario = new SelectList(db.Usuario, "id_usuario", "nombre", epp.id_usuario);
            ViewBag.Categoria = new SelectList(db.Categoria, "id_categoria", "nombre", epp.id_categoria);
            ViewBag.estado = new SelectList(db.Estado, "id_estado", "nombre", epp.id_estado);
            return PartialView("_Edit", epp);
        }
        [HttpPost]
        public ActionResult Edit(EPP epp)
        {

            db.Entry(epp).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }


        public ActionResult Delete(int id)
        {
            var epp = db.EPP.Find(id);
            return PartialView("_Delete", epp);
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var epp = db.EPP.Find(id);
            if (epp != null)
            {
                db.EPP.Remove(epp);
                db.SaveChanges();
                return Json("");
            }
            return Json("No se ha podido eliminar el producto");
        }


    }
}