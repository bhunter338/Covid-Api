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
        private readonly CovidDataRepo _repo = new CovidDataRepo();
        [HttpGet]
        public ActionResult<TotalData> GetGlobalData()
        {

            return Ok();
        }

        [HttpGet("{country}")]
        public ActionResult<TotalData> GetTotalDataByCountry(string country)
        {
            var data = _repo.GetTotalData(country);
            return Ok(data);
        }

        [HttpGet("{country}/daily")]
        public ActionResult<TotalData> GetDailyTotalDataByCountry(string country)
        {
            var data = _repo.GetDailyTotalData(country);
            return Ok(data);
        }

    }
}