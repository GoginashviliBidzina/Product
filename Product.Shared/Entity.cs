namespace Template.Shared
{
    public class Entity : IThrowsDomainExeption
    {
        public int Id { get; protected set; }

        public void ThrowDomainException(string message)
        {
            throw new DomainException(message);
        }
    }
}
