# DataAccessRepo
Simple utility class to manage SQL connection and execution of SQL queries

It provides an easy way to abstract out the creation, usage and maintenance of IDbConnection, DbCommand and DbDataReader. Easy to use, inject and unit test.

Example usage:

        private async Task TestDataAccessRepository()
        {
            var dataAccessRepository = new DataAccessRepository(() => new SqlConnection("connectionString")); // this should be handled by IoC
            var result = await dataAccessRepository.ExecuteQuery(reader =>
            {
                var name = (string) reader["name"];
                var address = (string) reader["address"];

                return (name, address);

            }, "select name, address from person where lastName like @lastName", ("@lastName", "Smith");
        }
