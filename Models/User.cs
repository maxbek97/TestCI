using System;
using System.Collections.Generic;

namespace TestCI.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserLogin { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
