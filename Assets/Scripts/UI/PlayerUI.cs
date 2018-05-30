using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UI
{
    public Text playerName;

    public Player player;

    public override IData data
    {
        get
        {
            return player;
        }
    }

    public override void Display(string str)
    {
        player = JsonUtility.FromJson<Player>(str);
        playerName.text = player.name;
    }
}
