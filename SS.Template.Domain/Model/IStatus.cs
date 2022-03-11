namespace SS.Template.Domain.Model
{
    public interface IStatus<T>
    {
        T Status { get; set; }
    }
}