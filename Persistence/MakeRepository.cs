using Vega.Core;
using Vega.Core.Models;

namespace Vega.Persistence
{
    public class MakeRepository : IMakeRepository
    {
        private readonly VegaDbContext context;
        public MakeRepository(VegaDbContext context)
        {
            this.context = context;

        }
        public void edit(Make make)
        {
            context.Makes.Update(make);
        }
    }
}