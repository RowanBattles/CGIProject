using System.ComponentModel.DataAnnotations;

namespace CGI.Models
{
    public class Journey
    {
        [Key]
        public int Journey_ID { get; set; }

        [Required]
        public int User_ID { get; set; }

        [Required]
        public int Total_Distance { get; set; }

        [Required]
        public int Total_Emission { get; set; }

        [Required]
        [StringLength(255)]
        public string Start { get; set; }

        [Required]
        [StringLength(255)]
        public string End { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
