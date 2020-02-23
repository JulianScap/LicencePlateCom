using System.Collections.Generic;

namespace LicencePlateCom.API.Database.Entities
{
    public interface IValidatable
    {
        bool Validate(out IEnumerable<string> messages);
    }
}