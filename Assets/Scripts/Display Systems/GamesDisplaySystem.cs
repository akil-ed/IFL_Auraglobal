public class GamesDisplaySystem : BaseDisplaySystem
{
    private void Start()
    {
        new Get(path, ChildAdded);
    }
}