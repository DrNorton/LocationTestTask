using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocationTestTask.DataLayer.Context;
using LocationTestTask.DataLayer.DataEntities;
using LocationTestTask.DataLayer.Dto;
using LocationTestTask.DataLayer.Repositories.Base;

namespace LocationTestTask.DataLayer.Repositories
{
    public class LocationRepository:BaseRepository<Location,LocationDto>, ILocationRepository
    {
        public LocationRepository(ILocationContext store)
            :base(store)
        {
            
        }

        public override LocationDto Convert(Location entity)
        {
            return new LocationDto(){Id=entity.Id,
                MeasurementDatetime = entity.Datetime,
                MapPosition = new MapPositionDto(){Id = entity.MapPosition.Id,Latitude = entity.MapPosition.Latitude,Longitude = entity.MapPosition.Longitude}};
        }

        protected override Location UpdateEntry(LocationDto sourceDto, Location targetEntity)
        {
            targetEntity.Id = sourceDto.Id;
            targetEntity.Datetime = sourceDto.MeasurementDatetime;
            targetEntity.MapPosition=new MapPosition();
            targetEntity.MapPosition.Id = sourceDto.MapPosition.Id;
            targetEntity.MapPosition.Latitude = sourceDto.MapPosition.Latitude;
            targetEntity.MapPosition.Longitude = sourceDto.MapPosition.Longitude;
            return targetEntity;
        }

        protected override Location CreateEntry(LocationDto dto)
        {
            return UpdateEntry(dto, new Location());
        }
    }
}
