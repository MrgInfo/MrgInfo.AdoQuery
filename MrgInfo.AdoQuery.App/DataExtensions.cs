using System.Collections.Generic;

namespace  MrgInfo.AdoQuery.App
{
    public static class DataExtensions
    {
        public static IList<IList<object?>>? ToData<TValue>(this TValue value) => new IList<object?>[] { new object?[] { value } };
    }
}
