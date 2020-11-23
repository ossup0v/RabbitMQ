using RabbitMQ.Connections.Models;
using System.Threading.Tasks;

namespace RabbitMQ.Connections
{
  public interface IServerDatabase
  {
    /// <returns>uId of new object in DB</returns>
    async Task CreateTest(TestDBModel model);
    void UpdateTest(int uId, TestDBModel model);
    TestDBModel ReadTest(int uId);
    void DeleteTest(int uId);
  }
}
