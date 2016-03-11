using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata.Internal;

namespace PizzaCRUD.Models
{
    public class Pizza
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name="Nazwa pizzy")]
        [StringLength(64, ErrorMessage = "Nazwa musi mieć długość od 2 do 64 znaków", MinimumLength = 2)]
        public string Name { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Rodzaj ciasta")]
        public string DoughType { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name="Cena (M)")]
        public decimal PriceMedium { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Cena (S)")]
        public decimal PriceSmall { set; get; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name="Cena (L)")]
        public decimal PriceLarge { set; get; }

        public ICollection<PizzaIngridient> Ingridients { set; get; }

        [NotMapped]
        public Dictionary<string,bool> UsedIngridients { set; get; }
    }
}
