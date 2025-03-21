﻿using System.ComponentModel.DataAnnotations;

namespace BaseApplication.Models;

public record PagingInformationRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    /// <summary>
    /// Page size of data
    /// </summary>
    [Required]
    [Range(1, 100)]
    public required int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    /// <summary>
    /// Page number of data
    /// </summary>
    [Required]
    [Range(1, Double.MaxValue)]
    public required int PageNumber { get; set; } = 1;

    /// <summary>
    /// Order by as string, it comes from UI based on end-user needs
    /// </summary>
    public string? OrderBy { get; set; }
}