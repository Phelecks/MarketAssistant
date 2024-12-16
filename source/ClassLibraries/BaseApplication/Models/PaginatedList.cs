using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace BaseApplication.Models;

public class PaginatedList<T>
{
    public List<T> data { get; }

    public PagingInformationResponse pagingInformation { get; }

    public PaginatedList(List<T> data, int count, int pageNumber, int pageSize)
    {
        pagingInformation =
            new PagingInformationResponse(pageNumber, (int) Math.Ceiling(count / (double) pageSize), count);
        this.data = data;
    }

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

public class PagingInformationResponse
{
    public PagingInformationResponse(int pageNumber, int totalPages, int totalCount)
    {
        this.pageNumber = pageNumber;
        this.totalPages = totalPages;
        this.totalCount = totalCount;
    }

    public int pageNumber { get; }
    public int totalPages { get; }
    public int totalCount { get; }

   

    public bool hasPreviousPage => pageNumber > 1;

    public bool hasNextPage => pageNumber < totalPages;
}