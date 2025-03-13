﻿using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Role.Queries.GetRoles;

public class RoleDto : IMapFrom<Domain.Entities.Role>
{
    public long Id { get; set; }

    [Required]
    public required string Title { get; set; }
}
