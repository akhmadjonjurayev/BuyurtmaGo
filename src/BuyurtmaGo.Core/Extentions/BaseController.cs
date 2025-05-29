using BuyurtmaGo.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuyurtmaGo.Core.Extentions
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Result<T, E>(OperationResult<T, E> data) where E : Enum
        {
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            else
            {
                return BadRequest(data.Error);
            }
        }
    }
}
