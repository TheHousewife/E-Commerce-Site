using Microsoft.EntityFrameworkCore;

namespace LiveChat.Data;

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartAsync(string userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }


    public async Task AddToCartAsync(string userId, Product product)
    {
        var cart = await GetCartAsync(userId);
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity++;
            await _context.SaveChangesAsync();
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                ImageUrl = product.ImageUrl
            });
        }

        if (cart.Id == 0) _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string userId, int productId)
    {
        var cart = await GetCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cart.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}