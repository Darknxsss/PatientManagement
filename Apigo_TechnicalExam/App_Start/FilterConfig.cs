using System.Web;
using System.Web.Mvc;

namespace Apigo_TechnicalExam
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
