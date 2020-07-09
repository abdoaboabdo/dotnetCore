using Microsoft.AspNetCore.Identity;

namespace Vega.Core.Models
{
    public class User : IdentityUser
    {
        public override string Id { get; set; }
    }
}