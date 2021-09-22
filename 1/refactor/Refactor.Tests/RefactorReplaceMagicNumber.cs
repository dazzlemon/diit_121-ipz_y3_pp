﻿using System;
using Xunit;

namespace Refactor.Tests
{
    public class RefactorReplaceMagicNumber
    {
        [Fact] //1
        public void Replace_SimplestCase()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "double area = 3.14 * pow(r, 2);");
            string expected = @"const double PI = 3,14;
                                double area = PI * pow(r, 2);";
            Assert.Equal(expected, result);
        }
       

        [Fact] //2
        public void Replace_FewTimesInLine()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("777", "HEAVEN", "int number = 777+777*777;");
            string expected = @"const int HEAVEN = 777;
                                int number = HEAVEN+HEAVEN*HEAVEN;";
            Assert.Equal(expected, result);
        }

        [Fact] //3
        public void Replace_ComplexCaseWithFormatting()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("1000000", "ONE_MILLION", "int million = 1'000'000;");
            string expected = @"const int ONE_MILLION = 1000000;
                                int million = ONE_MILLION;";
            Assert.Equal(expected, result);
        }

        [Fact] //4
        public void Replace_FewTimesInCode()
        {
            var refactor = new Refactor();
            string codeText =
                    @" 
                     int array [10];
                     for(int i = 0; i < 10; i++) 
                     {
                        array[i] = i;
                     }
                    ";
            string result = refactor.ReplaceMagicNumber("10", "BUFFER_SIZE", codeText);
            string expected =
                    @" 
                     int array [BUFFER_SIZE];
                     for(int i = 0; i < BUFFER_SIZE; i++) 
                     {
                        array[i] = i;
                     }
                    ";
            Assert.Equal(expected, result);
        }


        [Fact] //5
        public void Replace_ComplexCaseWhenConstExists()
        {
            var refactor = new Refactor();
            string codeText =
                    @" 
                     const double PI = 3.14159;
                     double area = 3.14 * pow(r, 2);
                     func(area);
                    ";
            string result = refactor.ReplaceMagicNumber("3.14", "PI", codeText);
            string expected =
                    @" 
                     const double PI = 3.14159;
                     const double PI1 = 3.14;
                     double area = PI1 * pow(r, 2);
                     func(area);
                     ";
            Assert.Equal(expected, result);
        }

        [Fact] //6
        public void NotReplace_SimplestCasePost()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "double area = 3.14159 * pow(r, 2);");
            string expected = @"double area = 3.14159 * pow(r, 2);";
            Assert.Equal(expected, result);
        }

        [Fact] //7
        public void NotReplace_SimplestCasePref()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "double area = 1113.14 * pow(r, 2);");
            string expected = @"double area = 1113.14 * pow(r, 2);";
            Assert.Equal(expected, result);

        }

        [Fact] //8
        public void NotReplace_SimplestCaseMid()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "double area = 1113.14159 * pow(r, 2);");
            string expected = "double area = 1113.14159 * pow(r, 2);";
            Assert.Equal(expected, result);

        }

        [Fact] //9
        public void NotReplace_NumberInString()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "string s = \" circle area formula: 3.14 * pow(r, 2);\"");
            string expected = "string s = \" circle area formula: 3.14 * pow(r, 2);\"";
            Assert.Equal(expected, result);
        }

        [Fact] //10
        public void NotReplace_NumberInSingleLineComment()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "//circle area formula: 3.14 * pow(r, 2);");
            string expected = "//circle area formula: 3.14 * pow(r, 2);";
            Assert.Equal(expected, result);
        }

        [Fact] //11
        public void NotReplace_NumberInMultiLineComment()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("3.14", "PI", "/*circle area formula: 3.14 * pow(r, 2);*/");
            string expected = "/*circle area formula: 3.14 * pow(r, 2);*/";
            Assert.Equal(expected, result);
        }

        [Fact] //12
        public void NotReplace_NumberInExistingName()
        {
            var refactor = new Refactor();
            string result = refactor.ReplaceMagicNumber("777", "HEAVEN", "int number777 = func777(int parameter777);");
            string expected = "int number777 = func777(int parameter777);";
            Assert.Equal(expected, result);
        }
    }
}