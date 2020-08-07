public abstract class GameCommand<T>
{
    public abstract void Execute(T parameter);

    protected static void Delete(ICellComponent zelle)
    {
        UnityEngine.Object.DestroyImmediate(zelle.GameObject);
    }
}