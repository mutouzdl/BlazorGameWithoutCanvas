using AntDesignGameFramework;

namespace AntDesignGame;


public class SpriteGameObject : GameObject
{
    public string AssetName { get; set; }

    public SpriteGameObject(Type webComponentType) : base(webComponentType)
    {
    }
}
