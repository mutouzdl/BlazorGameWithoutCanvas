namespace AntDesignGame;

public class FightProperty
{
    /// <summary>
    /// 血量
    /// </summary>
    public int HP { get; set; }
    /// <summary>
    /// 攻击
    /// </summary>
    public int Atk { get; set; }
    /// <summary>
    /// 攻击间隔（秒）
    /// </summary>
    public float AtkDelay { get; set; }
    /// <summary>
    /// 攻击范围
    /// </summary>
    public int AtkRange { get; set; }
    /// <summary>
    /// 攻击数量
    /// </summary>
    public int AtkNum { get; set; }
    /// <summary>
    /// 防御
    /// </summary>
    public int Def { get; set; }
    /// <summary>
    /// 移动速度
    /// </summary>
    public int MoveSpeed { get; set; }
}