using UnityEngine.Events;

public class EndOfSequence : Beat
{
    private UnityEvent endOfSequenceEvent = null;
    private bool sequenceIsFinished = false;
    public bool SequenceIsFinished { get { return sequenceIsFinished; } }

    public EndOfSequence()
    {
        if(endOfSequenceEvent == null)
        {
            endOfSequenceEvent = new UnityEvent();
        }
    }

    public override void PlayBeat()
    {
        endOfSequenceEvent.Invoke();
        sequenceIsFinished = true;
    }

    public void AddEventsToEndOfSequence(UnityAction eventMethod)
    {
        endOfSequenceEvent.AddListener(eventMethod);
    }
}
