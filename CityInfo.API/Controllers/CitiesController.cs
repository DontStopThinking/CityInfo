using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        // Get all cities
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        // Get specific city
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            CityDto? result = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
