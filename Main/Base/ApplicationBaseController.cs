using Application.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Main.Base
{
    public abstract class ApplicationBaseController<T> : ControllerBase where T : class
    {
        protected readonly ILogger<T> _logger;
        protected readonly IMediator _mediator;

        public ApplicationBaseController(IControllerDependencies<T> dependencies, IMediator mediator)
        {
            _logger = dependencies.Logger;
            _mediator = mediator;
        }
    }
}
