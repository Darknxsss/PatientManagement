using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Apigo_TechnicalExam.Controllers
{
    public class HomeController : Controller
    {
        ApigoCRUDDbEntities _context = new ApigoCRUDDbEntities();
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var totalPatients = _context.Patients.Count();
            var patients = _context.Patients
                .OrderBy(p => p.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalPatients / (double)pageSize);

            return View(patients);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Patient model)
        {
            try
            {
                var patientNo = await GetPatientNoAsync();
                model.patientNo = patientNo;

                _context.Patients.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Patient Added Successfully!";

            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }
            return View(new Patient());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = _context.Patients.Where(x => x.id == id).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Patient model)
        {
            var data = _context.Patients.Where(x => x.id == model.id).FirstOrDefault();
            if (data != null)
            {
                data.firstName = model.firstName;
                data.middleName = model.middleName;
                data.lastName = model.lastName;
                data.suffixName = model.suffixName;
                data.birthDate = model.birthDate;
                data.gender = model.gender;
                data.initialDiagnosis = model.initialDiagnosis;
                _context.SaveChanges();
                TempData["Message"] = "Patient Updated Successfully!";
            }
            return RedirectToAction("index");
        }
        public ActionResult Detail(int id)
        {
            var data = _context.Patients.Where(x => x.id == id).FirstOrDefault();
            return View(data);
        }
        public ActionResult Delete(int id)
        {
            var data = _context.Patients.Where(x => x.id == id).FirstOrDefault();
            _context.Patients.Remove(data);
            _context.SaveChanges();
            TempData["Message"] = "Patient Data Deleted Successfully!";
            return RedirectToAction("index");
        }
        private async Task<string> GetPatientNoAsync()
        {
            // Construct the API URL
            string apiUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/api/PatientNoAPI/GetPatientNo";

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<dynamic>(jsonString);
                        return result.PatientNumber;
                    }
                    else
                    {
                        throw new Exception("Unable to generate Patient Number from the API.");
                    }
                }
                catch (Exception ex)
                {
                    // Log or re-throw the exception as needed
                    throw new Exception($"Error calling GetPatientNo API: {ex.Message}");
                }
            }
        }
    }
}