using System;

namespace SwashbuckleExample.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
