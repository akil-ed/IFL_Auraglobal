using System;
using UnityEngine;
using UnityEngine.UI;
public class MatchItem : UI, IIndex
{
    public MatchData ItemDetails;
    public string TournamentName;
    public Text MatchNameTXT, TournamentNameTXT, TimerTXT;
    public GameObject SelectedImage;
    TimeSpan TimeDifference;
    public bool isLive, isSelected, isDataEntry;
    public int TournamentIndex, MatchIndex;
    Match match;
    int Index;

    public override IData data
    {
        get
        {
            return match;
        }
    }

    public int index
    {
        get
        {
            return Index;
        }

        set
        {
            Index = value;
        }
    }

    void Update()
    {
        if (!isLive)
        {
            TimeDifference = ItemDetails.Date.Subtract(DateTime.Now);
            TimerTXT.text = string.Format("{0:00}:{1:00}:{2:00}",
                (int)TimeDifference.TotalHours,
                TimeDifference.Minutes,
                TimeDifference.Seconds);
        }

    }

    public void AssignValues()
    {
        MatchNameTXT.text = ItemDetails.Team1.ToUpper() + "\nVS\n" + ItemDetails.Team2.ToUpper();
        TournamentNameTXT.text = TournamentName;
        if (isSelected)
        {
            Join();
        }
    }

    public void Join()
    {
        if (isDataEntry)
        {
            DataEntryUIManager.instance.SelectedMatch = ItemDetails;
            DataEntryUIManager.instance.CreateMatchListing();
            DataEntryUIManager.instance.SwapPage();
            return;
        }
        SelectedImage.SetActive(true);
        isSelected = true;
        TeamManager.instance.SelectedTournamentIndex = TournamentIndex;
        TeamManager.instance.SelectedMatchIndex = MatchIndex;
        TeamManager.instance.SelectedMatch = ItemDetails;
        TeamManager.instance.CreateLeagueListing();
    }

    public override void Display(string str)
    {
        match = JsonUtility.FromJson<Match>(str);
        MatchNameTXT.text = match.team1 + " vs " + match.team2;
        TimerTXT.text = match.startTime;
    }
}
