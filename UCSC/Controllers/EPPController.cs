using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCSC.Models;

namespace UCSC.Controllers
{
    public class EPPController : Controller
    {
        private gestion_eppEntities db = new gestion_eppEntities();


        // GET: EPP
        public ActionResult Index()
        {

            var EPP = db.EPP.ToList();
            return View(EPP);
        }


        public ActionResult Create()
        {
            ViewBag.categoria = new SelectList(db.Categoria, "id_categoria", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "id_usuario", "nombre");

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

        public ActionResult Edit(int id)
        {
           
            var epp = db.EPP.Find(id);
            
            ViewBag.proveedor = new SelectList(db.Usuario, "id_usuario", "nombre", epp.id_user);
            return PartialView("_Edit", epp);
        }
        [HttpPost]
        public ActionResult Edit(EPP epp)
        {

            db.Entry(epp).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }





    }
}