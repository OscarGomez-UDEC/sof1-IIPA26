using System;
using System.Collections.Generic;

namespace sof1_IIPA26.Repositories.Models;

public partial class State
{
    public int IdState { get; set; }

    public string Name { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
