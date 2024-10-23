using System;
using System.Collections.Generic;

namespace AinAlfahd.Models;

public partial class UserMerchant
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? MerchantId { get; set; }
}
