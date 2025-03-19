using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AinAlfahd.Models_New;

public partial class Platform
{
    [Key]
    public int Platform_id { get; set; }

    public string? Desciption { get; set; }
}
