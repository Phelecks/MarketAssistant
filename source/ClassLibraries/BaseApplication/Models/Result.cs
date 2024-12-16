namespace BaseApplication.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        this.succeeded = succeeded;
        this.errors = errors.ToArray();
    }

    public bool succeeded { get; set; }

    public string[] errors { get; set; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}
