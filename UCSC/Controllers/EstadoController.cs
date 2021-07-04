using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class EstadoController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: Estado
        public ActionResult Index()
        {

            var estado = db.Estado.ToList();
            return View(estado);
            
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Estado estado)
        {
            db.Estado.Add(estado);
            db.SaveChanges();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            Estado estado = db.Estado.Find(id);
            return PartialView("_Edit", estado);
        }
        [HttpPost]
        public ActionResult Edit(Estado estado)
        {
            db.Entry(estado).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var estado = db.Estado.Find(id);
                return PartialView("_Delete", estado);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var estado = db.Estado.Find(id);
            try
            {
                db.Estado.Remove(estado);
                db.SaveChanges();
                return Json("");
            }

            catch (Exception)
            {
                return Json("No se puede eliminar un Estado que ya se este utilizando.");
            }
        }



    }
}