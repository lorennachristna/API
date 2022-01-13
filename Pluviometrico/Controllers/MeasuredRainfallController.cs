using Microsoft.AspNetCore.Mvc;
using Pluviometrico.Core.Repository.Interface;
using System;
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

        //Retorna apenas os 10 primeiros?
        //TODO: Retornar status de erro quando der erro
        //TODO: padronizar controller
        //TODO: paging
        //TODO: DTOs
        [HttpGet("ano/{year:int}")]
        public async Task<IActionResult> GetByDate(int year)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByYear(year);
            return Ok(response);
        }

        [HttpGet("indice/{index:int}")]
        public async Task<IActionResult> GetByRainfallIndex(int index)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByRainfallIndex(index);
            return Ok(response);
        }

        [HttpGet("distancia")]
        public async Task<IActionResult> FilterByDistance([FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistance(distance);
            return Ok(response);
        }

        [HttpGet("distancia/indice")]
        public async Task<IActionResult> FilterByDistanceAndRainfallIndex([FromQuery] double distance, [FromQuery] double index)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndRainfallIndex(distance, index);
            return Ok(response);
        }

        [HttpGet("distancia/data")]
        public async Task<IActionResult> FilterByDistanceAndDate([FromQuery] double distance, [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndDate(distance, year, month, day);
            return Ok(response);
        }

        [HttpGet("distancia/data-intervalo")]
        public async Task<IActionResult> FilterByDistanceAndDateRange([FromQuery] double distance, [FromQuery] DateTime firstDate, [FromQuery] DateTime secondDate)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndDateRange(firstDate, secondDate, distance);
            return Ok(response);
        }

        [HttpGet("distancia/cidade")]
        public async Task<IActionResult> FilterByDistanceAndCity([FromQuery] double distance, [FromQuery] string city, [FromQuery] int limit)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndCity(distance, city, limit);
            return Ok(response);
        }

        [HttpGet("media/cidade")]
        public async Task<IActionResult> GetAverageRainfallIndexByCity([FromQuery] string city, [FromQuery] int limit)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetAverageRainfallIndexByCity(city, limit);
            return Ok(response);
        }








        [HttpGet("{greaterThanYear:int}/{lessThanYear:int}/{distance:double}")]
        public async Task<IActionResult> GetByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndYearRange(greaterThanYear, lessThanYear, distance);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDistanceAndYear([FromQuery] int year, [FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistanceAndYear(year, distance);
            return Ok(response);
        }

        [HttpGet("valormedida/ano")]
        public async Task<IActionResult> GetSumValueGroupByDate([FromQuery] int year)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityFilterByDate(year);
            return Ok(response);
        }

        [HttpGet("valormedida/distancia/ano")]
        public async Task<IActionResult> GetSumValueGroupByDateAndDistance([FromQuery] int year, [FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityFilterByYearAndDistance(year, distance);
            return Ok(response);
        }

        //TODO: Adicionar mês como parâmetro
        [HttpGet("valormedida/estacao/distancia/ano")]
        public async Task<IActionResult> GetSumValueGroupByStation([FromQuery] int year, [FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetAverageMeasureByCityAndStationFilterByDateAndDistance(year, distance, 7);
            return Ok(response);
        }

        [HttpGet("todos")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetAll();
            return Ok(response);
        }


        [HttpGet("todos/distancia")]
        public async Task<IActionResult> GetAllWithDistance()
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetAllWithDistance();
            return Ok(response);
        }

        [HttpGet("municipio/ano/soma")]
        public async Task<IActionResult> GetValueByYearAndCity()
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityAndYear();
            return Ok(response);
        }

        [HttpGet("distance/estacao")]
        public async Task<IActionResult> GetValueByDistanceAndStation([FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityAndYearFilterByDistance(distance);
            return Ok(response);
        }

        [HttpGet("distancia/agrupado-por-cidade")]
        public async Task<IActionResult> GetValueByDistance([FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityAndDateFilterByDistance(distance);
            return Ok(response);
        }
    }
}
