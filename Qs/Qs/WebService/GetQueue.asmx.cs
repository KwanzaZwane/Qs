using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Qs.Models;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Qs.Controllers;

namespace Qs.WebService
{
    /// <summary>
    /// Summary description for GetQueue
    /// </summary>
    //[WebService(Namespace = "http://qshack-001-site1.htempurl.com/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class GetQueue : System.Web.Services.WebService
    {
        private QsContext db = new QsContext();
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public int GetQueueForBranch(string GoogleId, string FunctionType)
        {
            DateTime Now = DateTime.Now;

            int QueueCount = db.Tickets.Where(m => m.AspNetUser.BranchGoogleId == GoogleId && m.FunctionType.FunctionName == FunctionType && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year && m.DateTimeHelped == null && m.DateTimeEnd == null).Count();

            return QueueCount;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CreateTicket(string GoogleId, string FunctionType)
        {
            AspNetUser branch = db.AspNetUsers.Where(m => m.BranchGoogleId == GoogleId).SingleOrDefault();

            FunctionType function = db.FunctionTypes.Where(m => m.FunctionName == FunctionType).SingleOrDefault();

            if(branch == null)
            {
                return "Branch Not Found";
            }

            DateTime Now = DateTime.Now;

            int ticketNumber = new TicketsController().getNextTicketNumber(branch.Id);

            Ticket ticket = new Ticket()
            {
                TicketNumber = ticketNumber,
                DateTimeIssued = Now,
                FunctionTypeId = function.Id,
                BranchId = branch.Id
            };

            db.Tickets.Add(ticket);
            db.SaveChanges();


            var data = new[]
            {
                  new { value = ticket.Id, data = ticket.TicketNumber },
            };

            return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

        }



        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public int GetPositionInQueue(int ticketId)
        {

            DateTime Now = DateTime.Now;
            Ticket ticket = db.Tickets.Where(m => m.Id == ticketId && m.DateTimeEnd == null && m.DateTimeHelped == null).SingleOrDefault();
            if(ticket == null)
            {
                return -1;
            }

            var ticketsInQueue = db.Tickets.Where(m => m.FunctionTypeId == ticket.FunctionTypeId && m.BranchId == ticket.BranchId && m.DateTimeIssued.Day == Now.Day && m.DateTimeIssued.Month == Now.Month && m.DateTimeIssued.Year == Now.Year && m.DateTimeHelped == null && m.DateTimeEnd == null).ToList();

            int count = 1;
            foreach(var item in ticketsInQueue)
            {
                if (item.Id == ticket.Id)
                {

                    return count;
                }
                count++;
            }

            return -1;
        }
    }
}


