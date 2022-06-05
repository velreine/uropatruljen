using System.Collections.Generic;
using CommonData.Model.Entity;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CommonData.Model.DTO {

/// <summary>
/// DTO object for a "AuthenticatedUserHomeItem"
/// </summary>
public class AuthenticatedUserHomeResponseDTO
{
    public int Id { get; }

    public string Name { get; }

    private ICollection<AuthenticatedUserRoom> _rooms;

    public IReadOnlyCollection<AuthenticatedUserRoom> Rooms => (IReadOnlyCollection<AuthenticatedUserRoom>)_rooms;

    public AuthenticatedUserHomeResponseDTO(int id, string name, ICollection<AuthenticatedUserRoom> rooms)
    {
        Id = id;
        Name = name;
        _rooms = rooms;
    }
    
}
}