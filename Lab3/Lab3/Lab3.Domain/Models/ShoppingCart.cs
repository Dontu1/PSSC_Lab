using CSharp.Choices;
using Lab3.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Domain.Models
{
    [AsChoice]
    public static partial class ShoppingCart
    {
        public interface IShoppingCart { }
        public record EmptyCart(UnvalidatedShoppingCart Cart) : IShoppingCart;

        public record UnvalidatedCart(UnvalidatedShoppingCart Cart) : IShoppingCart;

        public record ValidatedCart(ValidatedShoppingCart Cart) : IShoppingCart;

        public record PaidCart(ValidatedShoppingCart Cart, PaymentInfo PaymentInfo) : IShoppingCart;

    }
}
