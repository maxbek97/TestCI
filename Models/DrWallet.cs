using System;
using System.Collections.Generic;

namespace TestCI.Models;

public partial class DrWallet
{
    public Guid IdDrw { get; set; }

    public Guid ClientId { get; set; }

    public Guid? IdBill { get; set; }
    public StatusWallet Status { get; set; }
    public virtual Client Client { get; set; } = null!;
}
