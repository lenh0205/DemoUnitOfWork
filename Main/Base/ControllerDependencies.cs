using MediatR;

namespace Main.Base
{
    public interface IControllerDependencies<T>
    {
        public ILogger<T> Logger { get; }
        public IMediator Mediator { get; }
    }

    public class ControllerDependencies<T> : IControllerDependencies<T> where T : class
    {
        public ILogger<T> Logger { get; }
        public IMediator Mediator { get; }

        public ControllerDependencies(ILogger<T> logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }
    }
}
