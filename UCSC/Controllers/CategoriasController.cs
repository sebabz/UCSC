using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{
    public class CategoriasController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: Categorias
        public ActionResult Index()
        {
            var categorias = db.Categoria.ToList();
            return View(categorias);
                     
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Categoria categoria)
        {
            db.Categoria.Add(categoria);
            db.SaveChanges();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            Categoria categoria = db.Categoria.Find(id);
            return PartialView("_Edit", categoria);
        }
        [HttpPost]
        public ActionResult Edit(Categoria categoria)
        {
            db.Entry(categoria).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var categoria = db.Categoria.Find(id);
                return PartialView("_Delete", categoria);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var categoria = db.Categoria.Find(id);
            db.Categoria.Remove(categoria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}