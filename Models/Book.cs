using System.ComponentModel.DataAnnotations;

namespace LibManager.Models;

public class Book
{
    [Key]
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string title { get; set; } = default!;
    public string author { get; set; } = default!;
    public int publicYear { get; set; } = default!;
    public string publisher { get; set; } = default!;
    public int quantity { set; get; } = default!;
    public Category category { set; get; } = default!;


}