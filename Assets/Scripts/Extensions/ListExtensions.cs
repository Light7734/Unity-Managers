using System.Collections.Generic;

public static class ListExtensions
{
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static T First<T>(this List<T> list)
    {
        return list[0];
    }

    public static bool IsEmpty<T>(this List<T> list)
    {
        return list.Count == 0;
    }

}