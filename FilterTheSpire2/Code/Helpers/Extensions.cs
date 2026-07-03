namespace FilterTheSpire2.Code.Helpers;

public static class Extensions
{
    public static bool In<T>(this T @this, params T[] values)
    {
        return values.Contains(@this);
    }
}