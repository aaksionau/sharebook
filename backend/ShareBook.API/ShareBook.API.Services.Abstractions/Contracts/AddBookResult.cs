namespace ShareBook.API.Services.Abstractions.Contracts;

public class AddBookResult
{
    public string Message { get; set; } = string.Empty;
    public Guid? BookId { get; set; }
}
