using System;
using System.Collections.Generic;

namespace TestCI.Models;

public partial class Client
{
    public Guid Mid { get; set; }

    public string LastName { get; set; } = null!;

    public string FisrtName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public Guid? IdDr { get; set; }

    public virtual ICollection<DrWallet> DrWallets { get; set; } = new List<DrWallet>();
}
