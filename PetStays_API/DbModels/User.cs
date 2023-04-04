using System;
using System.Collections.Generic;

namespace PetStays_API.DBModels;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Mobile { get; set; }

    public DateTime? CreatedDate { get; set; }
}
