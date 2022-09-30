using System;


namespace SS.Template.Model
{
    /// <inheritdoc cref="IEntity{TKey}" />
    /// <summary>
    /// Model entity base class with Id of type <paramtyperef name="TKey" />.
    /// </summary>
    /// <typeparam name="TKey">The unique identifier type.</typeparam>
    public class Entity<TKey> : IEntity<TKey>, IEquatable<Entity<TKey>>
        where TKey : IEquatable<TKey>//, IComparable<TKey>
    {
        /// <summary>
        /// Get unproxied type
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <inheritdoc />
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TKey>);
        }

        /// <inheritdoc />
        public bool Equals(Entity<TKey> other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            if (Equals(Id, default(TKey)))
            {
                return base.GetHashCode();
            }

            return GetType().GetHashCode() ^ Id.GetHashCode();
        }

        /// <summary>
        /// Is transient
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Result</returns>
        private static bool IsTransient(Entity<TKey> obj)
        {
            return obj != null && Equals(obj.Id, default(TKey));
        }

        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator ==(Entity<TKey> x, Entity<TKey> y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Not equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator !=(Entity<TKey> x, Entity<TKey> y)
        {
            return !(x == y);
        }
    }

    /// <inheritdoc cref="Entity{TKey}" />
    /// <summary>
    /// Model entity base class with Id of type <see cref="T:System.Int32" />.
    /// </summary>
    public class Entity : Entity<int>, IEntity, IEquatable<Entity>
    {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Entity other)
        {
            return base.Equals(other);
        }
    }
}
