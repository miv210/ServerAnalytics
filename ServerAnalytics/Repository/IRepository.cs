namespace ServerAnalytics.Repository
{
    public interface IRepository <T>  where T : class
    {
        IEnumerable<T> GetAll ();
        T Get(int id);
        void Cretae(T item);
        void Update(T item);
        void Delete(int id);
    }
}
