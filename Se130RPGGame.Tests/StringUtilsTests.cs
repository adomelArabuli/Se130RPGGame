using Se130RPGGame.Data.TestingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se130RPGGame.Tests
{
    public class StringUtilsTests
    {
        [Fact]
        public void ConcatStrings_WhenGivenTwoStrings_ReturnsConcatenation()
        {
            // arrange
            StringUtils stringUtils = new StringUtils();

            //act
            string result = stringUtils.ConcatString("Hello, ", "world");

            //assert
            Assert.Equal("Hello, world", result);
        }

        [Theory]
        [InlineData("radar", true)]
        [InlineData("hello", false)]
        [InlineData("121", true)]
        public void IsPalindrome_WhenGivenString_ReturnsExpectedResult(string str, bool expectedResult)
        {
            // arrange
            StringUtils stringUtils = new StringUtils();

            //act
            bool result = stringUtils.IsPalindrome(str);

            //assert
            Assert.Equal(expectedResult, result);
        }
    }
}
