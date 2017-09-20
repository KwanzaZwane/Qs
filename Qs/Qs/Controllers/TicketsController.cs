using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Qs.Models;
using Microsoft.AspNet.Identity;

namespace Qs.Controllers
{
    public class TicketsController : Controller
    {
        private QsContext db = new QsContext();

        // GET: Tickets
        public ActionResult Index()
        {
            DateTime Today = DateTime.Now;
            var currentBranch = User.Identity.GetUserId();

            AspNetUser Branch = db.AspNetUsers.Find(currentBranch);
            var tickets = db.Tickets.Include(t => t.AspNetUser).Include(t => t.FunctionType).Where(p=>p.BranchId == currentBranch);

            ViewBag.BranchName = Branch.BranchName;
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            Ticket newTicket = new Ticket()
            {
                BranchId = User.Identity.GetUserId()
            };

            ViewBag.FunctionTypeId = new SelectList(db.FunctionTypes, "Id", "FunctionName");
            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                DateTime Now = DateTime.Now;
                ticket.TicketNumber = getNextTicketNumber(ticket.BranchId);
                ticket.DateTimeIssued = Now;

                db.Tickets.Add(ticket);
                db.SaveChanges();
                TempData["success"] = "New Ticket created!";
                return RedirectToAction("TicketOptions", new { ticket = ticket });
            }
            TempData["error"] = "Error creating Ticket!";

            ViewBag.FunctionTypeId = new SelectList(db.FunctionTypes, "Id", "FunctionName", ticket.FunctionTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.AspNetUsers, "Id", "Email", ticket.BranchId);
            ViewBag.FunctionTypeId = new SelectList(db.FunctionTypes, "Id", "FunctionName", ticket.FunctionTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketNumber,BranchId,FunctionTypeId,DateTimeIssued,DateTimeHelped,DateTimeEnd")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.AspNetUsers, "Id", "Email", ticket.BranchId);
            ViewBag.FunctionTypeId = new SelectList(db.FunctionTypes, "Id", "FunctionName", ticket.FunctionTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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

        public int getNextTicketNumber(string BranchId )
        {
            

            DateTime Now = DateTime.Now;

            var latestTicket = db.Tickets.Where(m => m.BranchId == BranchId && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year).OrderByDescending(o => o.TicketNumber).FirstOrDefault();
            if(latestTicket == null)
            {
                return 1;
            }
            int ticketNumber = latestTicket.TicketNumber;
            ticketNumber++;
            return ticketNumber++;

        }

        public ActionResult TicketScreen(string functionType)
        {
            ViewBag.FunctionType = functionType;

            return View();
        }

        [HttpGet]
        public JsonResult getTicketsForQueue(string functionType)
        {
            var currentBranch = User.Identity.GetUserId();
            DateTime Now = DateTime.Now;

            var ticketsInQueue = db.Tickets.Where(m => m.BranchId == currentBranch && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year && m.DateTimeHelped == null && m.DateTimeEnd == null && m.FunctionType.FunctionName == functionType).Select(p => new { Id = p.Id, TicketNumber = p.TicketNumber }).Take(5);

            return Json(new { result = ticketsInQueue }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult getTicketForHelped(string functionType)
        {
            var currentBranch = User.Identity.GetUserId();
            DateTime Now = DateTime.Now;

            var ticketsInHelped = db.Tickets.Where(m => m.BranchId == currentBranch && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year && m.DateTimeHelped != null && m.DateTimeEnd == null && m.FunctionType.FunctionName == functionType).Select(p => new { Id = p.Id, TicketNumber = p.TicketNumber });

            return Json(new { result = ticketsInHelped }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult TicketOptions(Ticket ticket)
        {



            return View(ticket);
        }

        //[HttpGet]
        //public JsonResult clearTicketsNotServed(string Type)
        //{
        //    var currentBranch = User.Identity.GetUserId();
        //    DateTime Now = DateTime.Now;

        //    var ticketsInHelped = db.Tickets.Where(m => m.BranchId == currentBranch && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year && m.DateTimeHelped != null && m.DateTimeEnd == null && m.FunctionType.FunctionName == Type);

        //    return Json(new { result = ticketsInHelped }, JsonRequestBehavior.AllowGet);

        //}

    }
}
