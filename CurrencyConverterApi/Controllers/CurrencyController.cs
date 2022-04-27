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
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CurrencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllCurrencies")]
        public async Task<ActionResult> GetAllCurrencies()
        {
            try
            {
                return Ok(await _unitOfWork.Currencies.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpGet("GetByName")]
        public async Task<ActionResult> GetCurrencyByName(string name)
        {
            try
            {
                var result =await _unitOfWork.Currencies.FindAsync(b => b.CurrencyName.ToLower() == name.ToLower(), new[] { "ExchangeHistory" });
                if (result == null)
                {
                    return NotFound($"No Currency was found with Name: {name}");
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetCurrencyById(int id)
        {
            try
            {
                var result = await _unitOfWork.Currencies.GetById(id);
                if (result == null)
                {
                    return NotFound($"No Currency was found with ID: {id}");
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
        public async Task<ActionResult> CreateCurrency(CurrencyDto currencyDto)
        {
            try
            {
                if (currencyDto == null)
                {
                    return BadRequest();
                }
                var curr = await _unitOfWork.Currencies.FindAsync(b=>b.CurrencyName.ToLower()==currencyDto.CurrencyName.ToLower()
                                                                                   , new[] {"ExchangeHistory"});
                if (curr != null)
                {
                    ModelState.AddModelError("CurrencyName", "Currency name already exists");
                    return BadRequest(ModelState);
                }
                Currency currency = new Currency()
                {
                    CurrencyName = currencyDto.CurrencyName,
                    CurrencySign = currencyDto.CurrencySign,
                    IsActive = currencyDto.IsActive
                };
                var createdCurrency = await _unitOfWork.Currencies.AddAsync(currency);
                 _unitOfWork.Complete();
                return CreatedAtAction(nameof(GetCurrencyById), new { id = createdCurrency.Id }, createdCurrency);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Currency record");
            }

        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCurrency(int id, [FromBody] CurrencyDto currencyDto)
        {
            try
            {
                var currency = await _unitOfWork.Currencies.GetById(id);
                if (currency == null)
                {
                    return NotFound($"No Currency was found with ID: {id}");
                }
                currency.CurrencyName = currencyDto.CurrencyName;
                currency.CurrencySign = currencyDto.CurrencySign;
                currency.IsActive = currencyDto.IsActive;
                 _unitOfWork.Currencies.Update(currency);
                _unitOfWork.Complete();
                return Ok(currency);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }

        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCurrency(int id)
        {
            try
            {
                var currency = await _unitOfWork.Currencies.GetById(id);
                if (currency == null)
                {
                    return NotFound($"No Currency was found with ID: {id}");
                }
                if (currency.IsActive)
                {
                    currency.CurrencyName = currency.CurrencyName;
                    currency.CurrencySign= currency.CurrencySign;
                    currency.IsActive = false;
                    _unitOfWork.Currencies.Update(currency);
                    _unitOfWork.Complete();
                    return Ok(currency);
                }
                _unitOfWork.Currencies.Delete(currency);
                _unitOfWork.Complete();
                return Ok(currency);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Error deleting data");

            }
        }
        [HttpGet("ConvertFromCurrencyToAnotherCurrency")]
        public async Task<ActionResult> ConvertFromCurrencyToAnother(int Amount, string FromCurrencyName, string ToCurrencyName)
        {
            try
            {
                if (Amount <= 0)
                {
                    return BadRequest("Amount must be Greater than Zero");
                }
                var fromCurrency = await _unitOfWork.Currencies.FindAsync(b=>b.CurrencyName.ToLower() == FromCurrencyName.ToLower()
                                                           , new[] {"ExchangeHistory"});
                var toCurrency = await _unitOfWork.Currencies.FindAsync(b => b.CurrencyName.ToLower() == ToCurrencyName.ToLower()
                                                           , new[] { "ExchangeHistory" });
                if (fromCurrency == null || fromCurrency.IsActive != true)
                {
                    return NotFound($"No Currency was found with Name: {FromCurrencyName}");
                }
                if (toCurrency == null || toCurrency.IsActive != true)
                {
                    return NotFound($"No Currency was found with Name: {ToCurrencyName}");
                }
                var result = (Amount * fromCurrency.ExchangeHistory.
                              LastOrDefault(b => b.CurrencyId == fromCurrency.Id).Rate) /
                              toCurrency.ExchangeHistory.LastOrDefault(b => b.CurrencyId == toCurrency.Id).Rate;

                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
