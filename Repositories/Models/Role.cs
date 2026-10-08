using System;
using System.Collections.Generic;

namespace sof1_IIPA26.Repositories.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
