using System.Numerics;
using BlazorGameFramework;

namespace FightGame;

public class BulletManager
{
    private List<Bullet> _bulletList = new();

    private const int CLEAR_COUNT = 40;         // 空闲对象数量临界值
    private const int MAX_COUNT = 100;           // 最大空闲对象数量
    private const int MIN_COUNT = 25;           // 最小空闲对象数量

    private GameObject _owner;

    public BulletManager(GameObject owner)
    {
        _owner = owner;
    }

    public async ValueTask Update(GameContext game)
    {
        //foreach (var bullet in _bulletList)
        //{
        //    if (bullet != null)
        //    {
        //        await bullet.Update(game);
        //    }
        //}
    }

    public Bullet GetOrAddBullet(Vector2 direction)
    {
        Bullet bullet = new()
        {
            Speed = 150 * direction.X,
        };

        /* 调整子弹位置，在人物中间发射 */
        var localPos = bullet.Transform.LocalPosition;
        bullet.Transform.LocalPosition = new Vector2(localPos.X, localPos.Y + _owner.Transform.Size.Height / 2);

        return bullet;

        //_bulletList.Add(bullet);
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    /// <param name="valueFrom">子弹发射起点</param>
    /// <param name="bulletType">子弹类型</param>
    /// <param name="entityAtk">发射子弹的生物的攻击力</param>
    /// <param name="aimEntity">攻击目标</param>
    //public void Fire(Vector2 valueFrom, int bulletType, int entityAtk, Actor aimEntity)
    //{
    //    BulletConfig bulletConfig = new();

    //    //bullet.Init(bulletType, entityAtk, aimEntity);

    //    //// 发射目的地
    //    //Vector3 valueTo = aimEntity.transform.position;

    //    //// 发射
    //    //bullet.Fire(valueFrom, valueTo);
    //}
}

