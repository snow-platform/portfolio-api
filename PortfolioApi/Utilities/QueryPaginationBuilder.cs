using System.Data;
using Dapper;
using PortfolioApi.EntityValueObject;
using PortfolioApi.Extensions;

namespace PortfolioApi.Utilities;

public class QueryPaginationBuilder
{
    private readonly Pagination _pagination;
    private readonly DynamicParameters _parameters;

    private string? _sortBy;
    private string? _sortDi;
    private string? _filter;

    public QueryPaginationBuilder(Pagination pagination, DynamicParameters parameters)
    {
        _pagination = pagination;
        _parameters = parameters;
    }

    public QueryPaginationBuilder ApplySize()
    {
        _parameters.Add("@Size", _pagination.SanitizeSize());

        return this;
    }

    public QueryPaginationBuilder ApplyOffset()
    {
        _parameters.Add("@Offset", (_pagination.SanitizePage() - 1) * _pagination.SanitizeSize());

        return this;
    }

    public QueryPaginationBuilder ApplySearch()
    {
        _parameters.Add("@Search", _pagination.Search, DbType.String);

        return this;
    }

    public QueryPaginationBuilder InsertSortBy(params string[] allowed)
    {
        _sortBy = _pagination.SanitizeSortBy(allowed);

        return this;
    }

    public QueryPaginationBuilder InsertSortDi()
    {
        _sortDi = _pagination.SanitizeSortDirection();

        return this;
    }

    public QueryPaginationBuilder InsertFilter(params string[] allowed)
    {
        var sanitize = _pagination.SanitizeFilters(allowed);

        if (sanitize?.Count > 0)
        {
            _filter = sanitize.ToQueryWithDynamicParameters(_parameters);
        }

        return this;
    }

    public string Build(string query)
    {
        query = query.Replace("$Sort", _sortBy ?? "");
        query = query.Replace("$Direction", _sortDi ?? "");
        query = query.Replace("$Filter", _filter ?? "true");

        return query;
    }
}