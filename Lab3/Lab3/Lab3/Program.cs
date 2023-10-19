using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lab3.Domain.Models;
using static Lab3.Domain.Models.ShoppingCart;

namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cart = ReadCartContent();
            UnvalidatedCart unvalidatedCart = new(cart);
            IShoppingCart result = ValidateCart(unvalidatedCart);
            result.Match(
                whenEmptyCart: emptyCart =>
                {
                    Console.WriteLine("The cart is empty!");
                    return emptyCart;
                },
                whenUnvalidatedCart: unvalidatedCart =>
                {
                    Console.WriteLine("The cart is unvalidated!");
                    return unvalidatedCart;
                },
                whenValidatedCart: validatedCart =>
                {
                    Console.WriteLine("The cart is validated!");
                    PaymentInfo payment = ReadPaymentInfo();
                    return PaidCart(validatedCart, payment);
                },
                whenPaidCart: PaidCart =>
                {
                    Console.WriteLine("The cart is paid!");
                    return PaidCart;
                }
            );
        }

        private static PaymentInfo ReadPaymentInfo()
        {
            PaymentMethod method = ReadPaymentMethod();
            decimal totalAmount = ReadTotalAmount();
            PaymentInfo? PaymentInfo = new(method, totalAmount);
            return PaymentInfo;
        }

        private static PaymentMethod ReadPaymentMethod()
        {
            Console.WriteLine("Choose a payment method:");
            Console.WriteLine("1: Cash");
            Console.WriteLine("2: Card");
            Console.WriteLine("3: IBAN");

            while (true)
            {
                string? input = Console.ReadLine();
                if (input == "1") return PaymentMethod.Cash;
                if (input == "2") return PaymentMethod.Card;
                if (input == "3") return PaymentMethod.IBAN;

                Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
            }
        }

        private static decimal ReadTotalAmount()
        {
            while (true)
            {
                Console.Write("Enter the total amount: ");
                string? input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal totalAmount))
                {
                    return totalAmount;
                }
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            }
        }

        private static UnvalidatedShoppingCart ReadCartContent()
        {
            var quantityOfProducts = 0;
            List<Product> listOfProducts = new();
            var clientName = ReadValue("Client name: ");
            while (string.IsNullOrEmpty(clientName))
            {
                Console.WriteLine("Client name cannot be empty!");
                clientName = ReadValue("Please enter client name: ");
            }
            var clientAddress = ReadValue("Client address: ");
            while (string.IsNullOrEmpty(clientAddress))
            {
                Console.WriteLine("Client address cannot be empty!");
                clientAddress = ReadValue("Please enter client address: ");
            }
            Client client = new Client(clientName, clientAddress);
            Regex ValidPattern = new("^[0-9]{6}$");
            do
            {
                var codeProduct = ReadValue("Code product: ");
                if (string.IsNullOrEmpty(codeProduct))
                {
                    break;
                }
                while (!ValidPattern.IsMatch(codeProduct))
                {
                    Console.WriteLine("Invalid code! It must contain exactly 6 digits!");
                    codeProduct = ReadValue("Please enter a correctly formatted code product: ");
                }
                quantityOfProducts++;
                Product product = new Product(codeProduct);
                listOfProducts.Add(product);
            } while (true);
            Cart cart = new Cart(listOfProducts, client, quantityOfProducts);
            UnvalidatedShoppingCart shoppingCart = new UnvalidatedShoppingCart(cart);
            return shoppingCart;
        }

        private static IShoppingCart ValidateCart(UnvalidatedCart unvalidatedCart) =>
            unvalidatedCart.Cart.Cart.Quantity == 0 ?
            new EmptyCart(new UnvalidatedShoppingCart(unvalidatedCart.Cart.Cart))
            : new ValidatedCart(new ValidatedShoppingCart(unvalidatedCart.Cart.Cart));

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static IShoppingCart PaidCart(ValidatedCart validatedCart, PaymentInfo payment)
        {
            if (payment.TotalAmount != 0)
            {
                Console.WriteLine("The cart is paid");
                return new PaidCart(new ValidatedShoppingCart(validatedCart.Cart.Cart), payment);
            }
            else
            {
                return validatedCart;
            }
        }
    }


}
