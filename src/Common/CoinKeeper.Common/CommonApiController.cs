using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Common;

/// <summary>
/// Базовый контроллер api
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommonApiController : ControllerBase
{
}
