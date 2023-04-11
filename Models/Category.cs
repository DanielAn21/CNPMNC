using System.ComponentModel.DataAnnotations;

namespace LibManager.Models;

public class Category
{
    [Key]
    public string id { get; set; } = Guid.NewGuid().ToString();

    public string name { get; set; } = default!;
    public List<Book> Books { get; set; } = default!;
}