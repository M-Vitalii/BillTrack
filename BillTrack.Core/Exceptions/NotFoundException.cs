namespace BillTrack.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        private static readonly string StaticMessage = "Entity with ID not found.";
        private Guid EntityId { get; }

        public NotFoundException(Guid entityId) 
            : base(StaticMessage)
        {
            EntityId = entityId;
        }

        public NotFoundException(Guid entityId, Exception innerException) 
            : base(StaticMessage, innerException)
        {
            EntityId = entityId;
        }

        public override string ToString()
        {
            return $"{Message} (ID: {EntityId})";
        }
    }
}