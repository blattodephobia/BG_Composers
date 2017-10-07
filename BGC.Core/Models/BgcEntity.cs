using CodeShield;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class BgcEntity<TKey>
        where TKey : struct
    {
        private static readonly Dictionary<IStructuralEquatable, IEnumerable<ValidationAttribute>> MemberValidationRulesMap =
            new Dictionary<IStructuralEquatable, IEnumerable<ValidationAttribute>>();

        private static IEnumerable<ValidationAttribute> GetValidationAttirbutes(Type declaringType, string propertyOrFieldName)
        {
            var validationRules = declaringType.GetMember(propertyOrFieldName).FirstOrDefault()?.GetCustomAttributes<ValidationAttribute>();
            return validationRules ?? Enumerable.Empty<ValidationAttribute>();
        }

        private IEnumerable<ValidationAttribute> GetValidationRules(string memberName, Tuple<string, int, string> memberId)
        {
            if (!MemberValidationRulesMap.ContainsKey(memberId))
            {
                BindingFlags searchFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                IEnumerable<ValidationAttribute> validationRules =
                    GetType()
                    .GetMember(memberName, searchFlags)
                    .FirstOrDefault()
                    ?.GetCustomAttributes<ValidationAttribute>();
                MemberValidationRulesMap.Add(memberId, validationRules);
            }

            return MemberValidationRulesMap[memberId];
        }

        /// <summary>
        /// Validates the value in a property's setter based on the <see cref="ValidationAttribute"/>s it has been decorated with.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberName"></param>
        /// <param name="lineNumber"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected T EnsureValid<T>(T value, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            var memberId = Tuple.Create(memberName, lineNumber, filePath);
            IEnumerable<ValidationAttribute> failingRules = GetValidationRules(memberName, memberId).Where(a => !a.IsValid(value));
            Validation<T> validationResult = Shield.Assert(value, !failingRules.Any(), (propertyValue) =>
            {
                ValidationException exception = failingRules.Aggregate(
                    seed: new ValidationException($"Validation failed for member {memberName}. See the {nameof(ValidationException.Data)} property for more information"),
                    func: (seed, currentAttirbute) =>
                    {
                        seed.Data.Add(currentAttirbute.GetType().FullName, currentAttirbute.FormatErrorMessage(memberName));
                        return seed;
                    },
                    resultSelector: seed => seed);

                return exception;
            });

            return validationResult.GetValueOrThrow();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
	}
}
