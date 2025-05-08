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
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDocument.RootElement;

            if (rootElement.TryGetProperty("$type", out var typeElement))
            {
                var type = typeElement.GetString();
                return type switch
                {
                    nameof(CategoryFilter) => JsonSerializer.Deserialize<CategoryFilter>(rootElement.GetRawText(), options)!,
                    nameof(ProductFilter) => JsonSerializer.Deserialize<ProductFilter>(rootElement.GetRawText(), options)!,
                    nameof(CartItemFilter) => JsonSerializer.Deserialize<CartItemFilter>(rootElement.GetRawText(), options)!,
                    _ => throw new JsonException($"Unknown filter type: {type}")
                };
            }

            throw new JsonException("Filter type not specified");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IFilter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("$type", value.GetType().Name);

            switch (value)
            {
                case CategoryFilter categoryFilter:
                    JsonSerializer.Serialize(writer, categoryFilter, options);
                    break;
                case ProductFilter productFilter:
                    JsonSerializer.Serialize(writer, productFilter, options);
                    break;
                case CartItemFilter cartItemFilter:
                    JsonSerializer.Serialize(writer, cartItemFilter, options);
                    break;
                default:
                    throw new JsonException($"Unknown filter type: {value.GetType().Name}");
            }

            writer.WriteEndObject();
        }
    }
}
