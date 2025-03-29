using System;
using System.Collections.Generic;

namespace IMDB_App.Models;

public partial class Episode
{
    public string TitleId { get; set; } = null!;

    public string? ParentTitleId { get; set; }

    public int? SeasonNumber { get; set; }

    public int? EpisodeNumber { get; set; }

    public virtual Title? ParentTitle { get; set; }

    public virtual Title Title { get; set; } = null!;
}
