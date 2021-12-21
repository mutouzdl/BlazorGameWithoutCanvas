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

        // 移除已销毁的对象
        GameObjects.RemoveAll(t => t.IsDestroy);

        await Update();

        CheckCollision();

        UpdateGameObjectRenderRect();
    }

    /// <summary>
    /// 检查碰撞
    /// </summary>
    private void CheckCollision()
    {
        var boundingBoxs = new List<BoundingBox>();

        foreach (var gameObject in GameObjects)
        {
            var components = gameObject.GetAllComponents<BoundingBox>(true);

            if (components.Length > 0)
            {
                boundingBoxs.AddRange(components);
            }
        }

        foreach (var boundingBox in boundingBoxs)
        {
            foreach (var otherBoundingBox in boundingBoxs)
            {
                if (boundingBox.Owner == otherBoundingBox.Owner)
                {
                    continue;
                }

                if (!boundingBox.Bounds.IntersectsWith(otherBoundingBox.Bounds))
                {
                    boundingBox.ExitCollisionBox(otherBoundingBox);
                    otherBoundingBox.ExitCollisionBox(boundingBox);

                    continue;
                }

                boundingBox.BindCollisionBox(otherBoundingBox);
                otherBoundingBox.BindCollisionBox(boundingBox);
            }
        }
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

        gameObject.Destroy();
        GameObjects.Remove(gameObject);

        if (OnDestoryGameObject != null)
        {
            OnDestoryGameObject.Invoke(gameObject);
        }
    }

}