using System.Numerics;
using AntDesignGameFramework;

namespace AntDesignGame;

public class BulletManager : BaseComponent
{
    private List<BulletGameObject> _bulletGameObjectList = new();

    private const int CLEAR_COUNT = 40;         // 空闲对象数量临界值
    private const int MAX_COUNT = 100;           // 最大空闲对象数量
    private const int MIN_COUNT = 25;           // 最小空闲对象数量

    //public override ValueTask Update(GameContext game)
    //{
    //}

    public BulletGameObject GetOrAddBulletGameObject(int bulletId, Vector2 direction, int atk, GameObject target)
    {
        BulletGameObject bulletGameObject = new(typeof(Bullet), $"{bulletId}")
        {
            Speed = 200,
            Atk = atk,
            Target = target,
        };

        bulletGameObject.Transform.Direction = direction;

        _bulletGameObjectList.Add(bulletGameObject);

        return bulletGameObject;
    }

}
