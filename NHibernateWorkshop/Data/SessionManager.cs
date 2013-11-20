using System;
using System.Data;
using NHibernate;

namespace NHibernateWorkshop.Data
{
    public class SessionManager : IDisposable
    {
        private readonly ISessionProvider sessionProvider;
        protected Lazy<ISession> Session { get; private set; }

        public SessionManager(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
            Session = CreateSession();
        }

        private Lazy<ISession> CreateSession()
        {
            return new Lazy<ISession>(() =>
            {
                var result = sessionProvider.Create();
                result.BeginTransaction(IsolationLevel.ReadCommitted);
                return result;
            });
        }

        public EntityStore GetEntityStore()
        {
            return new EntityStore(Session.Value);
        }

        public virtual void Commit()
        {
            if (!Session.IsValueCreated) { return; }

            try
            {
                Session.Value.Transaction.Commit();
            }
            catch (Exception)
            {
                Session.Value.Transaction.Rollback();
                throw;
            }
            finally
            {
                Session.Value.Dispose();
                Session = CreateSession();
            }
        }

        public virtual void Rollback()
        {
            if (!Session.IsValueCreated || !Session.Value.Transaction.IsActive) { return; }

            try
            {
                Session.Value.Transaction.Rollback();
            }
            finally
            {
                Session.Value.Dispose();
                Session = CreateSession();
            }
        }

        public void Dispose()
        {
            Commit();
        }
    }
}