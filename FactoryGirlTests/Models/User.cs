using FactoryGirlCore;
namespace FactoryGirlTests.Models
{
    public class User : IRepository<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Role Role { get; set; }
        public bool Saved { get; private set; }

        public User Save()
        {
            Saved = true;
            return this;
        }
    }

    public enum Role
    {
        Admin,
        User
    }
}
