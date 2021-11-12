// <copyright file="ConfigurationScanner.cs" company="Paulo Neves">
// Copyright (c) Paulo Neves. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace ConfigurationScanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Extension class for IConfigurationRoot scanning.
    /// See <see cref="ThrowOnForbiddenToken"/> and <see cref="ThrowOnConfiguredForbiddenToken"/>.
    /// The goal of this library is to provide a simple way to ensure no "forbidden" values are found in the
    /// configuration. The reason i personally use this library is that I have properties on the my appSettings which have
    /// placeholder values that must be overriden by environmental variables, otherwise the program cannot run.
    /// This library makes sure the forbidden values/placeholders are not in the final configuration.
    /// </summary>
    public static class ConfigurationScanner
    {
        /// <summary>
        /// The hard coded name of the section used to look for the forbidden token. Can be read to programatically inject a forbidden token into
        /// an existing configuration.
        /// Value: "ConfigurationScanner".
        /// </summary>
        public const string ClassSectionName = "ConfigurationScanner";

        /// <summary>
        /// The hard coded name of the Configuration Token Key inside <see cref="ClassSectionName"/> section. Can be read to programatically inject
        /// a forbidden token into an existing configuration.
        /// Value: "Token".
        /// </summary>
        public const string ConfigurationTokenKey = "Token";

        /// <summary>
        /// Extension method that throws an exception when a given token is found on a value of the configuration root.
        /// A configuration provider may have a property
        /// of a secret set to a placeholder that another configuration provider is supposed to overwrite. If the overwrite
        /// is not done this extension will throw an exception.
        /// This is useful for example in the case where the appSettings json file has a structure where some properties
        /// are secrets to be injected through other means like for example environmental variables. If the environmental
        /// variables are not injected and thus not override the placeholders, the method will throw an exception and give
        /// the program the opportunity to handle such situation.
        /// </summary>
        /// <param name="conf">The configuration root to be scanned.</param>
        /// <param name="forbiddenToken">The token that, if found will throw an exception "T:ForbiddenValueConfigurationException".</param>
        /// <returns>The unmodified <see cref="IConfigurationRoot"/> if no forbidden token exists.</returns>
        /// <exception cref="ForbiddenValueConfigurationException">
        ///         Thrown when a forbidden token is found in the configuration.
        /// </exception>
        public static IConfigurationRoot ThrowOnForbiddenToken(this IConfigurationRoot conf, string forbiddenToken, bool ignoreSelfConfiguration = false)
        {
            var foundValues = new List<KeyValuePair<string, string>>();
            void RecurseChildren(IEnumerable<IConfigurationSection> children)
            {
                foreach (var child in children)
                {
                    var (value, provider) = GetValueAndProvider(conf, child.Path);
                    if (provider != null
                        && value == forbiddenToken
                        && (
                            (!ignoreSelfConfiguration && child.Path != $"{ClassSectionName}:{ConfigurationTokenKey}")
                            || ignoreSelfConfiguration)
                        )
                    {
                            foundValues.Add(new KeyValuePair<string, string>(child.Key, child.Value));
                    }

                    RecurseChildren(child.GetChildren());
                }
            }

            RecurseChildren(conf.GetChildren());

            if (foundValues.Any())
            {
                throw new ForbiddenValueConfigurationException(foundValues);
            }

            return conf;
        }

        /// <summary>
        /// Has the same function as as <see cref="ThrowOnForbiddenToken"/> but the token is actually read from the configuration itself.
        /// For this method to work there needs to be a section with the name equal to <see cref="ClassSectionName" />.
        /// It also needs to have the key with the value equal to <see cref="ConfigurationTokenKey"/>.
        /// The actual value of the mentioned key contains the token that will be taken as forbidden.
        /// </summary>
        /// <param name="conf">The configuration root to be scanned.</param>
        /// <returns>The unmodified <see cref="IConfigurationRoot"/> if no forbidden token exists.</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when the forbidden token cannot be found in the configuration root.
        /// </exception>
        public static IConfigurationRoot ThrowOnConfiguredForbiddenToken(this IConfigurationRoot conf)
        {
            var section = conf
                .GetSection(ClassSectionName)
                .GetChildren()
                .SingleOrDefault(c => c.Key == ConfigurationTokenKey);

            if (section == null)
            {
                throw new ArgumentException("Could not find the forbidden token by scanning through section "
                    + $"{ClassSectionName} key ${ConfigurationTokenKey}");
            }

            return conf.ThrowOnForbiddenToken(section.Value);
        }

        private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(
            IConfigurationRoot root,
            string key)
        {
            foreach (var provider in root.Providers.Reverse())
            {
                if (provider.TryGet(key, out var value))
                {
                    return (value, provider);
                }
            }

            return (null, null);
        }
    }
}