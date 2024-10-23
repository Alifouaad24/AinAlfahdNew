using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class RegistrationLog
{
    public int Id { get; set; }

    public int? LabId { get; set; }

    public int? RepresentitiveId { get; set; }

    public DateOnly? InsertDate { get; set; }

    public int? RegistrationType { get; set; }

    public int? Amount { get; set; }
}
