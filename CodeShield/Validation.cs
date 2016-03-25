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

        public bool Success => this.ExceptionProvider != null;

        public Func<T, Exception> ExceptionProvider { get; private set; }

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

        public void ThrowOnError()
        {
            if (!this.Success) throw this.ExceptionProvider?.Invoke(this.Value) ?? new Exception();
        }

        public T GetValueOrThrow()
        {
            if (this.Success) return this.Value;

            throw this.ExceptionProvider?.Invoke(this.Value) ?? new Exception();
        }

        public static implicit operator bool(Validation<T> result)
        {
            return result.Success;
        }
    }
}
