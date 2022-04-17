namespace DotNetQuiz.BLL.Tests.Helpers;

internal static class ArrayHelper
{
    public static T GetLowest<T>(this IEnumerable<T> source, IComparer<T> comparator) => Sort(source, comparator).First();
    public static T GetLowest<T>(this IEnumerable<T> source, Comparison<T> comparator) => Sort(source, comparator).First();
    public static T GetHighest<T>(this IEnumerable<T> source, IComparer<T> comparator) => Sort(source, comparator).First();
    public static T GetHighest<T>(this IEnumerable<T> source, Comparison<T> comparator) => Sort(source, comparator).First();

    public static T[] Sort<T>(this IEnumerable<T> source, IComparer<T>? comparator)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparator);

        var arraySource = source.ToArray();
        Array.Sort(arraySource, comparator);

        return arraySource;
    }

    public static T[] Sort<T>(this IEnumerable<T> source, Comparison<T>? comparator)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparator);

        var arraySource = source.ToArray();
        Array.Sort(arraySource, comparator);

        return arraySource;
    }
}
