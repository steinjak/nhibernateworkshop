using NHibernate;

namespace NHibernateWorkshop.Data
{
    public class EntityStore
    {
        private readonly ISession session;

        public EntityStore(ISession session)
        {
            this.session = session;
        }

        public T Load<T>(object id)
        {
            return session.Load<T>(id);
        }

        public T Query<T>(Query<T> query)
        {
            return query.Execute(session);
        }

        public void Save(object entity)
        {
            session.Save(entity);
        }

        public void Delete(object entity)
        {
            session.Delete(entity);
        }

        public void Sync()
        {
            session.Flush();
        }
    }
}