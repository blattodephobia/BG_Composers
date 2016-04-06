using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShield
{
    public struct Validation<T>
    {
        public T Value { get; private set; }

        public bool Success => this.ExceptionProvider == null;

        public Func<T, Exception> ExceptionProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Validation{T}"/> struct using <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        public Validation(T value) :
            this()
        {
            this.Value = value;
        }

        public Validation(T value, Func<T, Exception> exceptionProvider)
        {
            this.Value = value;
            this.ExceptionProvider = exceptionProvider;
        }

        /// <summary>
        /// Throws the exception returned by the <see cref="ExceptionProvider"/> if the <see cref="Success"/> property
        /// is set to false.
        /// </summary>
        public void ThrowOnError()
        {
            if (!this.Success) throw this.ExceptionProvider?.Invoke(this.Value) ?? new Exception();
        }

        /// <summary>
        /// Returns the <see cref="Value"/> property if <see cref="Success"/> is set to true. Otherwise,
        /// an exception is retrieved via the <see cref="ExceptionProvider"/> and it is thrown.
        /// </summary>
        /// <returns>The <see cref="Value"/> property.</returns>
        public T GetValueOrThrow()
        {
            if (this.Success) return this.Value;

            throw this.ExceptionProvider?.Invoke(this.Value) ?? new Exception();
        }

        /// <summary>
        /// Returns a new validation result whose value is the current one's and whose <see cref="Success"/>
        /// property 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Validation<T> And(Validation<T> other)
        {
            if (this.Success && other.Success)
            {
                return new Validation<T>(this.Value);
            }
            else
            {
                Func<T, Exception> exceptionProvider = this.Success
                    ? other.ExceptionProvider
                    : this.ExceptionProvider;
                return new Validation<T>(this.Value, exceptionProvider);
            }
        }

        public static implicit operator bool(Validation<T> result)
        {
            return result.Success;
        }
    }
}
