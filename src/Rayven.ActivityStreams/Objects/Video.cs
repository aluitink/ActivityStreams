﻿namespace Rayven.ActivityStreams.Objects;

public class Video : Document
{
    public Video() => Type = new List<string>() { "Video" };
}
