namespace Common.Base
{
    public abstract class BaseFactoryImplementation<TDenpendencies> : IBaseFactoryImplementation
    {
        private readonly Dictionary<Type, object> _cacheRepositoryInstances = new();
        private readonly TDenpendencies _instanceDependencies;

        public BaseFactoryImplementation(TDenpendencies instanceDependencies)
        {
            _instanceDependencies = instanceDependencies;
        }

        public T GetInstance<T>() 
        {
            var type = typeof(T);
            if (_cacheRepositoryInstances.TryGetValue(type, out var service)) return (T)service;

            var newInstance = CreateInstance<T>();
            _cacheRepositoryInstances[type] = newInstance;
            return newInstance;
        }

        private T CreateInstance<T>()
        {
            var type = typeof(T);
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                // check constructor has 1 parameter with "IServiceProvider" type
                var parameters = constructor.GetParameters();
                if (parameters.Length != 1) continue;
                var param = parameters[0];
                if (param.ParameterType != typeof(TDenpendencies)) continue;

                var arguments = new object[1] { _instanceDependencies! };
                return (T)constructor.Invoke(arguments);
            }
            throw new InvalidOperationException($"Unable to create an instance of {type}. No suitable constructor found.");
        }
    }
}
