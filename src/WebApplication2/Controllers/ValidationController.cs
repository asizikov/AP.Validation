using System.Web.Http;
using Domain;

namespace WebApplication2.Controllers
{
    public class ValidationController : ApiController
    {

       [HttpGet]
        public IHttpActionResult Validate(Account account)
       {
           var acc = new Account();
           var validate = acc.Validate();
           var validate2 = acc.Validate();

           return Ok(account);
        }
    }
}