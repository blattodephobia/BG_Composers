using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class NumericExtensions
    {
        private const int ORDER_MAX_VALUE = 3998;

        /// <summary>
        /// Returns the representation of <paramref name="number"/> in Roman numerals.
        /// </summary>
        /// <param name="number">The number to generate the Roman numeral for. Must be between 0 and 3998 inclusively.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> was outside the range [0; 3998].</exception>
        /// <returns></returns>
        public static string ToRomanNumeral(this int number)
        {
            Shield.Assert(
                    value: number,
                    condition: (0 <= number) && (number <= ORDER_MAX_VALUE),
                    exceptionProvider: (x) => new ArgumentOutOfRangeException(nameof(number), $"{nameof(number)} must be betwheen 0 and {ORDER_MAX_VALUE}"))
                  .ThrowOnError();
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRomanNumeral(number - 1000);
            if (number >= 900) return "CM" + ToRomanNumeral(number - 900);
            if (number >= 500) return "D" + ToRomanNumeral(number - 500);
            if (number >= 400) return "CD" + ToRomanNumeral(number - 400);
            if (number >= 100) return "C" + ToRomanNumeral(number - 100);
            if (number >= 90) return "XC" + ToRomanNumeral(number - 90);
            if (number >= 50) return "L" + ToRomanNumeral(number - 50);
            if (number >= 40) return "XL" + ToRomanNumeral(number - 40);
            if (number >= 10) return "X" + ToRomanNumeral(number - 10);
            if (number >= 9) return "IX" + ToRomanNumeral(number - 9);
            if (number >= 5) return "V" + ToRomanNumeral(number - 5);
            if (number >= 4) return "IV" + ToRomanNumeral(number - 4);
            if (number >= 1) return "I" + ToRomanNumeral(number - 1);
            throw new Exception("something bad happened");
        }
    }
}
