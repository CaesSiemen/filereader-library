using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace FileReaderLibrary.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue<T>(this T value)
        {
            return typeof(T).GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }
    }
}
