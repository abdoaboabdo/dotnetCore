using System.Threading.Tasks;

namespace Vega.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}