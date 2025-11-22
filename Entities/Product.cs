namespace Entities
{
    public class Product
    {
        public Product(string description)
        {
            Description = description;
            Id = Guid.NewGuid();
        }

        public Product(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Description { get; private set; }
    }

    public class ProductViewModel
    {
        public string Description { get; set; }
        public bool ShouldCommit { get; set; } = true;
    }
}
