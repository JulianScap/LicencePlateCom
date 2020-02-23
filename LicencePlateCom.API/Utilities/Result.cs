using System.Collections.Generic;

namespace LicencePlateCom.API.Utilities
{
    public class Result
    {
        public Result(bool success)
        {
            Success = success;
        }

        public virtual bool Success { get; set; }
        public virtual bool HasItem => false;
        public IEnumerable<string> Messages { get; set; }

        public static explicit operator Result(bool b) => new Result(b);
        
        public static explicit operator bool(Result b) => b?.Success ?? false;
    }

    public class Result<T> : Result
        where T : class
    {
        public Result(bool success):base(success) { }

        public override bool HasItem => Item != default(T);
        public T Item { get; set; }
    }
}