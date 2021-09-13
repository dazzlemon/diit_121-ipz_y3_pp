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
            string result = refactor.RenameMethod("s", "s", "s");

            Assert.True(result == "s", "result has to be 's'");
        }
    }
}
