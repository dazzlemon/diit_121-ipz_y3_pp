using System;
using Xunit;

namespace Refactor.Tests
{
    public class RefactorRenameMethodTest
    {
        [Fact] //1
        public void Rename_SimplestCase()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "void func();");
            string expected = "void method();";
            Assert.Equal(expected, result);
        }

        [Fact] //2
        public void Rename_CaseWithSpacesBetweenBrackets()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "void func     ();");
            string expected = "void method     ();";
            Assert.Equal(expected, result);
        }

        [Fact] //3
        public void Simplest_CaseWithParameter()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "int func(int parameter);");
            string expected = "int method(int parameter);";
            Assert.Equal(expected, result);

        }

        [Fact] //4
        public void NotRename_MethodNameInString()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "string s = \" void func();\"");
            string expected = "string s = \" void func();\"";
            Assert.Equal(expected, result);
        }

        [Fact] //5
        public void NotRename_MethodNameAsVariableName()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "int func = 0;");
            string expected = "int func = 0;";
            Assert.Equal(expected, result);
        }

        [Fact] //6
        public void NotRename_MethodNameInSingleLineComment()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "//void func();");
            string expected = "//void func();";
            Assert.Equal(expected, result);

        }

        [Fact] //7
        public void NotRename_MethodNameInMultiLineComment()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "/*void func();*/");
            string expected = "/*void func();*/";
            Assert.Equal(expected, result);

        }

        [Fact] //8
        public void NotRename_FamiliarMethodNamePost()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "void func2();");
            string expected = "void func2();";
            Assert.Equal(expected, result);
        }

        [Fact] //9
        public void NotRename_FamiliarMethodNamePref()
        {
            var refactor = new Refactor();
            string result = refactor.RenameMethod("func", "method", "void some_func();");
            string expected = "void some_func();";
            Assert.Equal(expected, result);

        }

        [Fact] //10
        public void Rename_MethodNameInDifferentObjects()
        {
            var refactor = new Refactor();
            string codeText =
                   @" 
                      className1 objectName1;
                      className2 objectName2;
                      objectName1.func();
                      objectName2.func();
                    ";
            string result = refactor.RenameMethod("func", "method", CodeText);
            string expected =
                   @" 
                      className1 objectName1;
                      className2 objectName2;
                      objectName1.method();
                      objectName2.method();
                    ";
            Assert.Equal(expected, result);
        }

        [Fact] //11
        public void Rename_MethodNameInPointerToFunction()
        {
            var refactor = new Refactor();
            string CodeText =
                   @"
                    void func ( void (*f)(int) );
                    void func2 ( void (*func)(int) );
                    ";
            string result = refactor.RenameMethod("func", "method", CodeText);
            string expected =
                   @" 
                    void method ( void (*f)(int) );
                    void func2 ( void (*method)(int) );
                    ";
            Assert.Equal(expected, result);
        }

        [Fact] //12
        public void Rename_ComplexCase1()
        {
            var refactor = new Refactor();
            string CodeText =
                   @"
                      //void func(int parameter) some text
                      void func(int parameter)
                      {
                      std::cout << ""void func() has "" << parameter << std::endl;
                      }
                    ";
            string result = refactor.RenameMethod("func", "method", CodeText);
            string expected =
                   @" 
                      //void func(int parameter) some text
                      void method(int parameter)
                      {
                      std::cout << ""void func() has "" << parameter << std::endl;
                      }
                    ";
            Assert.Equal(expected, result);
        }

        [Fact] //13
        public void Rename_ComplexCase2()
        {
            var refactor = new Refactor();
            string CodeText =
                   @" 
                      //void func(int parameter) some text
                      void func2(int parameter)
                      {
                      className objectName;
                      int randomNumber1 = func();
                      int randomNumber2 = objectName.func();
                      }
                    ";
            string result = refactor.RenameMethod("func", "method", CodeText);
            string expected =
                   @" 
                      //void func(int parameter) some text
                      void func2(int parameter)
                      {
                      className objectName;
                      int randomNumber1 = method();
                      int randomNumber2 = objectName.method();
                      }
                    ";
            Assert.Equal(expected, result);
        }
    }
}
