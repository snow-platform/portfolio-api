using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Test.EntityValueObject;

public class PaginationTest
{
    [Theory]
    [InlineData(-5, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(7, 7)]
    public void SanitizePage_never_returns_below_one(int page, int expected)
    {
        // arrange
        var pagination = new Pagination
        {
            Page = page
        };

        // act
        var paginationPage = pagination.SanitizePage();

        // assert
        Assert.Equal(expected, paginationPage);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    [InlineData(50, 10)]
    public void SanitizeSize_clamps_to_default_bounds(int size, int expected)
    {
        // arrange
        var pagination = new Pagination
        {
            Size = size
        };

        // act
        var paginationSize = pagination.SanitizeSize();

        // assert
        Assert.Equal(expected, paginationSize);
    }

    [Theory]
    [InlineData(3, 20, 5)] // between custom bounds -> size
    [InlineData(3, 20, 2)] // below custom minimum -> minimum
    [InlineData(3, 20, 25)] // above custom maximum -> maximum
    public void SanitizeSize_honours_custom_bounds(int minimum, int maximum, int size)
    {
        // arrange
        var pagination = new Pagination
        {
            Size = size
        };

        // act
        var paginationSize = pagination.SanitizeSize(minimum, maximum);
        var expected = Math.Min(maximum, Math.Max(size, minimum));

        // assert
        Assert.Equal(expected, paginationSize);
    }

    [Fact]
    public void SanitizeSortBy_returns_the_column_when_allowed()
    {
        // arrange
        var pagination = new Pagination
        {
            SortBy = "Email"
        };

        // act
        var paginationSortBy = pagination.SanitizeSortBy("Id", "Email");

        // assert
        Assert.Equal("Email", paginationSortBy);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("DropTable")]
    public void SanitizeSortBy_falls_back_to_Id_when_missing_or_disallowed(string? sortBy)
    {
        // arrange
        var pagination = new Pagination
        {
            SortBy = sortBy
        };

        // act
        var paginationSortBy = pagination.SanitizeSortBy("Id", "Email");

        // assert
        Assert.Equal("id", paginationSortBy);
    }

    [Theory]
    [InlineData("asc", "asc")]
    [InlineData("desc", "desc")]
    [InlineData("ASC", "asc")]
    [InlineData("sideways", "asc")]
    [InlineData(null, "asc")]
    public void SanitizeSortDirection_only_allows_asc_or_desc(string? direction, string expected)
    {
        // arrange
        var pagination = new Pagination
        {
            SortDirection = direction
        };
        
        // act
        var paginationSortDi = pagination.SanitizeSortDirection();
        
        // assert
        Assert.Equal(expected, paginationSortDi);
    }

    [Fact]
    public void SanitizeFilters_returns_null_when_there_are_no_filters()
    {
        // arrange
        var pagination = new Pagination
        {
            Filters = null
        };
        
        // act
        var paginationFilters = pagination.SanitizeFilters("Email");
        
        // assert
        Assert.Null(paginationFilters);
    }

    [Fact]
    public void SanitizeFilters_keeps_only_allowed_names()
    {
        // arrange
        var pagination = new Pagination
        {
            Filters =
            [
                new Filter
                {
                    Name = "Email",
                    Value = "sample@google.com"
                },
                new Filter
                {
                    Name = "Secret",
                    Value = "xxx"
                }
            ]
        };
        
        // act
        var paginationFilters = pagination.SanitizeFilters("Email");

        Assert.NotNull(paginationFilters);
        Assert.Single(paginationFilters);
        Assert.Equal("Email", paginationFilters[0].Name);
    }

    [Fact]
    public void SanitizeFilters_deduplicates_by_name_keeping_the_first()
    {
        // arrange
        var pagination = new Pagination
        {
            Filters =
            [
                new Filter
                {
                    Name = "Email",
                    Value = "first"
                },
                new Filter
                {
                    Name = "Email",
                    Value = "second"
                }
            ]
        };

        // act
        var paginationFilters = pagination.SanitizeFilters("Email");

        // assert
        Assert.NotNull(paginationFilters);
        Assert.Single(paginationFilters);
        Assert.Equal("first", paginationFilters[0].Value);
    }
}