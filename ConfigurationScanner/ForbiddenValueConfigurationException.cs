// <copyright file="ForbiddenValueConfigurationException.cs" company="Paulo Neves">
// Copyright (c) Paulo Neves. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace ConfigurationScanner
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Exception class that informs on when forbidden values were found on the configuration by
    /// <see cref="ConfigurationScanner.ThrowOnForbiddenToken"/> or <see cref="ConfigurationScanner.ThrowOnConfiguredForbiddenToken"/>.
    /// </summary>
    public class ForbiddenValueConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenValueConfigurationException"/> class.
        /// </summary>
        /// <param name="foundValues">A list with the keys and values of the found forbidden properties found
        /// in the configuration.</param>
        public ForbiddenValueConfigurationException(List<KeyValuePair<string, string>> foundValues)
            : base(GenerateMessage(foundValues)) => this.FoundValues = foundValues;

        /// <summary>
        /// Gets list with the <seealso cref="KeyValuePair"/> containing the configuration's key and value that were found and led
        /// to the current exception.
        /// </summary>
        public List<KeyValuePair<string, string>> FoundValues { get; }

        private static string GenerateMessage(List<KeyValuePair<string, string>> foundValues)
        {
            var builder = new StringBuilder("Found values:\n");
            foreach (var (key, value) in foundValues)
            {
                builder.AppendFormat("{0} = {1}\n", key, value);
            }

            return builder.ToString();
        }
    }
}