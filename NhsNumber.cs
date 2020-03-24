using System.Linq;

namespace NhsNumber
{
    public class NhsNumber
    {
        public string Raw { get; }

        public string WithoutSeparators { get; }

        public string Formatted { get; }

        public bool IsValid { get; }

        public NhsNumber(string raw, char? imputSeparator = ' ', char outputSeparator = ' ')
        {
            Raw = raw;
            WithoutSeparators = RemoveSeparators(raw, imputSeparator);
            Formatted = Format(WithoutSeparators, outputSeparator);
            IsValid = Validate(WithoutSeparators);
        }

        private static string Format(string withoutSeparators, char separator)
        {
            // We can only apply 3 3 4 formatting if we have 10 digits.
            if (withoutSeparators.Length != 10 || !withoutSeparators.Any(char.IsDigit))
            {
                return withoutSeparators;
            }

            return withoutSeparators.Substring(0, 3) + separator +
                   withoutSeparators.Substring(3, 3) + separator +
                   withoutSeparators.Substring(6, 4);
        }

        private static string RemoveSeparators(string value, char? separator)
        {
            // We can only remove separators if the supplied value was in 3 3 4 format.
            if (value.Length != 12)
            {
                return value;
            }

            // Auto-detect separator if null was supplied.
            if (separator == null)
            {
                separator = value[3];
            }

            // Check both separators match.
            if (value[3] != separator || value[7] != separator)
            {
                return value;
            }

            // Remove separators.
            string withoutSeparators = value.Substring(0, 3) + value.Substring(4, 3) + value.Substring(8, 4);

            // If some of the remaining chars are not digits, we don't have a proper NHS Number.
            if (!withoutSeparators.All(char.IsDigit))
            {
                return value;
            }

            // We've successfully removed the separators.
            return withoutSeparators;
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
