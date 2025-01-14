﻿using Rayven.ActivityStreams.Activities;
using Rayven.ActivityStreams.Actors;
using Rayven.ActivityStreams.JsonLD;
using Rayven.ActivityStreams.Links;
using Rayven.ActivityStreams.Objects;
using Rayven.ActivityStreams.Ranges;
using System.Text;
using System.Text.Json;

namespace Rayven.ActivityStreams.Tests;

public class ObjectTests
{
    /// <summary>
    /// Example 1 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object
    /// </summary>
    [Fact]
    public void Example_001()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Object",
              "id": "http://www.test.example/object/1",
              "name": "A Simple, non-specific object"
            }
            """;

        // Act
        var ex1 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex1.Should().BeAssignableTo<IObject>();
        ex1.As<IObject>().JsonLDContext.First().As<ReferenceTermDefinition>().Href.Should().Be(new Uri("https://www.w3.org/ns/activitystreams"));
        ex1.As<IObject>().Id.Should().Be("http://www.test.example/object/1");
        ex1.As<IObject>().Name.First().Should().Be("A Simple, non-specific object");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 61 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-id
    /// </summary>
    [Fact]
    public void Example_061()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "Foo",
              "id": "http://example.org/foo"
            }
            """;

        // Act
        var ex61 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex61.Id.Should().Be("http://example.org/foo");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 62 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-type
    /// </summary>
    [Fact]
    public void Example_062()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A foo",
              "type": "http://example.org/Foo"
            }
            """;

        // Act
        var ex62 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex62.Type.Should().Contain("http://example.org/Foo");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 66 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment
    /// </summary>
    [Fact]
    public void Example_066()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Note",
              "name": "Have you seen my cat?",
              "attachment": [
                {
                  "type": "Image",
                  "content": "This is what he looks like.",
                  "url": "http://example.org/cat.jpeg"
                }
              ]
            }
            """;

        // Act
        var ex66 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex66.Should().BeAssignableTo<Note>();
        ex66.As<Note>().Attachment.Should().HaveCount(1);
        ex66.As<Note>().Attachment.First().Should().BeAssignableTo<Image>();
        ex66.As<Note>().Attachment.First().As<Image>().Content.First().Should().Be("This is what he looks like.");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 67 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attributedto
    /// </summary>
    [Fact]
    public void Example_067()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Image",
              "name": "My cat taking a nap",
              "url": "http://example.org/cat.jpeg",
              "attributedTo": [
                {
                  "type": "Person",
                  "name": "Sally"
                }
              ]
            }
            """;

        // Act
        var ex67 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex67.Should().BeAssignableTo<Image>();
        ex67.As<Image>().AttributedTo.Should().HaveCount(1);
        ex67.As<Image>().AttributedTo.First().Should().BeAssignableTo<Person>();
        ex67.As<Image>().AttributedTo.First().As<Person>().Name.First().Should().Be("Sally");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 68 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attributedto
    /// </summary>
    [Fact]
    public void Example_068()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Image",
              "name": "My cat taking a nap",
              "url": "http://example.org/cat.jpeg",
              "attributedTo": [
                "http://joe.example.org",
                {
                  "type": "Person",
                  "name": "Sally"
                }
              ]
            }
            """;

        // Act
        var ex68 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex68.Should().BeAssignableTo<Image>();
        ex68.As<Image>().AttributedTo.Should().HaveCount(2);
        ex68.As<Image>().AttributedTo.ElementAt(0).Should().BeAssignableTo<Link>();
        ex68.As<Image>().AttributedTo.ElementAt(0).As<Link>().Href.Should().Be(new Uri("http://joe.example.org"));
        ex68.As<Image>().AttributedTo.ElementAt(1).Should().BeAssignableTo<Objects.Object>();
        ex68.As<Image>().AttributedTo.ElementAt(1).As<IObject>().Name.First().Should().Be("Sally");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 69 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-audience
    /// </summary>
    [Fact]
    public void Example_069()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "Holiday announcement",
              "type": "Note",
              "content": "Thursday will be a company-wide holiday. Enjoy your day off!",
              "audience": {
                "type": "http://example.org/Organization",
                "name": "ExampleCo LLC"
              }
            }
            """;

        // Act
        var ex69 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex69.Should().BeAssignableTo<Note>();
        ex69.As<Note>().Audience.Should().HaveCount(1);
        ex69.As<Note>().Audience.First().Type.Should().Contain("http://example.org/Organization");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 70 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bcc
    /// </summary>
    [Fact]
    public void Example_070()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally offered a post to John",
              "type": "Offer",
              "actor": "http://sally.example.org",
              "object": "http://example.org/posts/1",
              "target": "http://john.example.org",
              "bcc": [
                "http://joe.example.org"
              ]
            }
            """;

        // Act
        var ex70 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex70.Should().BeAssignableTo<Offer>();
        ex70.As<Offer>().Bcc.Should().HaveCount(1);
        ex70.As<Offer>().Bcc.First().Should().BeAssignableTo<Link>();
        ex70.As<Offer>().Bcc.First().As<Link>().Href.Should().Be(new Uri("http://joe.example.org"));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 71 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bto
    /// </summary>
    [Fact]
    public void Example_071()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally offered a post to John",
              "type": "Offer",
              "actor": "http://sally.example.org",
              "object": "http://example.org/posts/1",
              "target": "http://john.example.org",
              "bto": [
                "http://joe.example.org"
              ]
            }
            """;

        // Act
        var ex71 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex71.Should().BeAssignableTo<Offer>();
        ex71.As<Offer>().Bto.Should().HaveCount(1);
        ex71.As<Offer>().Bto.First().Should().BeAssignableTo<Link>();
        ex71.As<Offer>().Bto.First().As<Link>().Href.Should().Be(new Uri("http://joe.example.org"));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 72 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-cc
    /// </summary>
    [Fact]
    public void Example_072()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally offered a post to John",
              "type": "Offer",
              "actor": "http://sally.example.org",
              "object": "http://example.org/posts/1",
              "target": "http://john.example.org",
              "cc": [
                "http://joe.example.org"
              ]
            }
            """;

        // Act
        var ex72 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex72.Should().BeAssignableTo<Offer>();
        ex72.As<Offer>().Cc.Should().HaveCount(1);
        ex72.As<Offer>().Cc.First().Should().BeAssignableTo<Link>();
        ex72.As<Offer>().Cc.First().As<Link>().Href.Should().Be(new Uri("http://joe.example.org"));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 73 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-context
    /// </summary>
    [Fact]
    public void Example_073()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Activities in context 1",
              "type": "Collection",
              "items": [
                {
                  "type": "Offer",
                  "actor": "http://sally.example.org",
                  "object": "http://example.org/posts/1",
                  "target": "http://john.example.org",
                  "context": "http://example.org/contexts/1"
                },
                {
                  "type": "Like",
                  "actor": "http://joe.example.org",
                  "object": "http://example.org/posts/2",
                  "context": "http://example.org/contexts/1"
                }
              ]
            }
            """;

        // Act
        var ex73 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex73.Should().BeAssignableTo<Collection>();
        ex73.As<Collection>().Items.First().As<Offer>().Context.First().As<Link>().Href.Should().Be("http://example.org/contexts/1");
        ex73.As<Collection>().Items.Last().As<Like>().Context.First().As<Link>().Href.Should().Be("http://example.org/contexts/1");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 78 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-generator
    /// </summary>
    [Fact]
    public void Example_078()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "generator": {
                "type": "Application",
                "name": "Exampletron 3000"
              }
            }
            """;

        // Act
        var ex78 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex78.Should().BeAssignableTo<Note>();
        ex78.As<Note>().Generator.Should().HaveCount(1);
        ex78.As<Note>().Generator.First().Should().BeAssignableTo<Application>();
        ex78.As<Note>().Generator.First().As<Application>().Name.First().Should().Be("Exampletron 3000");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 79 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-icon
    /// </summary>
    [Fact]
    public void Example_079()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "icon": {
                "type": "Image",
                "name": "Note icon",
                "url": "http://example.org/note.png",
                "width": 16,
                "height": 16
              }
            }
            """;

        // Act
        var ex79 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex79.Should().BeAssignableTo<Note>();
        ex79.As<Note>().Icon.Should().HaveCount(1);
        ex79.As<Note>().Icon.First().Should().BeAssignableTo<Image>();
        ex79.As<Note>().Icon.First().As<Image>().Name.First().Should().Be("Note icon");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 80 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-icon
    /// </summary>
    [Fact]
    public void Example_080()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "A simple note",
              "icon": [
                {
                  "type": "Image",
                  "summary": "Note (16x16)",
                  "url": "http://example.org/note1.png",
                  "width": 16,
                  "height": 16
                },
                {
                  "type": "Image",
                  "summary": "Note (32x32)",
                  "url": "http://example.org/note2.png",
                  "width": 32,
                  "height": 32
                }
              ]
            }
            """;

        // Act
        var ex80 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex80.Should().BeAssignableTo<Note>();
        ex80.As<Note>().Icon.Should().HaveCount(2);
        ex80.As<Note>().Icon.First().Should().BeAssignableTo<Image>();
        ex80.As<Note>().Icon.First().As<Image>().Summary.First().Should().Be("Note (16x16)");
        ex80.As<Note>().Icon.Last().As<Image>().Summary.First().Should().Be("Note (32x32)");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 81 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-image
    /// </summary>
    [Fact]
    public void Example_081()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "image": {
                "type": "Image",
                "name": "A Cat",
                "url": "http://example.org/cat.png"
              }
            }
            """;

        // Act
        var ex81 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex81.Should().BeAssignableTo<Note>();
        ex81.As<Note>().Image.Should().HaveCount(1);
        ex81.As<Note>().Image.First().Should().BeAssignableTo<Image>();
        ex81.As<Note>().Image.First().As<Image>().Name.First().Should().Be("A Cat");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 82 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-image
    /// </summary>
    [Fact]
    public void Example_082()
    {
        // Arrange
        // We changed the second element in the below image list to be a url to validate that that is possible.
        // We also added an explicit Link after that.
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "image": [
                {
                  "type": "Image",
                  "name": "Cat 1",
                  "url": "http://example.org/cat1.png"
                },
                "http://example.org/cat2.png",
                {
                  "type": "Link",
                  "href": "http://example.org/cat3.png"
                }
              ]
            }
            """;

        // Act
        var ex82 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex82.Should().BeAssignableTo<Note>();
        ex82.As<Note>().Image.Should().HaveCount(3);
        ex82.As<Note>().Image.ElementAt(0).Should().BeAssignableTo<Image>();
        ex82.As<Note>().Image.ElementAt(0).As<Image>().Name.First().Should().Be("Cat 1");
        ex82.As<Note>().Image.ElementAt(1).Should().BeAssignableTo<ILink>();
        ex82.As<Note>().Image.ElementAt(1).As<ILink>().Href.Should().Be(new Uri("http://example.org/cat2.png"));
        ex82.As<Note>().Image.ElementAt(2).Should().BeAssignableTo<ILink>();
        ex82.As<Note>().Image.ElementAt(2).As<ILink>().Href.Should().Be(new Uri("http://example.org/cat3.png"));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 83 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-inreplyto
    /// </summary>
    [Fact]
    public void Example_083()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "inReplyTo": {
                "summary": "Previous note",
                "type": "Note",
                "content": "What else is there?"
              }
            }
            """;

        // Act
        var ex83 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex83.Should().BeAssignableTo<Note>();
        ex83.As<Note>().InReplyTo.Should().HaveCount(1);
        ex83.As<Note>().InReplyTo.First().As<Note>().Content.First().Should().Be("What else is there?");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 84 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-inreplyto
    /// </summary>
    [Fact]
    public void Example_084()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "This is all there is.",
              "inReplyTo": "http://example.org/posts/1"
            }
            """;

        // Act
        var ex84 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex84.Should().BeAssignableTo<Note>();
        ex84.As<Note>().InReplyTo.Should().HaveCount(1);
        ex84.As<Note>().InReplyTo.First().As<Link>().Href.Should().Be("http://example.org/posts/1");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 88 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-location
    /// </summary>
    [Fact]
    public void Example_088()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Person",
              "name": "Sally",
              "location": {
                "name": "Over the Arabian Sea, east of Socotra Island Nature Sanctuary",
                "type": "Place",
                "longitude": 12.34,
                "latitude": 56.78,
                "altitude": 90,
                "units": "m"
              }
            }
            """;

        // Act
        var ex88 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex88.Should().BeAssignableTo<Person>();
        ex88.As<Person>().Location.Should().HaveCount(1);
        ex88.As<Person>().Location.First().As<Place>().Name.First().Should().Be("Over the Arabian Sea, east of Socotra Island Nature Sanctuary");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 89 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items
    /// </summary>
    [Fact]
    public void Example_089()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally's notes",
              "type": "Collection",
              "totalItems": 2,
              "items": [
                {
                  "type": "Note",
                  "name": "Reminder for Going-Away Party"
                },
                {
                  "type": "Note",
                  "name": "Meeting 2016-11-17"
                }
              ]
            }
            """;

        // Act
        var ex89 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex89.Should().BeAssignableTo<Collection>();
        ex89.As<Collection>().Items.Should().HaveCount(2);
        ex89.As<Collection>().Items.First().As<Note>().Name.First().Should().Be("Reminder for Going-Away Party");
        ex89.As<Collection>().Items.Last().As<Note>().Name.First().Should().Be("Meeting 2016-11-17");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 90 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items
    /// </summary>
    [Fact]
    public void Example_090()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally's notes",
              "type": "OrderedCollection",
              "totalItems": 2,
              "orderedItems": [
                {
                  "type": "Note",
                  "name": "Meeting 2016-11-17"
                },
                {
                  "type": "Note",
                  "name": "Reminder for Going-Away Party"
                }
              ]
            }
            """;

        // Act
        var ex90 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex90.Should().BeAssignableTo<OrderedCollection>();
        ex90.As<OrderedCollection>().OrderedItems.Should().HaveCount(2);
        ex90.As<OrderedCollection>().OrderedItems.First().As<Note>().Name.First().Should().Be("Meeting 2016-11-17");
        ex90.As<OrderedCollection>().OrderedItems.Last().As<Note>().Name.First().Should().Be("Reminder for Going-Away Party");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 102 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-preview
    /// </summary>
    [Fact]
    public void Example_102()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Video",
              "name": "Cool New Movie",
              "duration": "PT2H30M",
              "preview": {
                "type": "Video",
                "name": "Trailer",
                "duration": "PT1M",
                "url": {
                  "href": "http://example.org/trailer.mkv",
                  "mediaType": "video/mkv"
                }
              }
            }
            """;

        // Act
        var ex102 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex102.Should().BeAssignableTo<Video>();
        ex102.As<Video>().Preview.Should().HaveCount(1);
        ex102.As<Video>().Preview.First().As<Video>().Name.First().Should().Be("Trailer");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 104 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-replies
    /// </summary>
    [Fact]
    public void Example_104()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "id": "http://www.test.example/notes/1",
              "content": "I am fine.",
              "replies": {
                "type": "Collection",
                "totalItems": 1,
                "items": [
                  {
                    "summary": "A response to the note",
                    "type": "Note",
                    "content": "I am glad to hear it.",
                    "inReplyTo": "http://www.test.example/notes/1"
                  }
                ]
              }
            }
            """;

        // Act
        var ex104 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex104.Should().BeAssignableTo<Note>();
        ex104.As<Note>().Replies.As<Collection>().Items.First().As<Note>().Content.First().Should().Be("I am glad to hear it.");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 105 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-tag
    /// </summary>
    [Fact]
    public void Example_105()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Image",
              "summary": "Picture of Sally",
              "url": "http://example.org/sally.jpg",
              "tag": [
                {
                  "type": "Person",
                  "id": "http://sally.example.org",
                  "name": "Sally"
                }
              ]
            }
            """;

        // Act
        var ex105 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex105.Should().BeAssignableTo<Image>();
        ex105.As<Image>().Tag.Should().HaveCount(1);
        ex105.As<Image>().Tag.First().As<Person>().Name.First().Should().Be("Sally");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 108 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-to
    /// </summary>
    [Fact]
    public void Example_108()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "Sally offered the post to John",
              "type": "Offer",
              "actor": "http://sally.example.org",
              "object": "http://example.org/posts/1",
              "target": "http://john.example.org",
              "to": [
                "http://joe.example.org"
              ]
            }
            """;

        // Act
        var ex108 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex108.Should().BeAssignableTo<Offer>();
        ex108.As<Offer>().To.Should().HaveCount(1);
        ex108.As<Offer>().To.First().As<Link>().Href.Should().Be("http://joe.example.org");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 109 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-url
    /// </summary>
    [Fact]
    public void Example_109()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Document",
              "name": "4Q Sales Forecast",
              "url": "http://example.org/4q-sales-forecast.pdf"
            }
            """;

        // Act
        var ex109 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex109.Should().BeAssignableTo<Document>();
        ex109.As<Document>().Url.Should().HaveCount(1);
        ex109.As<Document>().Url.First().As<Link>().Href.Should().Be("http://example.org/4q-sales-forecast.pdf");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 110 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-url
    /// </summary>
    [Fact]
    public void Example_110()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Document",
              "name": "4Q Sales Forecast",
              "url": {
                "type": "Link",
                "href": "http://example.org/4q-sales-forecast.pdf"
              }
            }
            """;

        // Act
        var ex110 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex110.Should().BeAssignableTo<Document>();
        ex110.As<Document>().Url.Should().HaveCount(1);
        ex110.As<Document>().Url.First().As<Link>().Href.Should().Be("http://example.org/4q-sales-forecast.pdf");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 111 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-url
    /// </summary>
    [Fact]
    public void Example_111()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Document",
              "name": "4Q Sales Forecast",
              "url": [
                {
                  "type": "Link",
                  "href": "http://example.org/4q-sales-forecast.pdf",
                  "mediaType": "application/pdf"
                },
                {
                  "type": "Link",
                  "href": "http://example.org/4q-sales-forecast.html",
                  "mediaType": "text/html"
                }
              ]
            }
            """;

        // Act
        var ex111 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex111.Should().BeAssignableTo<Document>();
        ex111.As<Document>().Url.Should().HaveCount(2);
        ex111.As<Document>().Url.First().As<Link>().Href.Should().Be("http://example.org/4q-sales-forecast.pdf");
        ex111.As<Document>().Url.Last().As<Link>().Href.Should().Be("http://example.org/4q-sales-forecast.html");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 113 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-altitude
    /// </summary>
    [Fact]
    public void Example_113()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Place",
              "name": "Fresno Area",
              "altitude": 15.0,
              "latitude": 36.75,
              "longitude": 119.7667,
              "units": "miles"
            }
            """;

        // Act
        var ex113 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex113.Should().BeAssignableTo<Place>();
        ex113.As<Place>().Altitude.Should().Be(15);

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 114 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-content
    /// </summary>
    [Fact]
    public void Example_114()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "A <em>simple</em> note"
            }
            """;

        // Act
        var ex114 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex114.Should().BeAssignableTo<Note>();
        ex114.As<Note>().Content.Should().HaveCount(1);
        ex114.As<Note>().Content.First().Should().Be("A <em>simple</em> note");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 115 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-content
    /// </summary>
    [Fact]
    public void Example_115()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "contentMap": {
                "en": "A <em>simple</em> note",
                "es": "Una nota <em>sencilla</em>",
                "zh-Hans": "一段<em>简单的</em>笔记"
              }
            }
            """;

        // Act
        var ex115 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex115.Should().BeAssignableTo<Note>();
        ex115.As<Note>().ContentMap.Should().HaveCount(1);
        ex115.As<Note>().ContentMap.First().Keys.Should().HaveCount(3);
        ex115.As<Note>().ContentMap.First()["en"].Should().Be("A <em>simple</em> note");
        ex115.As<Note>().ContentMap.First()["es"].Should().Be("Una nota <em>sencilla</em>");
        ex115.As<Note>().ContentMap.First()["zh-Hans"].Should().Be("一段<em>简单的</em>笔记");

        // Serialize and check for intactness

        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 116 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-content
    /// </summary>
    [Fact]
    public void Example_116()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "mediaType": "text/markdown",
              "content": "## A simple note\nA simple markdown `note`"
            }
            """;

        // Act
        var ex116 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex116.Should().BeAssignableTo<Note>();
        ex116.As<Note>().Content.Should().HaveCount(1);
        ex116.As<Note>().Content.First().Should().Be("## A simple note\nA simple markdown `note`");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 117 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-name
    /// </summary>
    [Fact]
    public void Example_117()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Note",
              "name": "A simple note"
            }
            """;

        // Act
        var ex117 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex117.Should().BeAssignableTo<Note>();
        ex117.As<Note>().Name.Should().HaveCount(1);
        ex117.As<Note>().Name.First().Should().Be("A simple note");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 118 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-name
    /// </summary>
    [Fact]
    public void Example_118()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Note",
              "nameMap": {
                "en": "A simple note",
                "es": "Una nota sencilla",
                "zh-Hans": "一段简单的笔记"
              }
            }
            """;

        // Act
        var ex118 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex118.Should().BeAssignableTo<Note>();
        ex118.As<Note>().NameMap.Should().HaveCount(1);
        ex118.As<Note>().NameMap.First().Keys.Should().HaveCount(3);
        ex118.As<Note>().NameMap.First()["en"].Should().Be("A simple note");
        ex118.As<Note>().NameMap.First()["es"].Should().Be("Una nota sencilla");
        ex118.As<Note>().NameMap.First()["zh-Hans"].Should().Be("一段简单的笔记");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 119 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-duration
    /// </summary>
    [Fact]
    public void Example_119()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Video",
              "name": "Birds Flying",
              "url": "http://example.org/video.mkv",
              "duration": "PT2H"
            }
            """;

        // Act
        var ex119 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex119.Should().BeAssignableTo<Video>();
        ex119.As<Video>().Duration.Should().Be(new TimeSpan(2, 0, 0));

        // Serialize and check for intactness
        ex119 = Deserialize<IObjectOrLink>(Serialize(ex119));
        ex119.Should().BeAssignableTo<Video>();
        ex119.As<Video>().Duration.Should().Be(new TimeSpan(2, 0, 0));
    }

    /// <summary>
    /// Example 126 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-mediatype
    /// </summary>
    [Fact]
    public void Example_126()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Link",
              "href": "http://example.org/abc",
              "hreflang": "en",
              "mediaType": "text/html",
              "name": "Next"
            }
            """;

        // Act
        var ex126 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex126.Should().BeAssignableTo<Link>();
        ex126.As<Link>().MediaType.Should().Be("text/html");

        // Serialize and check for intactness
        ex126 = Deserialize<IObjectOrLink>(Serialize(ex126));
        ex126.Should().BeAssignableTo<Link>();
        ex126.As<Link>().MediaType.Should().Be("text/html");
    }

    /// <summary>
    /// Example 127 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-endtime
    /// </summary>
    [Fact]
    public void Example_127()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Event",
              "name": "Going-Away Party for Jim",
              "startTime": "2014-12-31T23:00:00-08:00",
              "endTime": "2015-01-01T06:00:00-08:00"
            }
            """;

        // Act
        var ex127 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex127.Should().BeAssignableTo<Event>();
        ex127.As<Event>().EndTime.Should().Be(DateTime.Parse("2015-01-01T06:00:00-08:00"));

        // Serialize and check for intactness
        ex127 = Deserialize<IObjectOrLink>(Serialize(ex127));
        ex127.As<Event>().EndTime.Should().Be(DateTime.Parse("2015-01-01T06:00:00-08:00"));
    }

    /// <summary>
    /// Example 128 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-published
    /// </summary>
    [Fact]
    public void Example_128()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "summary": "A simple note",
              "type": "Note",
              "content": "Fish swim.",
              "published": "2014-12-12T12:12:12Z"
            }
            """;

        // Act
        var ex128 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex128.Should().BeAssignableTo<Note>();
        ex128.As<Note>().Published.Should().Be(DateTime.Parse("2014-12-12T12:12:12Z", styles: System.Globalization.DateTimeStyles.AdjustToUniversal));

        // Serialize and check for intactness
        ex128 = Deserialize<IObjectOrLink>(Serialize(ex128));
        ex128.As<Note>().Published.Should().Be(DateTime.Parse("2014-12-12T12:12:12Z", styles: System.Globalization.DateTimeStyles.AdjustToUniversal));
    }

    /// <summary>
    /// Example 129 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-starttime
    /// </summary>
    [Fact]
    public void Example_129()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "type": "Event",
              "name": "Going-Away Party for Jim",
              "startTime": "2014-12-31T23:00:00-08:00",
              "endTime": "2015-01-01T06:00:00-08:00"
            }
            """;

        // Act
        var ex129 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex129.Should().BeAssignableTo<Event>();
        ex129.As<Event>().StartTime.Should().Be(DateTime.Parse("2014-12-31T23:00:00-08:00"));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 133 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-summary
    /// </summary>
    [Fact]
    public void Example_133()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "Cane Sugar Processing",
              "type": "Note",
              "summary": "A simple <em>note</em>"
            }
            """;

        // Act
        var ex133 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex133.Should().BeAssignableTo<Note>();
        ex133.As<Note>().Summary.Should().HaveCount(1);
        ex133.As<Note>().Summary.First().Should().Be("A simple <em>note</em>");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 134 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-summary
    /// </summary>
    [Fact]
    public void Example_134()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "Cane Sugar Processing",
              "type": "Note",
              "summaryMap": {
                "en": "A simple <em>note</em>",
                "es": "Una <em>nota</em> sencilla",
                "zh-Hans": "一段<em>简单的</em>笔记"
              }
            }
            """;

        // Act
        var ex134 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex134.Should().BeAssignableTo<Note>();
        ex134.As<Note>().SummaryMap.Should().HaveCount(1);
        ex134.As<Note>().SummaryMap.First().Keys.Should().HaveCount(3);
        ex134.As<Note>().SummaryMap.First()["en"].Should().Be("A simple <em>note</em>");
        ex134.As<Note>().SummaryMap.First()["es"].Should().Be("Una <em>nota</em> sencilla");
        ex134.As<Note>().SummaryMap.First()["zh-Hans"].Should().Be("一段<em>简单的</em>笔记");

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }

    /// <summary>
    /// Example 137 taken from https://www.w3.org/TR/activitystreams-vocabulary/#dfn-updated
    /// </summary>
    [Fact]
    public void Example_137()
    {
        // Arrange
        var input = """
            {
              "@context": "https://www.w3.org/ns/activitystreams",
              "name": "Cranberry Sauce Idea",
              "type": "Note",
              "content": "Mush it up so it does not have the same shape as the can.",
              "updated": "2014-12-12T12:12:12Z"
            }
            """;

        // Act
        var ex137 = Deserialize<IObjectOrLink>(input);

        // Assert
        ex137.Should().BeAssignableTo<Note>();
        ex137.As<Note>().Updated.Should().Be(DateTime.Parse("2014-12-12T12:12:12Z", styles: System.Globalization.DateTimeStyles.AdjustToUniversal));

        // Serialize and check for intactness
        Serialize(Deserialize<IObjectOrLink>(Serialize(Deserialize<IObjectOrLink>(input)))).Should().Be(Serialize(Deserialize<IObjectOrLink>(input)));
    }
}

