namespace OTP.Domain;

public interface IHashService
{
    Hash Hash(string data);
}