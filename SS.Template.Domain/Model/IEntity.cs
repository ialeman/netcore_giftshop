using System;

namespace SS.Template.Model
{
    /// <summary>
    /// Represents a model entity with an unique identifier of type <see cref="TKey"/>.
    /// </summary>
    /// <typeparam name="TKey">The entity's unique identifier type.</typeparam>
    public interface IEntity<TKey> where TKey : IEquatable<TKey>//, IComparable<TKey>
    {
        /// <summary>
        /// Gets or sets the entity's unique identifier.
        /// </summary>
        TKey Id { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents a model entity with an unique identifier of type <see cref="T:System.Int32" />.
    /// </summary>
    public interface IEntity : IEntity<int>
    {
    }
}
