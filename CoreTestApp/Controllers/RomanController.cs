using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreTestApp.Business.IServices;

namespace CoreTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RomanController : ControllerBase
    {
        private readonly ILogger<RomanController> _logger;
        private readonly IConvertToRoman _ConvertToRoman;
        public RomanController(ILogger<RomanController> logger, IConvertToRoman toRoman)
        {
            this._logger = logger;
            this._ConvertToRoman = toRoman;
        }



        [HttpGet(Name = "GetRomanNumaric")]
        public IActionResult GetRomanNumaric(int numToConvert)
        {
            if(this._ConvertToRoman.isValid(numToConvert))
            {
                string RomanNumber = this._ConvertToRoman.ToRoman(numToConvert);

                return Ok(RomanNumber);
            }
            else
            {
                return BadRequest("Input number is not in valid range");
            }
           
        }
    }
}
