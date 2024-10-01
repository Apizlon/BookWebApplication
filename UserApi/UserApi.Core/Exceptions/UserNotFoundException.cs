namespace UserApi.Core.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int id) : base($"Книга с id {id} не найдена.")
    {
        
    }
}