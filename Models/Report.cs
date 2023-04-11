using System.ComponentModel.DataAnnotations;

namespace LibManager.Models
{
    public class Report
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public int month { set; get; } = default!;
        public int year { set; get; } = default!;
        public int countUser { set; get; } = default!;
        public int countBorrowing { set; get; } = default!;
        public int countReader { set; get; } = default!;
    }
}