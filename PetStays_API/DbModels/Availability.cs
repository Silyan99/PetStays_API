using System;
using System.Collections.Generic;

namespace PetStays_API.DBModels;

public partial class Availability
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? TimeStart { get; set; }

    public TimeSpan? TimeEnd { get; set; }

    public bool? FullDay { get; set; }

    public int? AdminId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? Admin { get; set; }
}
