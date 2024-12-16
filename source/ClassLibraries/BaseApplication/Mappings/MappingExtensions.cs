using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Models;
using Microsoft.EntityFrameworkCore;


namespace BaseApplication.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> ProjectToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, string? orderBy = null, CancellationToken cancellationToken = default) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, orderBy, cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration, CancellationToken cancellationToken) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
}
