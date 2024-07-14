using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;
using System.Text.Json;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Utils.Base.Logger.Extension;

namespace Marketplace.BaseLibrary.Entity.Base.Logger;

/// <summary>
/// Сушность лога для записи в БД
/// </summary>
public class Log
{
    public Log(LogTypeEnum logType, string callingClass, string callingMethod, string logValue)
    {
        CallingClass = callingClass.FormatCallingMethod();
        CallingMethod = callingMethod.FormatCallingMethod();
        LogValue = logValue;
        LogType = logType;
        Time = DateTime.UtcNow;
    }
    
    public Log(LogTypeEnum logType, string callingClass, string callingMethod, object logValue)
    {
        CallingClass = callingClass.FormatCallingMethod();
        CallingMethod = callingMethod;
        LogType = logType;
        Time = DateTime.UtcNow;
        try
        {
            LogValue = JsonSerializer.Serialize(logValue, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        catch (System.Exception)
        {
            LogValue = logValue.ToString()!;
        }
    }

    public Log(LogRequest request)
    {
        CallingClass = request.LogClassInitializer;
        CallingMethod = request.LogCallingMethod;
        LogValue = request.LogJsonValue;
        LogType = (LogTypeEnum)request.LogType;
        Time = request.LogDate.ToDateTime();
    }
    
    /// <summary>
    /// Авто генерируемйы ключ в БД
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    /// <summary>
    /// Класс который вызвал логгирование
    /// </summary>
    public string CallingClass { get; private set; }
    
    /// <summary>
    /// Метод который вызывал логгирование
    /// </summary>
    public string CallingMethod { get; private set; }
    
    /// <summary>
    /// Тип лога
    /// </summary>
    public LogTypeEnum LogType { get; private set; }
    
    /// <summary>
    /// Время создания лога
    /// </summary>
    public DateTime Time { get; private set; }
    
    /// <summary>
    /// Содержание лога (в json)
    /// </summary>
    public string LogValue { get; private set; }
}