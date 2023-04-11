using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;

namespace LibManager.Models;

public class User
{

    [Key]
    public string id { get; set; } = Guid.NewGuid().ToString();
    public DateTime birthDate { get; set; } = DateTime.Now;
    public int sex { get; set; } = 0;
    public string address { get; set; } = default!;
    public string phone { get; set; } = default!;

    [Required(ErrorMessage = "Password is require")]
    [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "password has at least 6 characters")]
    public string hashPassword { set; get; } = default!;
    public string email { set; get; } = default!;
    public string position { set; get; } = default!;
    public Role role { set; get; } = Role.reader;


}