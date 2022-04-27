using Core;
using Core.Domain;
using Core.DTOs;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeHistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExchangeHistoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetExchangeHistoryById(int id)
        {
            try
            {
                var result = await _unitOfWork.ExchangeHistoryService.GetById(id);
                if (result == null)
                {
                    return NotFound($"No Exchange History was found with ID: {id}");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateExchangeHistory(ExchangeHistoryDto exchangeHistoryDto)
        {
            try
            {
                if (exchangeHistoryDto == null)
                {
                    return BadRequest();
                }

                ExchangeHistory exchangeHistory = new ExchangeHistory()
                {
                    ExchangeDate = exchangeHistoryDto.ExchangeDate,
                    Rate = exchangeHistoryDto.Rate,
                    CurrencyId = exchangeHistoryDto.CurrencyId
                };
                var createdExchangeHistory = await _unitOfWork.ExchangeHistoryService.AddAsync(exchangeHistory);
                _unitOfWork.Complete();
                return CreatedAtAction(nameof(GetExchangeHistoryById), new { id = exchangeHistory.Id }, createdExchangeHistory);
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Exchange History record");
            }

        }
        [HttpGet("GetHighestNCurrencies")]
        public async Task<ActionResult> HighestNCurrencies(int Number)
        {
            try
            {
                if (Number <= 0)
                {
                    return BadRequest("Number must be Greater than Zero");
                }
                var ListHighestCurrencies = await _unitOfWork.ExchangeHistoryService.GetHighestNumberCurrencies();
                return Ok(ListHighestCurrencies.Take(Number).Select(b => b.Currency.CurrencyName));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpGet("GetLowestNCurrencies")]
        public async Task<ActionResult> LowestNCurrencies(int Number)
        {
            try
            {
                if (Number <= 0)
                {
                    return BadRequest("Number must be Greater than Zero");
                }
                var ListHighestCurrencies = await _unitOfWork.ExchangeHistoryService.GetLowestNumberCurrencies();
                return Ok(ListHighestCurrencies.Take(Number).Select(b => b.Currency.CurrencyName));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpGet("GetMostNCurrenciesImproved")]
        public async Task<ActionResult> GetMostNIpmproved(int Number, DateTime fromDateTime, DateTime toDateTime)
        {
            try
            {
                if (Number <= 0)
                {
                    return BadRequest("Number must be Greater than Zero");
                }
                if (fromDateTime > toDateTime || fromDateTime > DateTime.Now)
                {
                    return BadRequest("From Date must be less than To Date And Less than Date Now");
                }
                var FDT = await _unitOfWork.ExchangeHistoryService.GetByDate(fromDateTime);
                if (FDT == null || FDT.Count() == 0 )
                {
                    return NotFound($"Not found any Exchange History with Date: {fromDateTime}");
                }
                var TDT = await _unitOfWork.ExchangeHistoryService.GetByDate(toDateTime);
                if (TDT == null || TDT.Count() == 0)
                {
                    return NotFound($"Not found any Exchange History with Date: {toDateTime}");
                }
                List<CalculateImprovedDto> Imp = new List<CalculateImprovedDto>();
                foreach (var f in FDT)
                {
                    foreach (var t in TDT)
                    {
                        if (f.CurrencyId == t.CurrencyId)
                        {
                            var result = t.Rate - f.Rate;
                            if (result > 0)
                            {
                                Imp.Add(new CalculateImprovedDto { CurrencyImproved = t.Currency.CurrencyName, improved = result });

                            }
                        }
                    }
                }
                var LsResult = Imp.OrderByDescending(b => b.improved).Take(Number).ToList();
                if (LsResult.Count()==0)
                {
                    return BadRequest("No Currencies Improved");
                }
                return Ok(LsResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpGet("GetLeastNCurrenciesImproved")]
        public async Task<ActionResult> GetLeastNIpmproved(int Number, DateTime fromDateTime, DateTime toDateTime)
        {
            try
            {
                if (Number <= 0)
                {
                    return BadRequest("Number must be Greater than Zero");
                }
                if (fromDateTime > toDateTime || fromDateTime > DateTime.Now)
                {
                    return BadRequest("From Date must be less than To Date And Less than Date Now");
                }
                var FDT = await _unitOfWork.ExchangeHistoryService.GetByDate(fromDateTime);
                if (FDT == null || FDT.Count() == 0)
                {
                    return NotFound($"Not found any Exchange History with Date: {fromDateTime}");
                }
                var TDT = await _unitOfWork.ExchangeHistoryService.GetByDate(toDateTime);
                if (TDT == null || TDT.Count() == 0)
                {
                    return NotFound($"Not found any Exchange History with Date: {toDateTime}");
                }
                List<CalculateImprovedDto> Imp = new List<CalculateImprovedDto>();
                foreach (var f in FDT)
                {
                    foreach (var t in TDT)
                    {
                        if (f.CurrencyId == t.CurrencyId)
                        {
                            var result = t.Rate - f.Rate;
                            if (result > 0)
                            {
                                Imp.Add(new CalculateImprovedDto { CurrencyImproved = t.Currency.CurrencyName, improved = result });

                            }
                        }
                    }
                }
                var LsResult = Imp.OrderBy(b => b.improved).Take(Number).ToList();
                if (LsResult.Count() == 0)
                {
                    return BadRequest("No Currencies Improved");
                }
                return Ok(LsResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
