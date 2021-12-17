namespace AntDesignGameFramework;

public interface IComponent
{
    ValueTask Update(GameContext game);

    public void SetOwner(GameObject owner);

}