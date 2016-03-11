using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCRUD.Models
{
    public class PizzaIngridient
    {
        public Guid PizzaId { set; get; }

        public Guid IngridientId { set; get; }

        public Pizza Pizza { set; get; }

        public Ingridient Ingridient { set; get; }
    }
}
