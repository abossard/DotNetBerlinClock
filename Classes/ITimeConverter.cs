namespace BerlinClock.Classes
{
    /// <summary>
    /// Converts an string based time input into a berlin clock string.
    ///
    /// E.g. 23:59:59 results into
    /// O
    /// RRRR
    /// RRRO
    /// YYRYYRYYRYY
    /// YYYY
    ///
    /// More information is available here: https://en.wikipedia.org/wiki/Mengenlehreuhr
    /// </summary>
    public interface ITimeConverter
    {
        string ConvertTime(string aTime);
    }
}