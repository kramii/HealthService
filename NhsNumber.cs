using System.Linq;

namespace NhsNumber
{
    public class NhsNumber
    {
        public string Raw { get; }

        public string WithoutSeparators { get; }

        public string Formatted { get; }

        public bool IsValid { get; }

        public NhsNumber(string raw)
        {
            Raw = raw;
            WithoutSeparators = RemoveSeparators(raw);
            Formatted = Format(WithoutSeparators);
            IsValid = Validate(WithoutSeparators);
        }

        private static string Format(string withoutSeparators)
        {
            if (withoutSeparators.Length != 10)
            {
                return withoutSeparators;
            }

            return withoutSeparators.Substring(0, 3) + " " + withoutSeparators.Substring(3, 3) + " " +
                   withoutSeparators.Substring(6, 4);
        }

        private static string RemoveSeparators(string raw, char? separator = null)
        {
            switch (raw.Length)
            {
                case 10:

                    return raw;

                case 12:

                    // Auto-detect separator if none was supplied.
                    if (separator == null)
                    {
                        separator = raw[3];
                    }

                    // Check separators match.
                    if (raw[3] != separator || raw[7] != separator)
                    {
                        return raw;
                    }

                    // Remove separators.
                    string withoutSeparators = raw.Substring(0, 3) + raw.Substring(4, 3) + raw.Substring(8, 4);

                    return withoutSeparators.All(char.IsDigit)
                        ? withoutSeparators
                        : raw;

                default:
                    return raw;
            }
        }

        private static bool Validate(string nhsNumberWithoutSeparators)
        {
            if (nhsNumberWithoutSeparators.Length != 10)
            {
                return false;
            }

            var runningTotal = 0;

            // Calc check digit from first 9 digits.
            for (var index = 1; index <= 9; index++)
            {
                char c = nhsNumberWithoutSeparators[index - 1];

                // All characters must be a digit.
                if (!char.IsDigit(c))
                {
                    return false;
                }

                var digit = (int)char.GetNumericValue(c);
                int factor = 11 - index;

                runningTotal += digit * factor;
            }

            int expectedCheckDigit = 11 - runningTotal % 11;

            var actualCheckDigit = (int)char.GetNumericValue(nhsNumberWithoutSeparators[9]);

            return actualCheckDigit == expectedCheckDigit;
        }
    }
}
