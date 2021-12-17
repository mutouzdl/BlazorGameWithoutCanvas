using AntDesignGameFramework;

namespace AntDesignGame;

public class PropertyBarGameObject : GameObject
{
    public decimal MaxValue { get; set; } = 100;
    public decimal CurrentValue { get; set; } = 0;
    public EnumPropertyBarType PropertyBarType { get; set; } = EnumPropertyBarType.HP;

    public PropertyBarGameObject(Type webComponentType) : base(webComponentType)
    {
    }

}
