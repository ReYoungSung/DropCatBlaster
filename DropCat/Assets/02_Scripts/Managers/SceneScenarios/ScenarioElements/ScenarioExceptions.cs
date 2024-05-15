using System;

public class AttemptAddingToLockedScenario : Exception
{
    public AttemptAddingToLockedScenario()
    {
    }

    public AttemptAddingToLockedScenario(string message)
        : base(message)
    {
    }

    public AttemptAddingToLockedScenario(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class AttemptPlayingUnLockedScenario : Exception
{
    public AttemptPlayingUnLockedScenario()
    {
    }

    public AttemptPlayingUnLockedScenario(string message)
        : base(message)
    {
    }

    public AttemptPlayingUnLockedScenario(string message, Exception inner)
        : base(message, inner)
    {
    }
}
