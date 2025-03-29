using System;
using System.Collections.Generic;

namespace IMDB_App.Models;

public partial class Title
{
    public string TitleId { get; set; } = null!;

    public string? TitleType { get; set; }

    public string? PrimaryTitle { get; set; }

    public string? OriginalTitle { get; set; }

    public bool? IsAdult { get; set; }

    public short? StartYear { get; set; }

    public short? EndYear { get; set; }

    public short? RuntimeMinutes { get; set; }

    public virtual ICollection<Episode> EpisodeParentTitles { get; set; } = new List<Episode>();

    public virtual Episode? EpisodeTitle { get; set; }

    public virtual Rating? Rating { get; set; }

    public virtual ICollection<TitleAlias> TitleAliases { get; set; } = new List<TitleAlias>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Name> Names { get; set; } = new List<Name>();

    public virtual ICollection<Name> Names1 { get; set; } = new List<Name>();

    public virtual ICollection<Name> NamesNavigation { get; set; } = new List<Name>();
}
