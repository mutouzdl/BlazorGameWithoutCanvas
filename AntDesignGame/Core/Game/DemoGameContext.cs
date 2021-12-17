
using AntDesignGameFramework;

namespace AntDesignGame;


public class DemoGameContext : GameContext
{
    protected override async ValueTask Update()
    {
        foreach (var gameObject in GameObjects)
        {
            await gameObject.Update(this);
        }
    }
}
