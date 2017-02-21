namespace SimpleLogger.Logging.Formatters
{
    internal class DefaultLoggerFormatter : ILoggerFormatter
    {
        public string ApplyFormat(LogMessage logMessage)
        {
            return string.Format("{0:dd.MM.yyyy HH:mm:ss}: {1} [ThreadID: {2}] [line: {3} {4} -> {5}()]: {6}",
                            logMessage.DateTime, logMessage.Level, logMessage.ThreadID, logMessage.LineNumber, logMessage.CallingClass,
                            logMessage.CallingMethod, logMessage.Text);
        }
    }
}