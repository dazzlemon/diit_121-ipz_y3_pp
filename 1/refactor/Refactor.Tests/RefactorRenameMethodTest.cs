using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Refactor.Tests {
    public class TestDataGenerator : IEnumerable<object[]> {
        private readonly List<object[]> _data = new List<object[]> {
            //{oldName, newName, input, expected}// TODO: put in class?
            new object[] {"func", "method", "void func();", "void method();"},
            new object[] {"func", "method", "void func ();", "void method ();"},
            new object[] {"func", "method", "int func(int parameter);", "int method(int parameter);"},
            new object[] {"func", "method", "string s = \" void func();\"", "string s = \" void func();\""},
            new object[] {"func", "method", "int func = 0;", "int func = 0;"},
            new object[] {"func", "method", "//void func();", "//void func();"},
            new object[] {"func", "method", "/*void func();*/", "/*void func();*/"},
            new object[] {"func", "method", "void func2();", "void func2();"},
            new object[] {"func", "method", "void some_func();", "void some_func();"},
            new object[] {"func", "method",
@"className1 objectName1;
className2 objectName2;
objectName1.func();
objectName2.func();"
,
@"className1 objectName1;
className2 objectName2;
objectName1.method();
objectName2.method();"
            },
            new object[] {"func", "method",
@"void func ( void (*f)(int) );
void func2 ( void (*func)(int) );"
,
@"void method ( void (*f)(int) );
void func2 ( void (*method)(int) );"
            },
            new object[] {"func", "method",
@"// void func(int parameter) some text
void func(int parameter) {
    std::cout << ""void func() has "" << parameter << std::endl;
}"
,
@"// void func(int parameter) some text
void method(int parameter) {
    std::cout << ""void func() has "" << parameter << std::endl;
}"
            },
            new object[] {"func", "method",
@"// void func(int parameter) some text
void func2(int parameter) {
    className objectName;
    int randomNumber1 = func();
    int randomNumber2 = objectName.func();
}"
,
@"// void func(int parameter) some text
void func2(int parameter) {
    className objectName;
    int randomNumber1 = method();
    int randomNumber2 = objectName.method();
}"
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RefactorRenameMethodTest {
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void RenameMethodTest(string oldName, string newName, string input, string expected) {
            var refactor = new Refactor();

            string output = refactor.RenameMethod(oldName, newName, input);
            Assert.Equal(output, expected);
        }
    }
}
