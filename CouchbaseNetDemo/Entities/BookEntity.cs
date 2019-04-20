using System;

namespace CouchbaseNetDemo.Repositories
{
    public class BookEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}