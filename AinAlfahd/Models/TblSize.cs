﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AinAlfahd.Models;

public partial class TblSize
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? GroupIndex { get; set; }

    public int? OrderIndex { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
}

