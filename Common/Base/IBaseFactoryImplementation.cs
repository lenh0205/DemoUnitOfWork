namespace Common.Base
{
    public interface IBaseFactoryImplementation
    {
        T GetInstance<T>();
    }
}
