public class PlayerDisplaySystem : BaseDisplaySystem
{
    string selectedGame;
    string selectedMatch;

    void OnMatchSelected()
    {
        new Get(path + "/" + selectedGame + "/" + selectedMatch, ChildAdded);
    }

    public void GameSelected(IData game)
    {
        selectedGame = game.ID;
    }

    public void MatchSelected(IData match)
    {
        selectedMatch = match.ID;
        OnMatchSelected();
    }
}
