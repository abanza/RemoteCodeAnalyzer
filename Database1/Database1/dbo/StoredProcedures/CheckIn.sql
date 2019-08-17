CREATE PROCEDURE [dbo].[CheckIn]
	@UserId int,
	@FolderId int,
	@Filename nvarchar(200),
	@Data varbinary(max)
AS
	IF EXISTS (
		SELECT a.Id 
		FROM 
			Files a
			INNER JOIN [Folders] b on a.FolderId = b.Id
			INNER JOIN Users c on b.UserId = c.Id
		WHERE 
			c.Id = @UserId
			AND b.Id = @FolderId
			AND a.Name = @Filename
	)
	BEGIN
		-- This file already exists for this user, mark as checked in
		UPDATE a
		SET CheckedOutBy = NULL
		FROM 
			Files a
			INNER JOIN [Folders] b on a.FolderId = b.Id
			INNER JOIN Users c on b.UserId = c.Id
		WHERE 
			c.Id = @UserId
			AND b.Id = @FolderId
			AND a.Name = @Filename
	END
	ELSE
	BEGIN
		INSERT INTO Files(Name, FolderId, FileData)
		VALUES(@Filename, @FolderId, @Data)

		RETURN @@IDENTITY
	END
RETURN 0
