using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class TestDetail
{
    public int Id { get; set; }

    public int? TestId { get; set; }

    public int? TestTypeId { get; set; }
}
