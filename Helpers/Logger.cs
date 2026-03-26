using MelonLoader;

namespace LMUI;

/// <summary>
/// Basic wrapper for the MelonLogger instance.
/// </summary>
public class Logger
{
    public void LogInfo(string msg)
    {
        MelonLogger.Msg(msg);
    }

    /// <summary>
    /// Logs only if the condition is true.
    /// </summary>
    public void LogInfo(bool condition, string msg)
    {
        if (!condition) return;
        MelonLogger.Msg(msg);
    }

    public void LogWarning(string msg)
    {
        MelonLogger.Warning(msg);
    }

    /// <summary>
    /// Logs a warning only if the condition is true.
    /// </summary>
    public void LogWarning(bool condition, string msg)
    {
        if (!condition) return;
        MelonLogger.Warning(msg);
    }

    public void LogError(string msg)
    {
        MelonLogger.Error(msg);
    }

    /// <summary>
    /// Logs an error only if the condition is true.
    /// </summary>
    public void LogError(bool condition, string msg)
    {
        if (!condition) return;
        MelonLogger.Error(msg);
    }
}
