using System;

namespace SS
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that the value for a property or constructor argument should be read from the configuration,
    /// using the provided key.
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class ConfigurationValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SS.ConfigurationValueAttribute" /> class.
        /// </summary>
        /// <param name="key">The configuration key to read the value from.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        /// <exception cref="T:System.ArgumentNullException">key</exception>
        /// <inheritdoc />
        public ConfigurationValueAttribute(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Gets the configuration key to read the value from.
        /// </summary>
        public string Key { get; }
    }
}