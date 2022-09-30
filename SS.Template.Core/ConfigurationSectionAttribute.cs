using System;

namespace SS
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates the name of a configuration section to read configuration values from.
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the configuration section to read values from.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SS.ConfigurationSectionAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="T:System.ArgumentNullException">name</exception>
        public ConfigurationSectionAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
