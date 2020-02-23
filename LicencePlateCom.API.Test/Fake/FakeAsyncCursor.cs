using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LicencePlateCom.API.Test.Fake
{
    public static class FakeAsyncCursor
    {
        public static FakeAsyncCursor<T, TS> CreateInstance<T, TS>(params T[] items)
            where T: IEnumerable<TS>
        {
            return FakeAsyncCursor<T, TS>.CreateInstance(items);
        }
        
        public static FakeAsyncCursor<IEnumerable<TS>, TS> CreateInstance<TS>(params TS[] items)
        {
            return FakeAsyncCursor<IEnumerable<TS>, TS>.CreateInstance(items);
        }
    }
    
    public class FakeAsyncCursor<T, TS> : IAsyncCursor<TS>
        where T: IEnumerable<TS>
    {
        internal static FakeAsyncCursor<T, TS> CreateInstance(params T[] items)
        {
            return new FakeAsyncCursor<T, TS>(items);
        }

        private IEnumerator _enumerator;

        private FakeAsyncCursor(params T[] items)
        {
            _enumerator = items.GetEnumerator();
        }
        
        public void Dispose()
        {
            _enumerator = null;
        }

        public bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
        {
            return _enumerator.MoveNext();
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.Run(_enumerator.MoveNext, cancellationToken);
        }

        public IEnumerable<TS> Current => (IEnumerable<TS>)_enumerator.Current;
    }
}