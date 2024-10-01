using MyBookApp.Application.Contracts;
using MyBookApp.Core.Exceptions;

namespace MyBookApp.Application.Validators;

public static class BookValidator
{
    public static void AddValidation(this BookRequest bookRequest)
    {
        if (string.IsNullOrEmpty(bookRequest.Name) || bookRequest.Name.Length>50)
        {
            throw new ValidationException("Имя должно быть непустым, длиной до 50 символов");
        }
        
        if (string.IsNullOrEmpty(bookRequest.Description) || bookRequest.Description.Length>200)
        {
            throw new ValidationException("Описание должно быть непустым, длиной до 200 символов");
        }

        if (string.IsNullOrEmpty(bookRequest.Author) || bookRequest.Author.Length > 50)
        {
            throw new ValidationException("Имя должно быть непустым, длиной до 50 символов");
        }
    }

    public static void UpdateValidation(this BookRequest bookRequest,int id,bool isBookExists)
    {
        if (!isBookExists)
        {
            throw new BookNotFoundException(id);
        }

        if (bookRequest.Name != null)
        {
            if (string.IsNullOrEmpty(bookRequest.Name) || bookRequest.Name.Length>50)
            {
                throw new ValidationException("Имя должно быть непустым, длиной до 50 символов");
            }
        }

        if (bookRequest.Description != null)
        {
            if (string.IsNullOrEmpty(bookRequest.Description) || bookRequest.Description.Length>200)
            {
                throw new ValidationException("Описание должно быть непустым, длиной до 200 символов");
            }
        }

        if (bookRequest.Author != null)
        {
            if (string.IsNullOrEmpty(bookRequest.Author) || bookRequest.Author.Length > 50)
            {
                throw new ValidationException("Имя должно быть непустым, длиной до 50 символов");
            }
        }
    }
}