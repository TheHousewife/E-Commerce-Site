namespace LiveChat.Data;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    
    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }

    public Cart Cart { get; set; }
}