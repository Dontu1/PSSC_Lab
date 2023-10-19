using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Domain.Models
{
    public record Cart(List<Product> Products, Client Client, decimal Quantity);
}
