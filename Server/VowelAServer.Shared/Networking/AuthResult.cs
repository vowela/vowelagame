namespace VowelAServer.Shared.Networking
{
    public enum AuthResult
    {
        Unauthorized,
        Authorized
    }

    public enum RegisterResult
    {
        Registered,
        AuthDataNotValid
    }
}