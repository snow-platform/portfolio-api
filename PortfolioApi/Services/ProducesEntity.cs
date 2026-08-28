namespace PortfolioApi.Services;

public class ProducesEntity<T>
{
    protected ProducesEntity()
    {
    }

    public T? Entity { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public string? Description { get; set; }

    public bool Success
    {
        get => EqualityComparer<T>.Default.Equals(Entity, default) is false;
    }
}

public class ProducesEntityGood<T> : ProducesEntity<T>
{
    public ProducesEntityGood(T value, int statusCode = StatusCodes.Status200OK)
    {
        Entity = value;
        StatusCode = statusCode;
    }
}

public class ProducesEntityFail<T> : ProducesEntity<T>
{
    public ProducesEntityFail(int statusCode,
        string error = "Unknown Error",
        string description = "Unable to generate an exact error message")
    {
        Entity = default;
        StatusCode = statusCode;
        Error = error;
        Description = description;
    }
}