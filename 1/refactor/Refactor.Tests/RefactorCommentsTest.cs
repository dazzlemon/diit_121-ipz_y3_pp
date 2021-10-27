using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Refactor.Tests {
    public class RefactorAdditionalTest {
        [Fact]
        public void MultiCommentInSingle() {
            var refactor = new Refactor();

            var str = @"
// /*
// this is a comment
not a comment
// */
";
            var expected = new List<(int, int)>{
                (2, 8),
                (9, 30),
                (46, 52)
            };

            Assert.Equal(expected, refactor.Comments(str));
        }


        [Fact]
        public void QuotesTest() {
            var refactor = new Refactor();

             var str = @"
// ""this is a comment ""
""not a comment""
";
            var expected = new List<(int, int)>{
                (5, 24),
                (27, 41),
            };

            Assert.Equal(expected, refactor.Quotes(str));
        }

    }
}