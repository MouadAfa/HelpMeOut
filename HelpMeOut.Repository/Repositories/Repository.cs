using System.Data;
using Dapper;

namespace HelpMeOut.Repository.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IDbConnection _connection;
    private readonly string _tableName;

    public Repository(IDbConnection connection, string tableName)
    {
        _connection = connection;
        _tableName = tableName;
    }

    public IEnumerable<T> GetAll()
    {
        return _connection.Query<T>($"SELECT * FROM {_tableName}");
    }

    public T Get(int id)
    {
        return _connection.QueryFirstOrDefault<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
    }

    public void Create(T item)
    {
        // Use reflection to get property names (better than hardcoding)
        var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

        string sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";

        _connection.Execute(sql, item);
    }

    public void Update(T item)
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
        var updates = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        string sql = $"UPDATE {_tableName} SET {updates} WHERE Id = @Id";

        _connection.Execute(sql, item);
    }

    public void Delete(int id)
    {
        _connection.Execute($"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });
    }
}