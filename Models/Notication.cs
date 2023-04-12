using System.ComponentModel.DataAnnotations;

namespace LibManager.Models
{
    public class Notication
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();

        public string text { get; set; } = default!;

        public DateTime dateCreate { get; set; } = DateTime.Now;

        public User sender { set; get; } = default!;
        public User receiver { set; get; } = default!;

        public TypeMessage typeMessage { set; get; } = default!;

    }
}