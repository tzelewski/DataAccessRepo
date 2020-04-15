namespace DataAccessRepo
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    public class DataAccessRepository : IDbRepository
    {
        public Func<IDbConnection> GetDbConnection { get; }

        public DataAccessRepository(Func<IDbConnection> getDbConnection)
        {
            GetDbConnection = getDbConnection;
        }

        public async Task<IList<T>> ExecuteQuery<T>(Func<DbDataReader, T> getResult, string query,
            params (string parameterName, string value)[] parameters)
        {
            var resultList = new List<T>();
            var connection = GetDbConnection();

            await using var command = (DbCommand)connection.CreateCommand();

            command.CommandText = query;

            foreach (var (parameterName, value) in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = value;
                command.Parameters.Add(parameter);
            }

            connection.Open();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                resultList.Add(getResult(reader));
            }

            return resultList;
        }
    }

    public interface IDbRepository
    {
        Task<IList<T>> ExecuteQuery<T>(Func<DbDataReader, T> getResult, string query,
            params (string parameterName, string value)[] parameters)
    }
}
