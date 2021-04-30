using System;

public static class GameplayEventChannel
{
    public static event Action Start;
    public static event Action End;

    public static void PublishStart()
    {
        Start?.Invoke();
    }

    public static void PublishEnd()
    {
        End?.Invoke();
    }
}