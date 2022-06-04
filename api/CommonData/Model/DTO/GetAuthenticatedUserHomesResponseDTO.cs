using System.Collections.Generic;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CommonData.Model.DTO;

/// <summary>
/// DTO object for a "AuthenticatedUserHomeItem"
/// </summary>
public class AuthenticatedUserHome
{
    public int Id { get; }

    public string Name { get; }

    public AuthenticatedUserHome(int id, string name)
    {
        Id = id;
        Name = name;
    }
}