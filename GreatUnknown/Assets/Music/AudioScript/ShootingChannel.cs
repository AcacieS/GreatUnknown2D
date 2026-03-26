using System;

[Serializable]
public class ShootingChannel: Channel
{
    public bool isShootingStory = false;
    public void Reset()
    {
        isShootingStory = false;
    }
}