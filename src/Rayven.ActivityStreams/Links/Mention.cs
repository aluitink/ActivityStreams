﻿namespace Rayven.ActivityStreams.Links;

public class Mention : Link
{
    public Mention() => Type = new List<string>() { "Mention" };

}
