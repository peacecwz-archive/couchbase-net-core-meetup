using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace CouchbaseNetDemo.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IBucket _bucket;

        public BookRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("Books", "12345678");
        }

        public async Task<BookEntity> GetById(string id)
        {
            var result = await _bucket.GetAsync<BookEntity>(id);
            if (result.Success)
            {
                return result.Value;
            }

            return null;
        }

        public async Task<bool> IsExists(string id)
        {
            return await _bucket.ExistsAsync(id);
        }

        public async Task<bool> Add(BookEntity book)
        {
            var result = await _bucket.InsertAsync(book.Id, book);

            return result.Success;
        }

        public async Task<bool> Upsert(string id, BookEntity book)
        {
            book.Id = id;
            var result = await _bucket.UpsertAsync(id, book);
            return result.Success;
        }
        
        public async Task<bool> Patch(string id, JsonPatchDocument request)
        {
            //TODO: Refactor
            var mutateInBuilder = _bucket.MutateIn<BookEntity>(id);
            foreach (var operation in request.Operations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Replace:
                        mutateInBuilder.Replace(operation.path, operation.value);
                        break;
                    case OperationType.Add:
                        mutateInBuilder.Insert(operation.path, operation.value);
                        break;
                    case OperationType.Remove:
                        mutateInBuilder.Remove(operation.path);
                        break;
                }
            }

            var result = await mutateInBuilder.ExecuteAsync();

            return result.Success;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _bucket.RemoveAsync(id);
            return result.Success;
        }
    }
}