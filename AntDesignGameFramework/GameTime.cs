namespace AntDesignGameFramework;

public class GameTime
{
    /// <summary>
    /// 游戏运行总时间（秒）
    /// </summary>
    public float TotalTime { get; set; }

    /// <summary>
    /// 距离上一帧的时间（秒）
    /// </summary>
    public float ElapsedTime { get; set; }
}