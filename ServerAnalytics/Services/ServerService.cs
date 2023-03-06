using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;

namespace ServerAnalytics.Services
{
    public class ServerService : IServerService
    {
        ServerAnalyticsContext db;

        public List<Server> GetAll()
        {
            List<Server> serverList;
            using(db = new ServerAnalyticsContext()) 
            { 
                serverList = db.Servers.ToList();
            }

            return serverList;
        }

        public Server Get(int id)
        {
            Server server;
            using(db = new ServerAnalyticsContext())
            {
                server = db.Servers.FirstOrDefault(p => p.Id == id);
            }

            return server;
        }

        public void Add(Server server)
        {
            using(db = new ServerAnalyticsContext())
            {
                db.Servers.Add(server);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using(db = new ServerAnalyticsContext())
            {
                var server = db.Servers.FirstOrDefault(s => s.Id == id);

                db.Remove(server);
                db.SaveChanges();
            }
        }

        public void Update(Server server)
        {
            var serverUpdate = db.Servers.FirstOrDefault(p => p.Id == server.Id);

            serverUpdate.Ip = server.Ip;
            serverUpdate.Name = server.Name;
            db.Servers.Add(serverUpdate);
        }
    }
}
