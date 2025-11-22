using Application.Base;

namespace Main.Base
{
    public interface IControllerDependencies<T>
    {
        public ILogger<T> Logger { get; }
        public IBusinessHandlersFactory BusinessHandlers { get; }
    }

    public class ControllerDependencies<T> : IControllerDependencies<T> where T : class
    {
        public ILogger<T> Logger { get; }
        public IBusinessHandlersFactory BusinessHandlers { get; }

        public ControllerDependencies(ILogger<T> logger, IBusinessHandlersFactory businessServices)
        {
            Logger = logger;
            BusinessHandlers = businessServices;
        }
    }
}
