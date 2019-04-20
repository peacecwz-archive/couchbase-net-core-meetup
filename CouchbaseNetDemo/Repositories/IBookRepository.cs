using System.Threading.Tasks;
using Couchbase.IO;
using Microsoft.AspNetCore.JsonPatch;

namespace CouchbaseNetDemo.Repositories
{
    public interface IBookRepository
    {
        Task<BookEntity> GetById(string id);
        Task<bool> IsExists(string id);
        Task<bool> Add(BookEntity book);
        Task<bool> Upsert(string id, BookEntity book);
        Task<bool> Patch(string id, JsonPatchDocument request);
        Task<bool> Delete(string id);
    }
}