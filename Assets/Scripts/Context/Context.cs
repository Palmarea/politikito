using System;
using UnityEngine;

public static class Context
{
    public static TutorialData TutorialData { get; private set; }
    
    public static void Initialize()
    {
        TutorialData = new TutorialData();
    }
}
