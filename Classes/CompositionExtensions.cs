using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BerlinClock.Classes
{
    public static class CompositionExtensions
    {
        public static Func<T, TResult2> Then<T, TResult1, TResult2>( // Before.
            this Func<T, TResult1> function1, Func<TResult1, TResult2> function2) =>
            value => function2(function1(value));
    }
}
