namespace HelpMeOut.Repository.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T Get(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
}