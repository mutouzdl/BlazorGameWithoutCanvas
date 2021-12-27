namespace AntDesignGame;


public class RelationUtility
{
    private class RelationObj
    {
        public string Tag1 { get; private set; }
        public string Tag2 { get; private set; }
        public EnumActorRelation Relation { get; private set; }

        public RelationObj(string tag1, string tag2, EnumActorRelation relation)
        {
            Tag1 = tag1;
            Tag2 = tag2;
            Relation = relation;
        }
    }

    private static List<RelationObj> _relations = new()
    {
        new RelationObj(Const.Tags.Hero, Const.Tags.Hero, EnumActorRelation.友好),
        new RelationObj(Const.Tags.Hero, Const.Tags.Monster, EnumActorRelation.敌对),
        new RelationObj(Const.Tags.Monster, Const.Tags.Monster, EnumActorRelation.友好),
        new RelationObj(Const.Tags.Monster, Const.Tags.Hero, EnumActorRelation.敌对),
    };

    /// <summary>
    /// 获取两个角色标签的阵营关系
    /// </summary>
    /// <param name="tag1"></param>
    /// <param name="tag2"></param>
    /// <returns></returns>
    public static EnumActorRelation GetRelation(string tag1, string tag2)
    {
        var relationObj = _relations.SingleOrDefault(t => t.Tag1 == tag1 && t.Tag2 == tag2);

        if (relationObj == null)
        {
            return EnumActorRelation.中立;
        }

        return relationObj.Relation;
    }
}
