﻿namespace Dfe.SchoolAccount.SignIn.Models;

using System.Text.Json.Serialization;

public sealed class Organisation
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public string Urn { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public string Ukprn { get; set; } = null!;

    public string LegacyId { get; set; } = null!;

    public string Sid { get; set; } = null!;

    [JsonPropertyName("DistrictAdministrative_code")]
    public string DistrictAdministrative_Code { get; set; } = null!;
}
