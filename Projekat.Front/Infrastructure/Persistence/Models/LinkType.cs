using System;
using System.Collections.Generic;

namespace Projekat.Front.Infrastructure.Persistence.Models
{
    public partial class LinkType
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
