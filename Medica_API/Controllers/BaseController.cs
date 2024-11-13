using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Medica_API.Controllers
{
    [ApiController]

    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        private ILogger<BaseController>? _logger;
        private IConfiguration? _config;

        protected IConfiguration Configuration => _config ?? (_config = HttpContext.RequestServices.GetService<IConfiguration>());

        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
        protected ILogger<BaseController> Logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<BaseController>>());

       

    }
}
