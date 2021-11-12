// <copyright file="UnitTest.cs" company="Paulo Neves">
// Copyright (c) Paulo Neves. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ConfigurationScanner;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="ConfigurationScanner"/>.
    /// </summary>
    public class UnitTest
    {
        private const string Token = "@@TOKEN@@";

        private static readonly KeyValuePair<string, string> PasswordKv = new KeyValuePair<string, string>("password", Token);
        private static readonly KeyValuePair<string, string> ExtraKv = new KeyValuePair<string, string>("extra_thing", Token);

        private static Dictionary<string, string> NormalConfiguration => new Dictionary<string, string>
        {
            { "username", "Guest" },
            { "greeting", "hello" },
        };

        private static Dictionary<string, string> ConfigurationScannerConf => new Dictionary<string, string>
        {
            {
                $"{ConfigurationScanner.ClassSectionName}:{ConfigurationScanner.ConfigurationTokenKey}", "@@TOKEN@@"
            },
        };

        /// <summary>
        /// Test that we throw a <see cref="ArgumentException"/> exception when we expect a configured forbidden
        /// token but no such configuration can be found.
        /// </summary>
        [Fact]
        public void TestThrowOnConfiguredForbiddenTokenWithNoConfThrows()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new ConfigurationBuilder()
                    .AddInMemoryCollection(NormalConfiguration)
                    .Build()
                    .ThrowOnConfiguredForbiddenToken();
            });
        }

        /// <summary>
        /// We want to make sure that <see cref="ConfigurationScanner.ThrowOnForbiddenToken"/>> does not throw when
        /// no forbidden token is found.
        /// </summary>
        [Fact]
        public void TestNoTokenDoesNotThrow()
        {
            var r = Record.Exception(() =>
            {
                new ConfigurationBuilder()
                    .AddInMemoryCollection(NormalConfiguration)
                    .Build()
                    .ThrowOnForbiddenToken(Token);
            });
            Assert.Null(r);
        }

        /// <summary>
        /// Test that a configuration defining the forbidden token is not itself caught as a violation.
        /// </summary>
        [Fact]
        public void TestNoTokenWithConfDoesNotThrow()
        {
            var r = Record.Exception(() =>
            {
                new ConfigurationBuilder()
                    .AddInMemoryCollection(NormalConfiguration)
                    .AddInMemoryCollection(ConfigurationScannerConf)
                    .Build()
                    .ThrowOnForbiddenToken(Token);
            });
            Assert.Null(r);
        }

        /// <summary>
        /// Test that if we do not ignore our own configuration section we will detect it as a fault.
        /// </summary>
        [Fact]
        public void TestWithConfThrowsWhenNotIgnoreConf()
        {
            var r = Assert.Throws<ForbiddenValueConfigurationException>(() =>
            {
                new ConfigurationBuilder()
                    .AddInMemoryCollection(NormalConfiguration)
                    .AddInMemoryCollection(ConfigurationScannerConf)
                    .Build()
                    .ThrowOnForbiddenToken(Token, true);
            });
            Assert.Single(r.FoundValues);
        }

        /// <summary>
        /// This tests whether the scan does not when no forbidden values are in the configuration.
        /// It also check that the return value is the same as the original <see cref="IConfigurationRoot"/>
        /// passed to us.
        /// </summary>
        [Fact]
        public void TestReturnsIdentity()
        {
            var c = new ConfigurationBuilder()
                .AddInMemoryCollection(NormalConfiguration)
                .Build();
            Assert.Equal(c, c.ThrowOnForbiddenToken(Token));
        }

        /// <summary>
        /// This is the most complex test which takes the forbidden token from the ConfigurationRoot itself.
        /// This test uses a test-configuration.json configuration file that has a more complex structure as well.
        /// This test also illustrates the usage of the const fields <see cref="ConfigurationScanner.ClassSectionName"/> and
        /// <see cref="ConfigurationScanner.ConfigurationTokenKey"/> to inject configuration for
        /// <see cref="ConfigurationScanner.ThrowOnConfiguredForbiddenToken"/>, even though this is not really necessary for our test, it
        /// serves as an illustration that you can do it.
        /// </summary>
        [Fact]
        public void TestThrowOnConfiguredForbiddenTokenWorks()
        {
            var threw = false;
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("Tests.assets.test-configuration.json");
            try
            {
                new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .AddInMemoryCollection(ConfigurationScannerConf)
                    .Build()
                    .ThrowOnConfiguredForbiddenToken();
            }
            catch (ForbiddenValueConfigurationException e)
            {
                Assert.Contains(PasswordKv, e.FoundValues);
                Assert.Contains(ExtraKv, e.FoundValues);
                Assert.Equal(2, e.FoundValues.Count);
                threw = true;
            }

            Assert.True(threw);
        }
    }
}