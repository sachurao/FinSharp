namespace StreamCipher.Common.Ioc
{
    public interface IDependencyManager
    {
        IDependencyManager Register<T1, T2>();
        T Resolve<T>();
    }
}
