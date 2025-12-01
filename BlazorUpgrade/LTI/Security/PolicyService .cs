using Microsoft.AspNetCore.Authorization;

namespace CalibrationSaaS.Domain.Aggregates.Security
{
    public class PolicyService : IPolicyService
    {
        //public PolicyService(ILocationService locationService, IPageInfoService pageInfoService)
        //{
        //	LocationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        //	PageInfoService = pageInfoService ?? throw new ArgumentNullException(nameof(pageInfoService));
        //}

        //public ILocationService LocationService { get; }
        //public IPageInfoService PageInfoService { get; }

        public string GetPolicy(string policyName, string pageHref = null, long? id = null)
        {
            //NavPage currentPage = null;
            //if (pageHref == null)
            //{
            //	currentPage = PageInfoService.CurrentPage;
            //}

            //var policyState = new PolicyState
            //{

            //	PageHref = pageHref ?? currentPage?.Href,
            //	PrimaryId = id ?? currentPage?.CurrentId,
            //	CurrentLocation = LocationService.CurrentLocation
            //};

            return ""; // $"{policyName}|{JsonConvert.SerializeObject(policyState)}";
        }





    }

    //public static class Policies
    //{

    //	public const string IsUser = "pepe";


    //	public const string HasView = "HasView";

    //	public const string HasEdit = "HasEdit";

    //	public const string HasApprove = "HasApprove";

    //	public const string HasSpecialEdit = "HasSpecialEdit";


    //	public static string test()
    //       {
    //		return "";
    //       }


    //}
    public static class Policies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsTech";

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("admin")
                                                   .Build();
        }

        public static AuthorizationPolicy IsUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("tech")
                                                   .Build();
        }
    }
}
