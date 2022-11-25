﻿using KristofferStrube.ActivityStreams.JsonConverters;
using System.Text.Json.Serialization;

namespace KristofferStrube.ActivityStreams;

public class Activity : Object
{
    /// <summary>
    /// Describes one or more entities that either performed or are expected to perform the activity. Any single activity can have multiple actors. The actor may be specified using an indirect Link.
    /// </summary>
    [JsonPropertyName("actor")]
    [JsonConverter(typeof(OneOrMultipleConverter<IObjectOrLink>))]
    public IEnumerable<IObjectOrLink>? Actor { get; set; }

    /// <summary>
    /// Describes an object of any kind. The Object type serves as the base type for most of the other kinds of objects defined in the Activity Vocabulary, including other Core types such as Activity, IntransitiveActivity, Collection and OrderedCollection.
    /// </summary>
    [JsonPropertyName("object")]
    [JsonConverter(typeof(OneOrMultipleConverter<IObjectOrLink>))]
    public IEnumerable<IObjectOrLink>? Object { get; set; }

    /// <summary>
    /// Describes the indirect object, or target, of the activity. The precise meaning of the target is largely dependent on the type of action being described but will often be the object of the English preposition "to". For instance, in the activity "John added a movie to his wishlist", the target of the activity is John's wishlist. An activity can have more than one target.
    /// </summary>
    [JsonPropertyName("target")]
    [JsonConverter(typeof(OneOrMultipleConverter<IObjectOrLink>))]
    public IEnumerable<IObjectOrLink>? Target { get; set; }


}
