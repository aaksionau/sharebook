using ShareBook.API.Contracts;

namespace ShareBook.API.Services.Abstractions.Contracts;

public class SearchResultDto
{
    public BookDto Book { get; set; }
    public bool ExistInCurrentLibrary { get; set; }
    public int NumberOfCopies { get; set; }
}
