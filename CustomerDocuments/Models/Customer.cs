using System;
using System.Collections.Generic;

namespace CustomerDocuments.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? TaxCode { get; set; }

    public string? Vat { get; set; }

    public string? Address { get; set; }
}
