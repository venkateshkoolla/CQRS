namespace Ocas.Domestic.Models
{
    public interface IModel<T>
    {
        T Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string LocalizedName { get; set; }
    }

    public abstract class Model<T> : IModel<T>
    {
        public T Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get; set; }
    }
}
