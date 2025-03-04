﻿using Rayven.ActivityStreams.Activities;
using Rayven.ActivityStreams.Actors;
using Rayven.ActivityStreams.Objects;

namespace Rayven.ActivityStreams;

internal static class ObjectTypes
{
    internal static readonly Dictionary<string, Type> Types = new()
    {
        { "Object", typeof(Objects.Object) },
        { "Collection", typeof(Collection) },
        { "CollectionPage", typeof(CollectionPage) },
        { "OrderedCollection", typeof(OrderedCollection) },
        { "OrderedCollectionPage", typeof(OrderedCollectionPage) },
        { "Relationship", typeof(Relationship) },
        { "Article", typeof(Article) },
        { "Document", typeof(Document) },
        { "Audio", typeof(Audio) },
        { "Image", typeof(Image) },
        { "Video", typeof(Video) },
        { "Note", typeof(Note) },
        { "Page", typeof(Page) },
        { "Event", typeof(Event) },
        { "Place", typeof(Place) },
        { "Profile", typeof(Profile) },
        { "Tombstone", typeof(Tombstone) },
        // Actors
        { "Application", typeof(Application) },
        { "Group", typeof(Group) },
        { "Organísation", typeof(Organisation) },
        { "Person", typeof(Person) },
        { "Service", typeof(Service) },
        // Activities
        { "Activity", typeof(Activity) },
        { "IntransitiveActiviy", typeof(IntransitiveActiviy) },
        { "Accept", typeof(Accept) },
        { "Add", typeof(Add) },
        { "Announce", typeof(Announce) },
        { "Arrive", typeof(Arrive) },
        { "Block", typeof(Block) },
        { "Create", typeof(Create) },
        { "Delete", typeof(Delete) },
        { "Dislike", typeof(Dislike) },
        { "Flag", typeof(Flag) },
        { "Follow", typeof(Follow) },
        { "Ignore", typeof(Ignore) },
        { "Invite", typeof(Invite) },
        { "Join", typeof(Join) },
        { "Leave", typeof(Leave) },
        { "Like", typeof(Like) },
        { "Listen", typeof(Listen) },
        { "Move", typeof(Move) },
        { "Offer", typeof(Offer) },
        { "Question", typeof(Question) },
        { "Reject", typeof(Reject) },
        { "Read", typeof(Read) },
        { "Remove", typeof(Remove) },
        { "TentativeReject", typeof(TentativeReject) },
        { "TentativeAccept", typeof(TentativeAccept) },
        { "Travel", typeof(Travel) },
        { "Undo", typeof(Undo) },
        { "Update", typeof(Update) },
        { "View", typeof(View) },
    };
}
