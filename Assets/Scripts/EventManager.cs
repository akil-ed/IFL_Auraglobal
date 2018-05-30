public class EventManager : Singleton<EventManager>
{
    public MatchDelegate onCricketMatchAdded, onFootballMatchAdded, onKabaddiMatchAdded;

    public void CricketMatchAdded(Match match)
    {
        onCricketMatchAdded(match);
    }

    public void FootballMatchAdded(Match match)
    {
        onFootballMatchAdded(match);
    }

    public void KabaddiMatchAdded(Match match)
    {
        onKabaddiMatchAdded(match);
    }
}

public delegate void MatchDelegate(Match match);