using System;
using System.Collections.Generic;

namespace IMDB_App.Models;

public partial class Name
{
    public string NameId { get; set; } = null!;

    public string? PrimaryName { get; set; }

    public int? BirthYear { get; set; }

    public int? DeathYear { get; set; }

    public string? PrimaryProfession { get; set; }

    public virtual ICollection<Principal> Principals { get; set; } = new List<Principal>();

    public virtual ICollection<Title> Titles { get; set; } = new List<Title>();

    public virtual ICollection<Title> Titles1 { get; set; } = new List<Title>();

    public virtual ICollection<Title> TitlesNavigation { get; set; } = new List<Title>();
}
