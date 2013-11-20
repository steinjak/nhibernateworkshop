using NHibernate;

namespace NHibernateWorkshop.Data
{
    public abstract class Query<T>
    {
        public abstract T Execute(ISession session);
    }
}