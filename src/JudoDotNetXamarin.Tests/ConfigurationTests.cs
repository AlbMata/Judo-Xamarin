using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace JudoDotNetXamarin.Tests
{
    [TestFixture]
    public class ConfigurationTests : JudoTestBase
    {
        public static IEnumerable<object[]> GetConfigurationTests()
        {
            yield return new object[] { string.Empty, "MYTOKEN", "MYSECRET" };
            yield return new object[] { null, "MYTOKEN", "MYSECRET" };
            yield return new object[] { "MYJUDOID", string.Empty, "MYSECRET" };
            yield return new object[] { "MYJUDOID", null, "MYSECRET" };
            yield return new object[] { "MYJUDOID", "MYTOKEN", string.Empty };
            yield return new object[] { "MYJUDOID", "MYTOKEN", null };
        }

        [Test, TestCaseSource("GetConfigurationTests")]
        public void When_Configuration_Is_Not_Valid_Validation_Must_Fail_And_Throw_Exception(string judoId, string token, string secret)
        {
            var sut = GetConfiguration();
            sut.ApiToken = token;
            sut.ApiSecret = secret;
            sut.JudoId = judoId;
            Assert.Throws<Exception>(() => sut.Validate());
        }
    }
}