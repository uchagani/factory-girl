using FactoryGirlCore;

namespace FactoryGirlTests.Models
{
    public class Book : IRepository<Book>
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public Category Category { get; set; }
        public int Isbn { get; set; }
        public bool Saved { get; private set; }
        public string Borrower { get; set; }

        public Book Save()
        {
            Saved = true;
            return this;
        }
    }

    public enum Category
    {
        Business,
        Art,
        Travel,
        History
    }
}
