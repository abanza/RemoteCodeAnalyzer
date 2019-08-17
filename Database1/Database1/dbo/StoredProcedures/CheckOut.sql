CREATE PROCEDURE [dbo].[CheckOut]
	@UserId int,
	@FolderId int,
	@Filename nvarchar(200)
AS
	SELECT
		a.Id,
		a.Name as Filename,
		a.FileData as Data,
		b.Name as ProjectName,
		c.Username as Username
	FROM
		Files a
		INNER JOIN [Folders] b on ISNULL(a.FolderId, 0) = ISNULL(b.Id, 0)
		INNER JOIN Users c on b.UserId = c.Id
	WHERE
		a.Name = @Filename
		AND b.Id = @FolderId
		AND c.Id = @UserId

	IF (@@ROWCOUNT > 0)
	BEGIN
		UPDATE a
		SET a.CheckedOutBy = c.Id
		FROM
			Files a
			INNER JOIN [Folders] b on a.FolderId = b.Id
			INNER JOIN Users c on b.UserId = c.Id
		WHERE
			a.Name = @Filename
			AND ISNULL(b.Id, 0) = ISNULL(@FolderId, 0)
			AND c.Id = @UserId
	END
		
RETURN 0
