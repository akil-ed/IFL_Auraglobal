using UnityEngine;
using UnityEngine.UI;

public class MatchUI : UI
{
    public Text matchName;

    public Match match;

    public override IData data
    {
        get
        {
            return match;
        }
    }

    public override void Display(string str)
    {
        match = JsonUtility.FromJson<Match>(str);
        matchName.text = match.team1 + " vs " + match.team2;
    }
}