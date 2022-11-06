namespace Authorisation2.Entities;

public class User
{
    public int Id        { get; set; }
    public string? Name  { get; set; }
    public int? Password { get; set; }
    public string? Role  { get; set; }
    public string? Email { get; set; }
    public string Key    { get; set; }
}
