using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IServerService
    {
        List<Server> GetAll();
        Server Get(int id);
        void Add(Server server);
        void Delete(int id);
        void Update(Server server);

    }
}
