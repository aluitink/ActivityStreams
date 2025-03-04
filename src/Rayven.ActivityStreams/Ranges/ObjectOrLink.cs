﻿using Rayven.ActivityStreams.JsonConverters;
using Rayven.ActivityStreams.JsonLD;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rayven.ActivityStreams.Ranges;

public class ObjectOrLink : IObjectOrLink
{
    /// <summary>
    /// The context of the JSON-LD object.
    /// </summary>
    [JsonPropertyName("@context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOrMultipleConverter<ITermDefinition>))]
    public IEnumerable<ITermDefinition>? JsonLDContext { get; set; } = new List<ITermDefinition>() { new ReferenceTermDefinition(new Uri("https://www.w3.org/ns/activitystreams")) };

    /// <summary>
    /// Provides the globally unique identifier for an Object or Link.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; set; }

    /// <summary>
    /// Provides globally unique types for an Object.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OneOrMultipleConverter<string>))]
    public IEnumerable<string>? Type { get; set; }

    /// <summary>
    /// A simple, human-readable, plain-text name for the object. HTML markup must not be included. The name may be expressed using multiple language-tagged values.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OneOrMultipleConverter<string>))]
    public IEnumerable<string>? Name { get; set; }

    /// <summary>
    /// A simple, human-readable, plain-text name for the object. HTML markup must not be included. The name may be expressed using multiple language-tagged values.
    /// </summary>
    [JsonPropertyName("mediaType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MediaType { get; set; }

    /// <summary>
    /// Identifies an entity that provides a preview of this object.
    /// </summary>
    [JsonPropertyName("preview")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OneOrMultipleConverter<IObjectOrLink>))]
    public IEnumerable<IObjectOrLink>? Preview { get; set; }
}
