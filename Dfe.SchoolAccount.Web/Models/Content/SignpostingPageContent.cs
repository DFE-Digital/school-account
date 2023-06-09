﻿namespace Dfe.SchoolAccount.Web.Models.Content;

using Contentful.Core.Models;

public sealed class SignpostingPageContent : IContent
{
    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public Document? Body { get; set; }

    public bool IsApplicableToAcademies { get; set; }

    public bool IsApplicableToLaMaintainedSchools { get; set; }
}
