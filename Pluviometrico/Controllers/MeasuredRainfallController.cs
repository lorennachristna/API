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

        [HttpGet("{greaterThanYear}/{lessThanYear}/{distance:double}")]
        public async Task<IActionResult> GetByDistance(int greaterThanYear, int lessThanYear, double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetByDistance(greaterThanYear, lessThanYear, distance);
            return Ok(response);
        }
    }
}
