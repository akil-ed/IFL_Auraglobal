using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public List<Game> games;
    public List<Match> cricket, football, kabaddi;

    private void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference("games").ChildAdded += GameAdded;
    }

    private void GameAdded(object sender, ChildChangedEventArgs e)
    {
        var game = e.Snapshot.ToClass<Game>();
        games.Add(game);
        switch (game.name)
        {
            case "cricket":
                AddMatchToList(game, cricket, EventManager.Instance.CricketMatchAdded);
                break;

            case "football":
                AddMatchToList(game, football, EventManager.Instance.FootballMatchAdded);
                break;

            case "kabaddi":
                AddMatchToList(game, kabaddi, EventManager.Instance.KabaddiMatchAdded);
                break;
            default:
                print(game.name + ": No use case available for this game type!");
                break;
        }
    }

    void AddMatchToList(Game game, List<Match> list, MatchDelegate matchDelegate)
    {
        FirebaseDatabase.DefaultInstance.GetReference("matches/" + game.id).ChildAdded += delegate (object sender, ChildChangedEventArgs e)
        {
            var match = e.Snapshot.ToClass<Match>();
            list.Add(match);
            matchDelegate(match);
        };
    }
}