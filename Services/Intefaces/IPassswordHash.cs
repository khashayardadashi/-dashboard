namespace cvweb.Services.Intefaces;

public interface IPassswordHash
{
    string HashPassword(string password);
}