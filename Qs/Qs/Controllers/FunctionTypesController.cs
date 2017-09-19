using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Qs.Models;

namespace Qs.Controllers
{
    public class FunctionTypesController : Controller
    {
        private QsContext db = new QsContext();

        // GET: FunctionTypes
        public ActionResult Index()
        {
            return View(db.FunctionTypes.ToList());
        }

        // GET: FunctionTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionType functionType = db.FunctionTypes.Find(id);
            if (functionType == null)
            {
                return HttpNotFound();
            }
            return View(functionType);
        }

        // GET: FunctionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FunctionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FunctionName")] FunctionType functionType)
        {
            if (ModelState.IsValid)
            {
                db.FunctionTypes.Add(functionType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(functionType);
        }

        // GET: FunctionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionType functionType = db.FunctionTypes.Find(id);
            if (functionType == null)
            {
                return HttpNotFound();
            }
            return View(functionType);
        }

        // POST: FunctionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FunctionName")] FunctionType functionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(functionType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(functionType);
        }

        // GET: FunctionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionType functionType = db.FunctionTypes.Find(id);
            if (functionType == null)
            {
                return HttpNotFound();
            }
            return View(functionType);
        }

        // POST: FunctionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FunctionType functionType = db.FunctionTypes.Find(id);
            db.FunctionTypes.Remove(functionType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
