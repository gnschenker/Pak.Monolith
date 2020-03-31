using Dapper.Contrib.Extensions;
using System;
using System.Data.SqlClient;

namespace EventSourcing.Library.Infrastructure
{
    public class SqlServerProjectionWriter : IProjectionWriter
    {
        private readonly string connectionString;

        public SqlServerProjectionWriter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add<T>(T view) where T : class
        {
            using var conn = new SqlConnection(connectionString);
            conn.Insert(view);
        }

        public void Update<T>(Guid id, Action<T> updateAction) where T : class
        {
            using var conn = new SqlConnection(connectionString);
            var view = conn.Get<T>(id);
            updateAction(view);
            conn.Update(view);
        }
    }
}