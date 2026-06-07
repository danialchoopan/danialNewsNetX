namespace danialNewsNetX.WebUI.Controllers;

public class TestController : Microsoft.AspNetCore.Mvc.Controller
{
    [Microsoft.AspNetCore.Mvc.HttpGet("/test-css")]
    public string TestCss()
    {
        return "<html><head><script src='https://cdn.tailwindcss.com'></script></head><body class='bg-red-500'><div class='p-10 bg-blue-500 text-white font-bold rounded-xl'>Tailwind Test</div></body></html>";
    }
}
