using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Refactor.Tests {
    public class RefactorCommentsTest {
        [Fact]
        public void MultiCommentInSingle() {
            var refactor = new Refactor();

            var str = @"
// /*
// this is a comment
not a comment
// */
";

            Assert.Equal(refactor.Comments(str), new List<(int, int)>{
                (0, 4),
                (5, 26),
                (40, 44)
            });
        }
    }
}