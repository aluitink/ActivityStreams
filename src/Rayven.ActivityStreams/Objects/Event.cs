﻿namespace Rayven.ActivityStreams.Objects;

public class Event : Object
{
    public Event() => Type = new List<string>() { "Event" };
}
