using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Domain.Models
{
    public enum PaymentMethod
    {
        Cash,
        Card,
        IBAN
    }
    public record PaymentInfo(PaymentMethod PaymentMethod, decimal TotalAmount);
}
