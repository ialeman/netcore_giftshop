using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace SS
{
    /// <summary>
    /// Handles the creation of <see cref="HttpClient" /> instances in order to maintain &quot;singletons&quot; per
    /// service, through the use of a key.
    /// </summary>
    /// <summary>
    /// The purpose of this service is to avoid memory leaks cause by per-request instantiation of <see cref="HttpClient" />
    /// instances. For more information see <see cref="http://stackoverflow.com/a/15708633/4019761" /> or
    /// <see cref="http://chimera.labs.oreilly.com/books/1234000001708/ch14.html#although_httpcl" />.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Gets the default <see cref="HttpClient" /> instance.
        /// </summary>
        /// <returns></returns>
        HttpClient Get();

        /// <summary>
        /// Gets an instance of <see cref="HttpClient" /> that is unique per key, optionally using the provided factory
        /// only the first time the method is invoked with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="factory">The custom <see cref="HttpClient" /> factory (optional).</param>
        /// <returns></returns>
        HttpClient Get(object key, Func<HttpClient> factory = null);
    }

    /// <inheritdoc cref="SS.Net.IHttpClientFactory"/>
    public sealed class HttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly Dictionary<object, HttpClient> _cache = new Dictionary<object, HttpClient>();
        private readonly Lazy<HttpClient> _defaultLazy = new Lazy<HttpClient>(CreateDefault);

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private int _disposed;

        private const int DisposedValue = 1;

        public bool IsDisposed
        {
            get { return _disposed == DisposedValue; }
        }

        public HttpClient Get()
        {
            return Get(null, null);
        }

        public HttpClient Get(object key, Func<HttpClient> factory = null)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(HttpClientFactory));
            }

            if (key == null)
            {
                return _defaultLazy.Value;
            }

            _lock.EnterUpgradeableReadLock();
            try
            {
                if (!_cache.TryGetValue(key, out var client))
                {
                    return Add(key, factory ?? CreateDefault);
                }

                return client;
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        private HttpClient Add(object key, Func<HttpClient> factory)
        {
            _lock.EnterWriteLock();
            try
            {
                var client = factory();
                _cache.Add(key, client);
                return client;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private static HttpClient CreateDefault()
        {
            return new HttpClient();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, DisposedValue) != DisposedValue)
            {
                _lock.Dispose();
                var copy = new Dictionary<object, HttpClient>(_cache);
                _cache.Clear();
                foreach (var httpClient in copy)
                {
                    httpClient.Value.Dispose();
                }
            }
        }
    }
}
