using System.Collections.Generic;
using LocationTestTask.DataLayer.DataEntities;
using LocationTestTask.DataLayer.Dto;

namespace LocationTestTask.DataLayer.Repositories
{
    public interface ILocationRepository
    {
        IEnumerable<LocationDto> GetAll();
        LocationDto GetItem(long id);
        void InsertOrUpdate(LocationDto dto);
        IEnumerable<LocationDto> ConvertEntityListToDtoList(IEnumerable<Location> entities);
        void Insert(LocationDto dto);
    }
}