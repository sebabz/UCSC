using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{

    public class TipousuarioController : Controller
    {
        private gestion_eppEntities db = new gestion_eppEntities();
        // GET: Tipousuario
        public ActionResult Index()
        {

            var tipousuario = db.TipoUsuario.ToList();
            return View(tipousuario);
            
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TipoUsuario tipoUsuario)
        {
            db.TipoUsuario.Add(tipoUsuario);
            db.SaveChanges();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            TipoUsuario tipoUsuario = db.TipoUsuario.Find(id);
            return PartialView("_Edit", tipoUsuario);
        }
        [HttpPost]
        public ActionResult Edit(TipoUsuario tipoUsuario)
        {
            db.Entry(tipoUsuario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var tipoUsuario = db.TipoUsuario.Find(id);
                return PartialView("_Delete", tipoUsuario);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var tipoUsuario = db.TipoUsuario.Find(id);
            db.TipoUsuario.Remove(tipoUsuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}