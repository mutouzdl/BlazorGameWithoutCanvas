namespace AntDesignGameFramework;

public abstract class GameContext
{
    public GameTime GameTime { get; } = new GameTime();
    public Display Display { get; } = new Display();
    public List<GameObject> GameObjects { get; } = new();

    public event Action<GameObject> OnAddGameObject;
    public event Action<GameObject> OnDestoryGameObject;

    public async ValueTask Step(float elapsedTime)
    {
        this.GameTime.TotalTime += elapsedTime;
        this.GameTime.ElapsedTime = elapsedTime;

        await Update();

        UpdateGameObjectRenderRect();
    }

    /// <summary>
    /// 更新游戏物体的渲染规格
    /// </summary>
    private void UpdateGameObjectRenderRect()
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Transform.UpdateRenderRect();
        }
    }

    protected abstract ValueTask Update();

    public void AddGameObject(GameObject gameObject)
    {
        if (GameObjects.Any(t => t.Uid == gameObject.Uid))
        {
            return;
        }

        GameObjects.Add(gameObject);

        if (OnAddGameObject != null)
        {
            OnAddGameObject.Invoke(gameObject);
        }
    }

    public void DestoryGameObject(string uid)
    {
        var gameObject = GameObjects.SingleOrDefault(t => t.Uid == uid);

        if (gameObject == null)
        {
            return;
        }

        GameObjects.Remove(gameObject);

        if (OnDestoryGameObject != null)
        {
            OnDestoryGameObject.Invoke(gameObject);
        }
    }

}