using UnityEngine;
using UnityEngine.UI;

public class GameUI : UI
{
    public Text gameName;

    public Game game;

    public override IData data
    {
        get
        {
            return game;
        }
    }

    public override void Display(string str)
    {
        game = JsonUtility.FromJson<Game>(str);
        gameName.text = game.name;
    }
}
