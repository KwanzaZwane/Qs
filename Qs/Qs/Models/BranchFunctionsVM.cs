using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Qs.Models
{
    public class BranchFunctionsVM
    {
        public BranchFunctionsVM()
        {
            FunctionList = new List<System.Web.Mvc.SelectListItem>();

        }
        public string Id { get; set; }

        [Display(Name = "Branch")]
        public string BranchId { get; set; }

        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Display(Name = "Branch Function")]
        public List<System.Web.Mvc.SelectListItem> FunctionList { get; set; }

        [Display(Name = "Branch Function")]
        public int FunctionId { get; set; }

        [Display(Name = "Function Name")]
        public string FunctionName { get; set; }

    }
}