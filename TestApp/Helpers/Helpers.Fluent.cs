using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Fluent
{
    public class Test
    {
        public readonly Action Action;
        public readonly string Name;
        public Test(string name) { Name = name; }
        public Test(string name, Action action) { Name = name; Action = action; }
        public Check Check() { return new Check(this); }
        public override string ToString() { return Name; }
        public static Action Body(Action action) { return action; }
        public static Func<TResult> Body<TResult>(Func<TResult> function) { return function; }
        public static Test Case(string name, Action action) { return new Test(name, action); }
        public static Test<TResult> Case<TResult>(string name, Func<TResult> function) { return new Test<TResult>(name, function); }
    }

    public sealed class Test<TResult> : Test
    {
        public readonly Func<TResult> Function;
        public Test(string name, Func<TResult> function) : base(name) { Function = function; }
        new public TResult Check() { return Function(); }
        public Check<TResult> Check(Predicate<TResult> predicate) { return new Check<TResult>(this, predicate); }
    }

    public class Check
    {
        private readonly Test test;
        public Check(Test test) { this.test = test; }
        public override string ToString() { if (test.Action != null) test.Action(); return test.Name; }
    }

    public sealed class Check<TResult> : Check
    {
        public readonly Test<TResult> Test;
        public readonly Predicate<TResult> Predicate;
        public Check(Test<TResult> test, Predicate<TResult> predicate) : base(test) { Test = test; Predicate = predicate; }
        public override string ToString() { var value = Test.Check(); return String.Format("{0} ({1})", String.Format(Test.Name, value), Predicate(value)); }
    }
}