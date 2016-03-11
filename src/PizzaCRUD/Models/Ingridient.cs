using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata.Internal;

namespace PizzaCRUD.Models
{
    public class Ingridient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Nazwa")]
        [StringLength(64,ErrorMessage = "Nazwa musi mieć długość od 2 do 64 znaków",MinimumLength = 2)]
        public string Name { set; get; }

        public ICollection<PizzaIngridient> Pizzas { set; get; }
    }
}
