using Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
    }
}