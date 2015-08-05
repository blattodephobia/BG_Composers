using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class DebugCheck
    {
		internal static readonly Func<Exception> DefaultExceptionProvider = () => new Exception("An unknown error occurred in the application");

		public static void IsTrue(bool condition, Func<Exception> exceptionProvider = null)
		{
			if (!condition)
			{
				throw (exceptionProvider ?? DefaultExceptionProvider).Invoke();
			}
		}
		public static void IsFalse(bool condition, Func<Exception> exceptionProvider = null)
		{
			IsTrue(!condition, exceptionProvider);
		}
    }
}
