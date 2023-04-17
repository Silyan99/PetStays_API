using System;
using System.Collections.Generic;

namespace PetStays_API.DBModels;

public partial class Request
{
    public int Id { get; set; }

    public int? MadeBy { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? TimeFrom { get; set; }

    public TimeSpan? TimeTo { get; set; }

    public int? PetId { get; set; }

    public bool? IsPaymentDone { get; set; }

    public string? Status { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? MadeByNavigation { get; set; }

    public virtual Pet? Pet { get; set; }
}
