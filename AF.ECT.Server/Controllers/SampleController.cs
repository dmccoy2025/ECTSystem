using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AF.ECT.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private readonly IAntiforgery _antiforgery;

    public SampleController(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery;
    }

    [HttpGet("token")]
    public IActionResult GetToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PostData([FromBody] SampleData data)
    {
        // Process the data
        return Ok(new { Message = "Data received securely", Data = data });
    }

    [HttpGet]
    public IActionResult GetData()
    {
        // This endpoint doesn't need antiforgery since it's GET
        return Ok(new { Message = "Public data" });
    }
}

public class SampleData
{
    public string? Name { get; set; }
    public int Value { get; set; }
}