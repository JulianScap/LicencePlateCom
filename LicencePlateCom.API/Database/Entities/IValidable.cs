using System.Collections.Generic;

namespace LicencePlateCom.API.Database.Entities
{
    public interface IValidable
    {
        bool Validate(out IEnumerable<string> messages);
    }
}