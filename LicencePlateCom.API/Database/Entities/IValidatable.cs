using System.Collections.Generic;

namespace LicencePlateCom.API.Database.Entities
{
    public interface IValidatable
    {
        bool Validate(out List<string> messages);
    }
}