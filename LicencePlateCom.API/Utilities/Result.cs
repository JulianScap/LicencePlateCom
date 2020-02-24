using System.Collections.Generic;

namespace LicencePlateCom.API.Utilities
{
    public class Result
    {
        private bool _success;

        public Result(bool success = true)
        {
            _success = success;
        }

        public virtual bool Success
        {
            get => _success;
            set => _success = value;
        }
        
        public virtual bool HasItem => false;
        public IEnumerable<string> Messages { get; set; }

        public static explicit operator Result(bool b) => new Result(b);
        public static explicit operator bool(Result b) => b?.Success ?? false;
    }

    public class Result<T> : Result
        where T : class
    {
        private T _item;

        public Result(bool success):base(success) { }
        public Result(T item): this(true)
        {
            _item = item;
        }

        public override bool HasItem => Item != default(T);
        public T Item { get => _item; set => _item = value; }
    }

    public static class Return
    {
        public static Result<T> Failed<T>()
            where T : class
        {
            return new Result<T>(false);
        }

        public static Result<T> Success<T>(T item)
            where T : class
        {
            return new Result<T>(item);
        }

        public static Result Success(bool success)
        {
            return new Result(success);
        }
    }
}