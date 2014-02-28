using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using LocationTestTask.Core;
using LocationTestTask.DataLayer.Context;
using LocationTestTask.DataLayer.Repositories;
using Microsoft.Phone.Controls.Maps;
using Phone7.Fx;
using Phone7.Fx.Ioc;

namespace LocationTestTask.UI
{
    public class BootStrapper : PhoneBootstrapper
    {
        private const string ConnectionString = "Data Source=isostore:/db.sdf";

        protected override void Configure()
        {
            var db = new LocationContext(ConnectionString);
            var context = CreateDataBaseIfNeeded(db);

            var locationRepository = new LocationRepository(context);
            var locationManager = new LocationManager(locationRepository);
            var bingMapProvider = new ApplicationIdCredentialsProvider("AuxNBnpoOmnhUmUoL2a8xcc5z6B1eK_58NBtUEUaIuHkuKpHQDmVRxaB7lw_uuye");
            Container.Current.RegisterInstance(bingMapProvider);
            Container.Current.RegisterInstance<ILocationContext>(context);
            Container.Current.RegisterInstance<ILocationRepository>(locationRepository);
            Container.Current.RegisterInstance<ILocationManager>(locationManager);
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
