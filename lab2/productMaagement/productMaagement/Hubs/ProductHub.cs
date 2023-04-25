using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using productMaagement.Models;

namespace productMaagement.Hubs
{
    public class ProductHub : Hub
    {
        private readonly ProductDbContext _context; 
        public ProductHub(ProductDbContext context)
        {
            _context = context;
        }
        public async Task decreaseQuantiy(int id, int quantity)
        {
            Clients.All.SendAsync("updateQuantity", id, quantity - 1);
            
            // find the product with the given id
            var product = await _context.Products.FindAsync(id);

            // update the product's quantity and save changes to the database
            product.Quantity = quantity - 1;
           await _context.SaveChangesAsync();
            
        }
        public async Task addComment(int id, string text, string name)
        {
            Clients.All.SendAsync("updateComments", id, text, name);

            // create a new Comment entity with the provided data
            var comment = new Comment
            {
                Text = text,
                Name = name,
                ProductId = id
            };

            // add the comment to the DbContext and save changes to the database
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }
}
