using System;

using System.Text.Json;

namespace Framework.Entity
{
    public abstract class EntityBase<T>
    {
        public T Clone()
        {
            var obj = (T)Convert.ChangeType(this, typeof(T));
            string json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
