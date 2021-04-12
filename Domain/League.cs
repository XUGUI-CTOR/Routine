using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class League
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50),Required]
        public string Country { get; set; }
    }

    public class Clue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        [Column(TypeName ="date")]
        public DateTime DateOfEstablishment { get; set; }
        public string History { get; set; }
        public League League { get; set; }
        public List<Player> Players { get; set; }
    }
}
