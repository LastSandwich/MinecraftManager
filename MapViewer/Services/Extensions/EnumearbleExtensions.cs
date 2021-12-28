namespace MapViewer.Services.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> UnWrap<T>(this IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                yield return item;
            }
        }
    }
}
