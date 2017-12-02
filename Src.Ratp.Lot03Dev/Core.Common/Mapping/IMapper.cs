using System;

namespace Core.Common.Mapping
{
    /// <summary>
    /// Represents an object that can map an instance of <typeparamref name="TSource"/> into a new or existing instance of <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type to map from.</typeparam>
    /// <typeparam name="TTarget">The target type to map to.</typeparam>
    public interface IMapper<TSource, TTarget>
    {
        /// <summary>
        /// Maps an instance of <typeparamref name="TSource"/> to an existing instance of <typeparamref name="TTarget"/>.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="target">The target object to map to.</param>
        void Map(TSource source, TTarget target);

        /// <summary>
        /// Maps an instance of <typeparamref name="TSource"/> to a new instance of <typeparamref name="TTarget"/>.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <returns>The new instance mapped from the source object.</returns>
        TTarget Map(TSource source);
    }
}
