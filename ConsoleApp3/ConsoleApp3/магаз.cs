using System;
using System.Collections.Generic;
using System.Linq;


public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int Rating { get; set; }

    public Product(string name, decimal price, string description, string category, int rating)
    {
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Rating = rating;
    }
}


public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        PurchaseHistory = new List<Order>();
    }
}


public class Order
{
    public List<Product> Products { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    public Order(List<Product> products, int quantity, decimal totalPrice, string status)
    {
        Products = products;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Status = status;
    }
}


public interface ISearchable
{
    List<Product> SearchByPrice(decimal maxPrice);
    List<Product> SearchByCategory(string category);
    List<Product> SearchByRating(int minRating);
}


public class Store : ISearchable
{
    public List<Product> Products { get; set; }
    public List<User> Users { get; set; }
    public List<Order> Orders { get; set; }

    public Store()
    {
        Products = new List<Product>();
        Users = new List<User>();
        Orders = new List<Order>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public void PlaceOrder(User user, List<Product> products, int quantity)
    {
        decimal totalPrice = products.Sum(p => p.Price * quantity);
        Order order = new Order(products, quantity, totalPrice, "Pending");
        user.PurchaseHistory.Add(order);
        Orders.Add(order);
    }

    public List<Product> SearchByPrice(decimal maxPrice)
    {
        return Products.Where(p => p.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchByRating(int minRating)
    {
        return Products.Where(p => p.Rating >= minRating).ToList();
    }
}

class Program
{
    static void Main()
    {
        Store store = new Store();

       
        store.AddProduct(new Product("Laptop", 800, "High-performance laptop", "Electronics", 4));
        store.AddProduct(new Product("Coffee Maker", 50, "Coffee maker with grinder", "Kitchen Appliances", 4));
        store.AddProduct(new Product("Running Shoes", 100, "Lightweight running shoes", "Sports", 5));

        store.AddUser(new User("user1", "password1"));
        store.AddUser(new User("user2", "password2"));

        
        User user1 = store.Users[0];
        Product laptop = store.Products[0];
        store.PlaceOrder(user1, new List<Product> { laptop }, 2);

        Console.WriteLine("Purchase history for user1:");
        foreach (var order in user1.PurchaseHistory)
        {
            Console.WriteLine($"Order Status: {order.Status}, Total Price: {order.TotalPrice:C}");
        }

        
        Console.WriteLine("Products under $100:");
        List<Product> affordableProducts = store.SearchByPrice(100);
        foreach (var product in affordableProducts)
        {
            Console.WriteLine($"{product.Name} - {product.Price:C}");
        }

        Console.WriteLine("Electronics category:");
        List<Product> electronics = store.SearchByCategory("Electronics");
        foreach (var product in electronics)
        {
            Console.WriteLine($"{product.Name} - {product.Category}");
        }

        Console.ReadLine();
    }
}
