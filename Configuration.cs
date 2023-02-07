namespace Tabler
{
    public class Configuration : Singleton<Configuration>
    {
        IConfiguration configuration;

        public Configuration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Properties/settings.json");
            configuration = configurationBuilder.Build();
        }

        public string this[string key] => configuration[key];


        public T Get<T>(string key)
        {
            string value = configuration[key];
            if (typeof(T) == typeof(string))
            {
                if (value is T result)
                    return result;
            }
            else if (typeof(T) == typeof(int))
            {
                int valueInt = int.Parse(value);
                if (valueInt is T result)
                    return result;
            }
            else if (typeof(T) == typeof(float))
            {
                float valueInt = float.Parse(value);
                if (valueInt is T result)
                    return result;
            }
            throw new InvalidOperationException($"{key} is not a {nameof(T)}.");
        }

        internal string GetPostPath(string postId, string extension)
        {
            extension = extension.Replace(".", "");
            string path = this["postsPath"];
            string filepath = Path.Combine(path, $"{postId}.{extension}");
            if (File.Exists(filepath))
            {
                return filepath;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
