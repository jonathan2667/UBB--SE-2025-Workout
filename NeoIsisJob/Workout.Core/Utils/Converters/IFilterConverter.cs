// <copyright file="IFilterConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Utils.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// A custom JSON converter for deserializing and serializing IFilter implementations.
    /// </summary>
    public class IFilterConverter : JsonConverter<IFilter>
    {
        /// <inheritdoc/>
        public override IFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize into ProductFilter
            ProductFilter filter = JsonSerializer.Deserialize<ProductFilter>(ref reader, options)!;
            return filter;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IFilter value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (ProductFilter)value, options);
        }
    }
}
