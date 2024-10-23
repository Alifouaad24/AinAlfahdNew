using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class UsCustomerReview
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? BusinessId { get; set; }

    public int? ReviewScore { get; set; }
}
