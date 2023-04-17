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

    public string? Role { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Availability> Availabilities { get; } = new List<Availability>();

    public virtual ICollection<Pet> Pets { get; } = new List<Pet>();

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
