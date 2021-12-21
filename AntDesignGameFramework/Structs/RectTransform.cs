namespace AntDesignGameFramework;

public struct RectTransform
{
    public float X { get; } = 0;
    public float Y { get; } = 0;
    public int Width { get; } = 0;
    public int Height { get; } = 0;

    public RectTransform(float x, float y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}
