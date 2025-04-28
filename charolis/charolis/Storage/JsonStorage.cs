using System.Text.Json;
using charolis.Entity;

namespace charolis.Storage;

public class JsonStorage<T> : IDataStorage<T> where T : BaseEntity
{
    private readonly string _filePath;
        private List<T> _items;

        public JsonStorage(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _items = LoadFromFile();
        }

        private List<T> LoadFromFile()
        {
            // Якщо файла нема — створюємо папку й порожній файл із порожнім масивом
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<T>();
            }

            var json = File.ReadAllText(_filePath);

            // Якщо файл порожній або містить тільки пробіли — повертаємо порожній список
            if (string.IsNullOrWhiteSpace(json))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<T>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (JsonException)
            {
                // Якщо в файлі кривий JSON — перезапишемо його порожнім масивом
                File.WriteAllText(_filePath, "[]");
                return new List<T>();
            }
        }

        public List<T> GetAll() => _items;

        public T? GetById(int id) =>
            _items.FirstOrDefault(e => e.Id == id);

        public void Add(T entity)
        {
            if (entity.Id == 0)
            {
                var nextId = _items.Any() ? _items.Max(e => e.Id) + 1 : 1;
                entity.Id = nextId;
            }

            _items.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            var idx = _items.FindIndex(e => e.Id == entity.Id);
            if (idx >= 0)
            {
                _items[idx] = entity;
                Save();
            }
        }

        public void Delete(int id)
        {
            var item = _items.FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                _items.Remove(item);
                Save();
            }
        }

        public void Save()
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_items, opts);
            File.WriteAllText(_filePath, json);
        }
}