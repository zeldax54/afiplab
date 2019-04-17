using System;
using System.Diagnostics;
using CUbaBuscaApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestExtractPrecio()
        {
            Trace.WriteLine(Helper.FindBetweenChars("televisor plano (80-250)"));

        }

        public void TestSplit()
        {
            Trace.WriteLine(Helper.FindBetweenChars("televisor plano (80-250)"));

        }
    }
}
