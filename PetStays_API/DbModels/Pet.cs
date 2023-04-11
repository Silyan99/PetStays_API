using System;
using System.Collections.Generic;

namespace PetStays_API.DBModels;

public partial class Pet
{
    public int Id { get; set; }

    public string? Category { get; set; }

    public string? Uid { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public bool? Vaccinated { get; set; }

    public string? Color { get; set; }

    public string? Breed { get; set; }

    public string? Details { get; set; }

    public int? OwnerId { get; set; }

    public string? Photo { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? Owner { get; set; }

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
