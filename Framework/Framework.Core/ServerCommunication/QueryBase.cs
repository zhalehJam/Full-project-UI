using System.Text;

namespace Framework.Core.ServerCommunication
{
    public class QueryBase<TQuery> where TQuery : class
    {
        public string ToQuery()
        {
            var properties = typeof(TQuery).GetProperties();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(this, null);
                if (propValue != null && !string.IsNullOrEmpty(propValue.ToString()))
                    stringBuilder.Append($"&{prop.Name}={propValue}");
            }
            return stringBuilder.ToString();
        }
    }
}
