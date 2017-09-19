using Qs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Qs.Controllers
{
    public class BranchFunctionsController : Controller
    {
        private QsContext db = new QsContext();

        // GET: BranchFunctions
        public ActionResult Index(string id)
        {
            AspNetUser branch = db.AspNetUsers.Find(id);
            var branchFunctions = branch.FunctionTypes.ToList();

            ViewBag.branchId = branch.Id;

            return View(branchFunctions);

        }

        // GET: BranchFunctions/Create
        public ActionResult Create(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser existingBranch = db.AspNetUsers.Find(id);
            if (existingBranch == null)
            {
                TempData["error"] = "Unable to find Branch!";
                return RedirectToAction("Index");
            }

            BranchFunctionsVM model = new BranchFunctionsVM()
            {
                BranchId = existingBranch.Id
            };

            ViewBag.BranchName = existingBranch.BranchName;

            model.BranchId = existingBranch.Id;

            ViewBag.FunctionId = new SelectList(db.FunctionTypes, "Id", "FunctionName", model.FunctionId);


            return View(model);
        }

        // POST: BranchFunctions/Create
        [HttpPost]
        public ActionResult Create(BranchFunctionsVM branchFunction)
        {
            if (ModelState.IsValid)
            {

                AspNetUser currBranch = db.AspNetUsers.Find(branchFunction.BranchId);
                if(currBranch == null)
                {
                    TempData["error"] = "Unable to find Branch!";
                    ViewBag.FunctionId = new SelectList(db.FunctionTypes, "Id", "FunctionName", branchFunction.FunctionId);

                    return View(branchFunction);
                }

                FunctionType newBranchFunction = db.FunctionTypes.Find(branchFunction.FunctionId);
                if (newBranchFunction == null)
                {
                    TempData["error"] = "Unable to find Function!";
                    ViewBag.FunctionId = new SelectList(db.FunctionTypes, "Id", "FunctionName", branchFunction.FunctionId);

                    return View(branchFunction);
                }

                currBranch.FunctionTypes.Add(newBranchFunction);
                db.SaveChanges();
                TempData["success"] = "Function successfully added to Branch!";

                return RedirectToAction("Index", new { id = branchFunction.BranchId });
            }

            TempData["error"] = "Error adding user to Business Unit";
            return View(branchFunction);
        }

        // GET: BranchFunctions/Delete/5
        public ActionResult Delete(string BranchId, int? FunctionId)
        {
            if (BranchId == null || FunctionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            AspNetUser existingBranch = db.AspNetUsers.Find(BranchId);

            var branchFunction = existingBranch.FunctionTypes.Where(p => p.Id == FunctionId).SingleOrDefault();

            if (branchFunction == null)
            {
                return HttpNotFound();
            }

            BranchFunctionsVM model = new BranchFunctionsVM()
            {
                BranchId = existingBranch.Id,
                FunctionId = branchFunction.Id,
                BranchName = existingBranch.BranchName,
                FunctionName = branchFunction.FunctionName,

            };

            return View(model);
        }

        // POST: UserBusinessUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(BranchFunctionsVM model)
        {
            AspNetUser existingBranch = db.AspNetUsers.Find(model.BranchId);

            var branchFunction = existingBranch.FunctionTypes.Where(p => p.Id == model.FunctionId).SingleOrDefault();

            existingBranch.FunctionTypes.Remove(branchFunction);
            db.SaveChanges();
            TempData["success"] = "Business Unit successfully removed from User!";
            return RedirectToAction("Index", new { id = model.BranchId });
        }
    }
}
