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
        protected readonly IMediator Mediator;
        protected readonly ILogger Logger;
        protected readonly IConfiguration? Config;

       
        public BaseController(IMediator _Mediator, ILogger _Logger, IConfiguration _Config)
        {
            Mediator = _Mediator;
            Logger = _Logger;
            Config = _Config;
        }
       
              

    }
}
