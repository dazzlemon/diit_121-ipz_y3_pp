using System;
using Xunit;

namespace Refactor.Tests
{
    public class RefactorRenameMethodTest
    {
        [Fact]
        public void Test1()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "void func();");
            string expected = "void method();";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "string s = \" void func();\"");
            string expected = "string s = \" void func();\"";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test3()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "//void func();");
            string expected = "//void func();";
            Assert.Equal(expected, result);

        }


    }
}
