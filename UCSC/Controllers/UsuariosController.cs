using UCSC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCSC.Controllers
{

    [OutputCache(VaryByCustom = "*", Duration = 0)]
    public class UsuariosController : Controller
    {
        private gestion_eppEntities1 db = new gestion_eppEntities1();
        // GET: Usuarios


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(String email, string pass)
        {
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
            {
                
                var usuario = db.Usuario.FirstOrDefault(u => u.email.Equals(email) && u.pass.Equals(pass));
                if(usuario != null)
                {

                    Session["tipoUsuario"] = usuario.TipoUsuario.nombre;
                    Session["usuario"] = usuario.nombre + " " + usuario.apellido;

                    Session["idusuario"] = usuario.id_usuario;
                    Session["idTipoUsuario"] = usuario.TipoUsuario.id_tipo;
                    return RedirectToAction("ListadoSalidas", "OrdenSalidas");
                }
            }


            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {

            var usuario = db.Usuario.ToList();
            return View(usuario);

         
        }

        public ActionResult Create()
        {
            ViewBag.tipo = new SelectList(db.TipoUsuario, "id_tipo", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return Json("");
            }

            catch (Exception ex)
                        {
                Console.WriteLine(ex.ToString());
                return Json("Ha ocurrido un error, intentelo mas tarde.");
            }
        }

        public ActionResult Edit(int? id)
        {

            var usuario = db.Usuario.Find(id);

            ViewBag.tipo = new SelectList(db.TipoUsuario, "id_tipo", "nombre", usuario.id_tipo);
            ViewBag.estado = new SelectList(db.Estado, "id_estado", "nombre", usuario.id_estado);
            return PartialView("_Edit", usuario);
        }
        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {

            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();
            return Json("");
        }


        public ActionResult Delete(int id)
        {
            var usuario = db.Usuario.Find(id);
            return PartialView("_Delete", usuario);
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var usuario = db.Usuario.Find(id);

            try
            {

                if (usuario != null)
                {
                    db.Usuario.Remove(usuario);
                    db.SaveChanges();
                    return Json("");
                }
            }
            catch (Exception)
            {
                return Json("No se ha podido eliminar el Usuario");
            }
            return Json("No se puede eliminar un usuario que este asignado");
        }

        



     }
}