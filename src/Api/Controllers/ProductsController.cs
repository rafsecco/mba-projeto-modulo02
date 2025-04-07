using Api.Extensions;
using Application.App.Products.Commands;
using Application.App.Products.Queries;
using Application.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private IHostEnvironment _hostingEnvironment;

    public ProductsController(IMediator mediator, IHostEnvironment hostingEnvironment)
    {
        _mediator = mediator;
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = User.GetUserId();
        command.SellerId = id;

        return await _mediator.Send(command, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveProductCommand { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetProductsQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> Find(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetProductQuery { Id = id }, cancellationToken);
    }

    [HttpGet("{categoryId}/categoryId")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> GetByCategoryId(int categoryId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetProductsByCategory { CategoryId = categoryId }, cancellationToken);
    }
}