using ServerAnalytics.Models;

namespace ServerAnalytics.Repository
{
    public class UnitOfWork : IDisposable
    {
        private ServerAnalyticsContext db = new ServerAnalyticsContext();


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
