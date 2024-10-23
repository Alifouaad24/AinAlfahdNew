using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class Preorder
{
    public int Id { get; set; }

    public int? CustId { get; set; }

    public int? Totalamount { get; set; }

    public int? OrderDate { get; set; }

    public int? InsertDate { get; set; }

    public int? Qty { get; set; }

    public int? Status { get; set; }
}
