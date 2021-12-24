using System.Numerics;

namespace AntDesignGameFramework.Utility;


public static class Utility
{
    public static class Maths
    {
        /// <summary>
        /// 直线方程
        /// </summary>
        public static class LinerEquation
        {
            /// <summary>
            /// 已知两个点，计算斜率
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            public static float CountK(Vector2 p1, Vector2 p2)
            {
                return (p2.Y - p1.Y) / (p2.X - p1.X);
            }

            /// <summary>
            /// 已知斜率、距离、一个点，求另一个点
            /// </summary>
            /// <param name="k">斜率</param>
            /// <param name="d">距离</param>
            /// <param name="p1">已知点</param>
            /// <returns></returns>
            public static Vector2 GetP2(float k, float d, Vector2 p1)
            {
                var r_sq = d * d / (1 + k * k);
                var r = (float)Math.Sqrt(r_sq);
                var p2 = new Vector2(p1.X + r, p1.Y + k * r);

                return p2;
            }
        }
    }
}
