namespace Marketplace.BaseLibrary.Exception.Settings;

public class SettingsServiceIsEmptyException(string message) : NullReferenceException(message)
{
    
}