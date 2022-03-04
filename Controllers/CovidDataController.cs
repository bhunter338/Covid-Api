using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Covid_Api.Data;
using Covid_Api.Dtos;
using Covid_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Covid_Api.Controllers
{
    [Route("api/covidData")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataRepo _repo;
        private readonly IMapper _mapper;

        public CovidDataController(ICovidDataRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
        public ActionResult<DailyDataReadDto> GetDailyTotalDataByCountry(string country)
        {
            var data = _repo.GetTotalDataByCountry(country);

            if (data != null)
            {
                return Ok(_mapper.Map<DailyDataReadDto>(data));
            }

            return NotFound();

        }

        [HttpGet("historical/{country}")]
        public ActionResult<List<DailyDataReadDto>> GetHistoricalData(string country)
        {
            var data = _repo.GetHistroicalDataByCountry(country);

            if (data != null)
            {
                return Ok(_mapper.Map<List<DailyDataReadDto>>(data));
            }
            return NotFound();
        }

    }
}