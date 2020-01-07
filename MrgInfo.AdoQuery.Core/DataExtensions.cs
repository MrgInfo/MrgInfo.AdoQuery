namespace Sda.Query
{
    public static class DataExtensions
    {
        public static object[][] ToData<TValue>(this TValue value) => new[] { new[] { (object)value } };
    }
}
