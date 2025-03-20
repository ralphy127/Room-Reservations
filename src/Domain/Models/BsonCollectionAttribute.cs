namespace RoomRes.Domain.Models {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BsonCollectionAttribute : Attribute {
        private string _collectionName ="";
        public string? CollectionName { 
            get => _collectionName;
            private set => _collectionName = value ?? throw new ArgumentNullException(nameof(CollectionName));
        }

        public BsonCollectionAttribute(string collectionName) {
            CollectionName = collectionName;
        }
    }
}