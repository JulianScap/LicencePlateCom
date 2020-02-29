using System.Collections.Generic;
using LicencePlateCom.API.Database.Entities;

namespace LicencePlateCom.API.Business
{
    public class BaseService<T>
        where T : class, IValidatable
    {
        protected virtual bool Validate(T item, out List<string> messages)
        {
            if (item != default(T))
            {
                return item.Validate(out messages);
            }

            messages = new List<string> {$"No {TypeName} specified (null)."};
            return false;
        }

        protected virtual string TypeName => typeof(T).Name;
    }
}