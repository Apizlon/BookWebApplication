namespace PostApi.Core.Exceptions;

public class PostNotFoundException : NotFoundException
{
    public PostNotFoundException(int id) : base($"Книга с id {id} не найдена.")
    {
        
    }
}