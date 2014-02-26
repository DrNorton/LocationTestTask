using System.Collections.Generic;
using System.Linq;
using LocationTestTask.DataLayer.Context;
using LocationTestTask.DataLayer.DataEntities;
using LocationTestTask.DataLayer.Dto;

namespace LocationTestTask.DataLayer.Repositories.Base
{
    public abstract class BaseRepository<Entity, Dto>
        where Entity : class,IEntity
        where Dto : class,IDto
    {
        private readonly ILocationContext _locationStore;

        protected BaseRepository(ILocationContext store)
        {
            _locationStore = store;
        }

        public IEnumerable<Dto> GetAll()
        {
            return ConvertColl(_locationStore.GetTable<Entity>().ToList());
        }

        public Dto GetItem(long id)
        {
            Entity item = _locationStore.GetTable<Entity>().FirstOrDefault(x => x.Id.Equals(id));
            return Convert(item);

        }

        public void InsertOrUpdate(Dto dto)
        {

            var updatedOrSavedEntity = _locationStore.GetTable<Entity>().FirstOrDefault(x => x.Id.Equals(dto.Id));
            if (updatedOrSavedEntity != null)
            {
                UpdateEntry(dto, updatedOrSavedEntity);
            }
            else
            {
                updatedOrSavedEntity = CreateEntry(dto);
                _locationStore.GetTable<Entity>().InsertOnSubmit(updatedOrSavedEntity);
            }

            dto.Id = updatedOrSavedEntity.Id;

            _locationStore.SubmitChanges();
        }

        public IEnumerable<Dto> ConvertEntityListToDtoList(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                yield return Convert(entity);
            }
        }

        public abstract Dto Convert(Entity entity);

        public void Insert(Dto dto)
        {
            var insertedEntity = CreateEntry(dto);
            _locationStore.GetTable<Entity>().InsertOnSubmit(insertedEntity);
            _locationStore.SubmitChanges();
        }
       
        protected IEnumerable<Dto> ConvertColl(IEnumerable<Entity> entities)
        {

            foreach (var entity in entities)
            {
                yield return Convert(entity);
            }
        }

        protected abstract Entity UpdateEntry(Dto sourceDto, Entity targetEntity);
        protected abstract Entity CreateEntry(Dto dto);

    }
}
