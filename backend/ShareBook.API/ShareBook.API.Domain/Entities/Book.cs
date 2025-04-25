using System.ComponentModel.DataAnnotations;

namespace ShareBook.API.Domain.Entities;

public class Book : Audit
{
    public Guid Id { get; set; }
    [StringLength(128)]
    public required string LibraryId { get; set; }
    public virtual Library? Library { get; set; }
    [StringLength(512)]
    public string? Publisher { get; set; }
    [StringLength(2000)]
    public string? Synopsis { get; set; }
    [StringLength(64)]
    public string? Language { get; set; }
    [StringLength(512)]
    public string? Image { get; set; }
    [StringLength(512)]
    public string? ImageOriginal { get; set; }
    [StringLength(512)]
    public string? TitleLong { get; set; }
    [StringLength(256)]
    public string? Edition { get; set; }
    [StringLength(256)]
    public string? Dimensions { get; set; }
    public int Pages { get; set; }
    public DateTime DatePublished { get; set; }
    public List<string>? Subjects { get; set; }
    public List<string>? Authors { get; set; }
    [StringLength(512)]
    public required string Title { get; set; }
    [StringLength(16)]
    public string? Isbn13 { get; set; }
    [StringLength(256)]
    public string? Binding { get; set; }
    [StringLength(16)]
    public string? Isbn { get; set; }
    [StringLength(16)]
    public string? Isbn10 { get; set; }
}