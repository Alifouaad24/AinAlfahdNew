using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class BusinessArea
{
    public int Id { get; set; }

    public int? BusinessId { get; set; }

    public int? CityId { get; set; }
}
