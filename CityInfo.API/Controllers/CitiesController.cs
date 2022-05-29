using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using CityInfo.API.Services;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CityInfo.API.Controllers
{
    [ApiController]
    // [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get all cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            [FromQuery] string? name, [FromQuery] string? searchQuery, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include points of interest in the response</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            else
            {
                return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
            }
        }
    }
}
