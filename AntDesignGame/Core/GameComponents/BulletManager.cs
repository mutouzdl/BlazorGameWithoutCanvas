using System.Numerics;
using AntDesignGameFramework;

namespace AntDesignGame;

/// <summary>
/// 子弹管理器
/// 未完善，仅供演示
/// </summary>
public class BulletManager : BaseComponent
{
    private List<BulletGameObject> _bulletGameObjectList = new();

    private const int CLEAR_COUNT = 40;         // 空闲对象数量临界值
    private const int MAX_COUNT = 100;           // 最大空闲对象数量
    private const int MIN_COUNT = 25;           // 最小空闲对象数量

    public BulletGameObject GetOrAddBulletGameObject(int bulletId, Vector2 direction, int atk, GameObject target)
    {
        BulletGameObject bulletGameObject = new(typeof(Bullet), $"{bulletId}", target)
        {
            Speed = 200,
            Atk = atk,
        };

        bulletGameObject.Transform.Direction = direction;
        bulletGameObject.Init();

        _bulletGameObjectList.Add(bulletGameObject);

        return bulletGameObject;
    }

}
