namespace PortfolioApi.ExternalServices.CMS;

public class CmsEither
{
    protected CmsEither(int statusCode)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

public class CmsEitherOk<T> : CmsEither
{
    public CmsEitherOk(int statusCode, T value) : base(statusCode)
    {
        Value = value;
    }

    public T Value { get; }
}

public class CmsEitherEmpty : CmsEither
{
    public CmsEitherEmpty(int statusCode) : base(statusCode)
    {
    }
}

public class CmsEitherError : CmsEither
{
    public CmsEitherError(int statusCode, string error, string description) : base(statusCode)
    {
        Error = error;
        Description = description;
    }

    public string Error { get; }
    public string Description { get; }
}