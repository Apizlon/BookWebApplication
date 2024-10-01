UPDATE "Books"
SET
    "Name" = CASE WHEN @Name IS NULL THEN "Name" ELSE @Name END,
    "Description" = CASE WHEN @Description IS NULL THEN "Description" ELSE @Description END,
    "Author" = CASE WHEN @Author IS NULL THEN "Author" ELSE @Author END
WHERE "Id" = @Id;