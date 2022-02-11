using System.Collections;
using System.Collections.Generic;
using Covid_Api.Data;
using Covid_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Covid_Api.Controllers
{
    [Route("api/covidData")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataRepo _repo;

        public CovidDataController(ICovidDataRepo repo)
        {
            _repo = repo;
        }
        // private readonly CovidDataRepo _repo = new CovidDataRepo();
        [HttpGet("countries")]
        public ActionResult<IEnumerable<string>> GetCountries()
        {
            var data = _repo.GetCountries();

            if (data != null)
            {
                return Ok(data);
            }

            return NotFound();


        }


        [HttpGet("{country}")]
        public ActionResult<DailyData> GetDailyTotalDataByCountry(string country)
        {
            var data = _repo.GetTotalDataByCountry(country);

            if (data != null)
            {
                return Ok(data);
            }

            return NotFound();

        }

    }
}