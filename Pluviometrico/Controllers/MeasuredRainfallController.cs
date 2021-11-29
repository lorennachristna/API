﻿using Microsoft.AspNetCore.Mvc;
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

        //Retorna apenas os 10 primeiros?
        //TODO: Retornar status de erro quando der erro
        //TODO: padronizar controller
        //TODO: paging
        //TODO: DTOs
        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> GetByDate(int month, int year)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByMonthAndYear(month, year);
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
            var response = await _unitOfWork.MeasuredRainfallList.GetAverageMeasureByCityAndStationFilterByDateAndDistance(year, distance,7);
            return Ok(response);
        }

        [HttpGet("todos")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetAll();
            return Ok(response);
        }

        [HttpGet("filtro/distancia")]
        public async Task<IActionResult> FilterByDistance([FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.FilterByDistance(distance);
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

        [HttpGet("distancia")]
        public async Task<IActionResult> GetValueByDistance([FromQuery] double distance)
        {
            var response = await _unitOfWork.MeasuredRainfallList.GetMeasureByCityAndDateFilterByDistance(distance);
            return Ok(response);
        }
    }
}
