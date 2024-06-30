namespace Marketplace.BaseLibrary.Exception.Data;

public class CannotAccessToRepository(string message) : NullReferenceException(message)
{
    
}