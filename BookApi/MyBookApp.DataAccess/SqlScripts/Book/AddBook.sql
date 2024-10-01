INSERT INTO "Books" ("Name","Description","Author")
VALUES (@Name,@Description,@Author)
RETURNING "Id";