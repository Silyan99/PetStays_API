using System;
using System.Collections.Generic;

namespace PetStays_API.DBModels;

public partial class Contact
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }
}
