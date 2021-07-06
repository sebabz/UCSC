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




    }
}