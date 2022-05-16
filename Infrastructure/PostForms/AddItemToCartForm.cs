using Infrastructure.Models;

namespace Infrastructure.PostForms
{
    public class AddItemToCartForm
    {
        public Item item { get; set; }
        public int CartId { get; set; }
        public int Count { get; set; }
    }
}
