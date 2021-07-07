using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class CarrerasController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: Carreras
        public ActionResult Index()
        {
            var carrera = db.Carrera.ToList();
            return View(carrera);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Carrera carrera)
        {
            db.Carrera.Add(carrera);
            db.SaveChanges();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            Carrera carrera = db.Carrera.Find(id);
            return PartialView("_Edit", carrera);
        }
        [HttpPost]
        public ActionResult Edit(Carrera carrera)
        {
            db.Entry(carrera).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var carrera = db.Carrera.Find(id);
                return PartialView("_Delete", carrera);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var carrera = db.Carrera.Find(id);
            try
            {
                db.Carrera.Remove(carrera);
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