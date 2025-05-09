using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Common;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommonApiController : ControllerBase
{
}
