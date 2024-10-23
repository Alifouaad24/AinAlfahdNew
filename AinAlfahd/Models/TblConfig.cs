using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class TblConfig
{
    public int Id { get; set; }

    public string Db_env { get; set; }

    public int? MissingFav { get; set; }
}
