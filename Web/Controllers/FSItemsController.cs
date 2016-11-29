using BusinessLogic;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class FSItemsController : ApiController
    {
        [Route("api/FSItems/{*dir?}")]
        public DirInfo Get(string dir = null)
        {
            BrowseService service = new BrowseService();

            return service.GetDirectoryInfo(dir);
        }
    }
}