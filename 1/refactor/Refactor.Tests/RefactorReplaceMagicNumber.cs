using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Refactor.Tests {
    public class ReplaceMagicNumberTestDataGenerator : IEnumerable<object[]> {
        private readonly List<object[]> _data = new List<object[]> {
            // {number, name, input, expected}
            new object[] {"3.14", "PI", "double area = 3.14 * pow(r, 2);",
@"const double PI = 3.14;
double area = PI * pow(r, 2);"
            },
            new object[] {"777", "HEAVEN", "int number = 777+777*777;",
@"const int HEAVEN = 777;
int number = HEAVEN+HEAVEN*HEAVEN;"
            },
            new object[] {"10", "BUFFER_SIZE",
@"int array [10];
for(int i = 0; i < 10; i++) {
    array[i] = i;
}"
,
@"const int BUFFER_SIZE = 10;
int array [BUFFER_SIZE];
for(int i = 0; i < BUFFER_SIZE; i++) {
    array[i] = i;
}"
            }, 
            new object[] {"3.14", "PI", "double area = 3.14159 * pow(r, 2);", "double area = 3.14159 * pow(r, 2);"},
            new object[] {"3.14", "PI", "double area = 1113.14 * pow(r, 2);", "double area = 1113.14 * pow(r, 2);"},
            new object[] {"3.14", "PI", "double area = 1113.14159 * pow(r, 2);", "double area = 1113.14159 * pow(r, 2);"},
            new object[] {"3.14", "PI", "string s = \" circle area formula: 3.14 * pow(r, 2);\"", "string s = \" circle area formula: 3.14 * pow(r, 2);\""},
            new object[] {"3.14", "PI", "//circle area formula: 3.14 * pow(r, 2);", "//circle area formula: 3.14 * pow(r, 2);"},
            new object[] {"3.14", "PI", "/*circle area formula: 3.14 * pow(r, 2);*/", "/*circle area formula: 3.14 * pow(r, 2);*/"},
            new object[] {"777", "HEAVEN", "int number777 = func777(int parameter777);", "int number777 = func777(int parameter777);"},
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RefactorReplaceMagicNumberTest {
        [Theory]
        [ClassData(typeof(ReplaceMagicNumberTestDataGenerator))]
        public void ReplaceMagicNumberTest(string number, string name, string input, string expected) {
            var refactor = new Refactor();

            string output = refactor.ReplaceMagicNumber(number, name, input);
            Assert.Equal(expected, output);
        }
    }
}
