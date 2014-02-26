using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LocationTestTask.DataLayer.Context
{
    public interface ILocationContext
    {
        bool CreateIfNotExists();
        ITable GetTable(Type type);
        Table<TEntity> GetTable<TEntity>() where TEntity : class;
        bool DatabaseExists();
        void CreateDatabase();
        void SubmitChanges();
        void SubmitChanges(ConflictMode failureMode);
    }
}