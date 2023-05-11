using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EchoController : ControllerBase
{
    [HttpGet("{message}")]
    public ActionResult<string> Index(string message)
    {
        return "Hello111, " + message;
    }

    [HttpGet("jba/{message}")]
    public ActionResult<string> Jba(string message)
    {
        double d = 123.456789;
string result = d.ToString("0.000"); // 123.457
        return result;
        return string.Join("...", message.ToCharArray());
    }
}
