using Microsoft.AspNetCore.Mvc;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using System.Threading.Tasks;

namespace Pluviometrico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasuredRainfallController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeasuredRainfallController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Retorna apenas os 10 primeiros
        //TODO: paging
        //TODO: DTOs
        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> GetByDate(int month, int year)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetListByMonthAndYear(month, year);
            return Ok(response);
        }

        [HttpGet("{greaterThanYear:int}/{lessThanYear:int}/{distance:double}")]
        public async Task<IActionResult> GetByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetByDistanceAndYearRange(greaterThanYear, lessThanYear, distance);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDistanceAndYear([FromQuery] int year, [FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetByDistanceAndYear(year, distance);
            return Ok(response);
        }
    }
}
