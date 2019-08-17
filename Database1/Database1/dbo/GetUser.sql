CREATE PROCEDURE [dbo].[GetUser]
	@Id int
AS
	SELECT *
	FROM
		Users
	WHERE
		Id = @Id
RETURN 0
