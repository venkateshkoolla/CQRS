namespace Ocas.Domestic.Apply.Models.Lookups
{
    public class LookupEnumItem<T>
        where T : struct
    {
        public T Id { get; set; }
        public string Label { get; set; }
    }
}
