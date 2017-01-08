namespace Gira.Business
{
    public interface IMapper<T, TU>
    {
        T Map(TU originalObject);

        TU Map(T originalObject),
    }
}
