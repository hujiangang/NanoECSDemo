using NanoEcs;


public static class Extensions
{
    public static int GetComponentIndex<T>()
    {
        string name = typeof(T).Name;
        int item = -1;
        for (int i = 0; i < GameComponentsMap.Names.Length; i++)
        {
            if (name.Equals(GameComponentsMap.Names[i]))
            {
                item = i;
                break;
            }
        }
        return item;
    }

}
public partial class GameWithBuilder : WithBuilder<GameEntity>
{
    public GameGroup WithGroup<T>()
    {
        int item = Extensions.GetComponentIndex<T>();
        if (item >= 0)
        {
            group.WithTypes.Add(item);
        }
        return group as GameGroup;
    }
}

public partial class GameEntity
{
    public GameEntity Remove<T>()
    {
        if (IsReserved) throw new System.Exception("Unable to remove component from reserved entity");
        int item = Extensions.GetComponentIndex<T>();
        if (item >= 0)
        {
            RemoveComponentOfIndex(item);
        }
        return this;
    }

}
