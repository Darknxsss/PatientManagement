using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Apigo_TechnicalExam.Controllers
{
    public class PatientNoAPIController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetPatientNo()
        {
            var random = new Random();
            int randomNumber = random.Next(0, 100000000);
            var result = new { PatientNumber = randomNumber.ToString("D8") };
            return Ok(result);
        }
    }
}