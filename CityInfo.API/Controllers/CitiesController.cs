using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        // Get all cities
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        // Get specific city
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            CityDto? result = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
