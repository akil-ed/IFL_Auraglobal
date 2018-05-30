public class MatchesDisplaySystem : BaseDisplaySystem
{
    string selectedGame;

    void OnGameSelected()
    {
        new Get(path + "/" + selectedGame, ChildAdded);
    }

    public void GameSelected(IData data)
    {
        selectedGame = data.ID;
        OnGameSelected();
    }
}