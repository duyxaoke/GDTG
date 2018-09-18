using System.Collections.Generic;
using System.Web.Mvc;
using Web.Infrastructure;


namespace Web.Areas.Admin.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ClaimedActionsProvider claimedActionsProvider;


        public ClaimsController(ClaimedActionsProvider claimedActionsProvider)
        {
            this.claimedActionsProvider = claimedActionsProvider;
        }


        public ActionResult Index()
        {
            var viewModel = new ClaimsIndexViewModel()
            {
                Claims = claimedActionsProvider.GetClaimGroups(),
            };

            return View(viewModel);
        }
    }


    public class ClaimsIndexViewModel
    {
        public List<ClaimsGroup> Claims { get; set; }
    }
}