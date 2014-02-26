using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using LocationTestTask.DataLayer.Context;
using LocationTestTask.DataLayer.Repositories;
using Phone7.Fx;
using Phone7.Fx.Ioc;

namespace LocationTestTask.UI
{
    public class BootStrapper : PhoneBootstrapper
    {
        private readonly string _connectionString;

        public BootStrapper()
        {
           
            _connectionString = "Data Source=isostore:/db.sdf";
       
        }

        protected override void Configure()
        {
            var db = new LocationContext(_connectionString);
            var context = CreateDataBaseIfNeeded(db);
            Container.Current.RegisterInstance<ILocationContext>(context);
            Container.Current.RegisterType<ILocationRepository, LocationRepository>();
        }

        private ILocationContext CreateDataBaseIfNeeded(ILocationContext db)
        {
            if (db.DatabaseExists() == false)
            {
                db.CreateDatabase();
            }
            return db;
        }

    }
}
