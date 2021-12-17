namespace AntDesignGame;

public partial class PropertyBar : GameWebComponent<PropertyBarGameObject>
{
    private decimal Percent => GameObject.CurrentValue / GameObject.MaxValue * 100;
    private string StrokeColor => GameObject.PropertyBarType switch
    {
        EnumPropertyBarType.HP => "#FF4136",
        EnumPropertyBarType.MP => "#0074D9",
        _ => "#FF4136",
    };

    private string SizeStyle => $"width:{GameObject.Transform.Size.Width}px;";
}
