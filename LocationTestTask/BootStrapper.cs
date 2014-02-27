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
        private const string ConnectionString = "Data Source=isostore:/db.sdf";

        public BootStrapper()
        {
           
        }
        
        protected override void Configure()
        {
            var db = new LocationContext(ConnectionString);
            var context = CreateDataBaseIfNeeded(db);
            var locationRepository = new LocationRepository(context);
            Container.Current.RegisterInstance<ILocationContext>(context);
            Container.Current.RegisterInstance<ILocationRepository>(locationRepository);
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
