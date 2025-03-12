using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace BaseApplication.Models;

public class PaginatedList<T>
{
    public PaginatedList(List<T> data, int count, int pageNumber, int pageSize)
    {
        Data = data;
        PagingInformation = new PagingInformationResponse(pageNumber, (int)Math.Ceiling(count / (double)pageSize), count);
    }

    //For cache pipeline
    [System.Text.Json.Serialization.JsonConstructor]
    public PaginatedList(List<T> data, PagingInformationResponse pagingInformation)
    {
        Data = data;
        PagingInformation = pagingInformation;
    }


    public List<T> Data { get; }

    public PagingInformationResponse PagingInformation { get; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, string? orderBy, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(orderBy)) source = ApplySort(source, orderBy);
        var count = await source.CountAsync(cancellationToken: cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> ApplySort(IQueryable<T> entities, string? orderByQueryString)
    {
        if (!entities.Any())
            return entities;

        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return entities;
        }

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(' ')[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
                continue;

            var sortingOrder = param.ToLower().EndsWith(" desc") || param.ToLower().EndsWith(" descending") ? "descending" : "ascending";

            orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        return entities.OrderBy(orderQuery);
    }
}

public class PagingInformationResponse(int pageNumber, int totalPages, int totalCount)
{
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = totalPages;
    public int TotalCount { get; } = totalCount;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}