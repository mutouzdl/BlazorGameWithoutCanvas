using System.Numerics;

namespace FightGame;

public partial class BulletManager
{
    private List<BulletConfig> bulletConfigList = new();
    private Dictionary<BulletConfig, Bullet> bulletMap = new();

    /* 子弹对象的uid和Bullet组件对应关系 */
    private Dictionary<int, BulletConfig> bulletUIDDic = new Dictionary<int, BulletConfig>();

    private const int CLEAR_COUNT = 40;         // 空闲对象数量临界值
    private const int MAX_COUNT = 100;           // 最大空闲对象数量
    private const int MIN_COUNT = 25;           // 最小空闲对象数量

    /// <summary>
    /// 逻辑处理，由其他对象负责调用
    /// </summary>
    public void Logic(double deltaTime)
    {
        foreach (var bullet in bulletMap.Values)
        {
            if (bullet != null)
            {
                bullet.Logic(deltaTime);
            }
        }
    }

    public void Fire()
    {
        BulletConfig bulletConfig = new()
        {
            Speed = 150,
        };

        bulletConfigList.Add(bulletConfig);

        StateHasChanged();
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    /// <param name="valueFrom">子弹发射起点</param>
    /// <param name="bulletType">子弹类型</param>
    /// <param name="entityAtk">发射子弹的生物的攻击力</param>
    /// <param name="aimEntity">攻击目标</param>
    public void Fire(Vector2 valueFrom, int bulletType, int entityAtk, Actor aimEntity)
    {
        BulletConfig bulletConfig = new();

        bulletMap[bulletConfig] = null;
        bulletConfigList.Add(bulletConfig);

        //bullet.Init(bulletType, entityAtk, aimEntity);

        //// 发射目的地
        //Vector3 valueTo = aimEntity.transform.position;

        //// 发射
        //bullet.Fire(valueFrom, valueTo);
    }

}
