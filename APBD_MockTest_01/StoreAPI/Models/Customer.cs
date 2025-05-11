using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models;

public class Customer
{
    [Key]
    public int CustomerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}