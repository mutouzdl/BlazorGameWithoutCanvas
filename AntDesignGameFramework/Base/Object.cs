namespace AntDesignGameFramework;

public class Object
{
    public string Uid { get; set; } = Guid.NewGuid().ToString();

    public override int GetHashCode()
    {
        return Uid.GetHashCode();
    }
}
