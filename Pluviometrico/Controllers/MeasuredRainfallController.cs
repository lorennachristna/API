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

        [HttpGet]
        public async Task<MeasuredRainfall> Get()
        {
            var response = await _unitOfWork.MeasuredRainfallList.Get(115812084);
            return response;
        }
    }
}
