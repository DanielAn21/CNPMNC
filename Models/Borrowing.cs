namespace LibManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
public class Borrowing
{
    [Key]
    public string id { get; set; } = Guid.NewGuid().ToString();

    public User user { set; get; } = default!;
    public Book book { set; get; } = default!;

    public DateTime brrowingDate { set; get; } = DateTime.Now;
    public DateTime dueTime { set; get; } = DateTime.Now;
    public DateTime returnDate { set; get; } = DateTime.Now;

    public double fineAmount { set; get; } = default!;

}