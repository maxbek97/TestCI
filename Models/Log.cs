using System;
using System.Collections.Generic;

namespace TestCI.Models;

public partial class Log
{
    public long Id { get; set; }

    public string TableName { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public string? ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }
}
