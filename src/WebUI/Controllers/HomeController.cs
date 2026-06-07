using danialNewsNetX.Application.Feeds.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace danialNewsNetX.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("feed")]
    public async Task<IActionResult> Feed()
    {
        var response = await _mediator.Send(new GetExploreFeedQuery());
        return View(response);
    }
}
