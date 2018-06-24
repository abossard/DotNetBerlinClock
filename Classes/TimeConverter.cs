using System;
using System.Linq;

namespace BerlinClock.Classes
{
    public class TimeConverter : ITimeConverter
    {
        private const char Yellow = 'Y';

        /// <summary>
        ///     Denotes the defaultCharacter with which remaining positions are filled
        /// </summary>
        public const char Filler = 'O';

        private const char Red = 'R';

        private static readonly Func<int, string> FormatHours = CreateFormatFunc(4, Red);

        private static readonly Func<int, string> FormatMinutes =
            CreateFormatFunc(11, Yellow, index => (index + 1) % 3 == 0);

        private static readonly Func<int, string> FormatMinutes1 = CreateFormatFunc(4, Yellow);

        /// <summary>
        ///     Verifies that the numeric boundaries are within realistic values
        /// </summary>
        public static readonly Func<(int hours, int minutes, int seconds), (int hours, int minutes, int seconds)>
            VerifyTimeTuple = timeTuple =>
            {
                if (timeTuple.hours < 0 || timeTuple.hours > 24)
                    throw new ArgumentException($"Hours should be within 0 and 24, it was ${timeTuple.hours}");
                if (timeTuple.minutes < 0 || timeTuple.minutes > 59)
                    throw new ArgumentException($"Minutes should be within 0 and 59, it was ${timeTuple.minutes}");
                if (timeTuple.seconds < 0 || timeTuple.seconds > 59)
                    throw new ArgumentException($"Seconds should be within 0 and 59, it was ${timeTuple.seconds}");
                return timeTuple;
            };

        /// <summary>
        ///     Converts an input like: 23:59:59 into a named tuple (hours, minutes, seconds)
        /// </summary>
        /// <returns>time as tuple</returns>
        public static readonly Func<string, (int hours, int minutes, int seconds)> ParseTimeFormat = aTime =>
        {
            try
            {
                var parts = aTime.Split(':').Select(x => Convert.ToInt16(x)).ToArray();
                if (parts.Length != 3)
                    throw new ArgumentException(
                        $"Input needs to contain three numbers divided by a colon. E.g. 10:11:12. Given Input: ${aTime}");

                return (
                    hours: parts[0],
                    minutes: parts[1],
                    seconds: parts[2]
                );
            }
            catch (Exception exception)
            {
                if (exception is FormatException || exception is OverflowException)
                    throw new ArgumentException(
                        $"Input needs to contain three numbers divided by a colon. E.g. 10:11:12. Given Input: ${aTime}",
                        exception
                    );
                throw;
            }
        };

        /// <summary>
        ///     Converts time as (hours, minutes, seconds) into a berlin clock tuple (hours5, hours1, minutes5, minutes1,
        ///     secondsEven)
        ///     Explanations of the berlin touple fields:
        ///     hours5/1: how many 5/1-hour segments should be lit
        ///     minutes5/1: how many 5/1-minutes segments should be lit
        ///     secondsEven: whether the seconds are even
        /// </summary>
        /// <returns>a Berlin clock tuple</returns>
        public static readonly
            Func<(int hours, int minutes, int seconds), (int hours5, int hours1, int minutes5, int minutes1, bool
                secondsEven)> ConvertTimeTupeToBerlinTuple = timeTuple => (
                hours5: timeTuple.hours / 5,
                hours1: timeTuple.hours % 5,
                minutes5: timeTuple.minutes / 5,
                minutes1: timeTuple.minutes % 5,
                secondsEven: timeTuple.seconds % 2 == 0
            );

        private static readonly Func<(int hours5, int hours1, int minutes5, int minutes1, bool secondsEven), string>
            FormatBerlinTuple = berlinTuple => string.Join("\r\n",
                berlinTuple.secondsEven ? Yellow : Filler,
                FormatHours(berlinTuple.hours5),
                FormatHours(berlinTuple.hours1),
                FormatMinutes(berlinTuple.minutes5),
                FormatMinutes1(berlinTuple.minutes1)
            );

        public string ConvertTime(string aTime)
        {
            var composition = ParseTimeFormat
                .Then(VerifyTimeTuple)
                .Then(ConvertTimeTupeToBerlinTuple)
                .Then(FormatBerlinTuple);
            return composition(aTime);
        }

        /// <summary>
        ///     Create a Function the format a string to a certain length with an options Functions to tranpose characters at
        ///     certain positions.
        ///     e.g. var f = CreateFormatFunc(5, 'R', index => index == 2, 'X')
        ///     f(3) would result into "RRXOO"
        /// </summary>
        /// <param name="positions">length of the output string</param>
        /// <param name="defaultCharacter">to fill up from the start, the rest is filled with TimeConverter.Filler</param>
        /// <param name="transposeOnIndex">if true, the transposeCharacter is used instead of defaultCharacter</param>
        /// <param name="transposeCharacter">used in case of the defaultCharacter if transposeOnIndex is true</param>
        /// <returns>the formatted string</returns>
        public static Func<int, string> CreateFormatFunc(int positions, char defaultCharacter,
            Func<int, bool> transposeOnIndex = null,
            char transposeCharacter = Red)
        {
            return count =>
            {
                var characterArray = Enumerable.Range(0, count)
                    .Select(index =>
                        transposeOnIndex != null && transposeOnIndex(index) ? transposeCharacter : defaultCharacter)
                    .ToArray();
                var filler = new string(Filler, positions - count);
                return new string(characterArray) + filler;
            };
        }
    }
}