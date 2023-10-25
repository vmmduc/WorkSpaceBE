namespace Common.Extentions
{
    public static class Extentions
    {
        public static TValue? GetValue<TItem, TValue>(this TItem? source, Func<TItem, TValue> selector, TValue? defaultValue)
            where TItem : class
        {
            return source != null ? selector(source) : defaultValue;
        }
    }
}
