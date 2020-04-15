namespace DataAccessRepo
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IDbRepository
    {
        Task<IList<T>> ExecuteQuery<T>(Func<DbDataReader, T> getResult, string query,
            params (string parameterName, string value)[] parameters);
    }
}