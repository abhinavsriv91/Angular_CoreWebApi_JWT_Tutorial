USE [Tutorial]
GO
/****** Object:  StoredProcedure [dbo].[usp_AuthenticateUser]    Script Date: 7/10/2022 11:08:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AuthenticateUser] 
	@username	VARCHAR(50),
	@password	VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON
	IF EXISTS(SELECT 1 FROM [User] WITH(NOLOCK) WHERE [name] = @username AND [password] = @password)
	BEGIN
		SELECT [Name], [FullName], [EmailAddress] FROM dbo.[User] 
		WHERE [name] = @username

		SELECT R.Id, R.[Name] FROM dbo.[User] U
		INNER JOIN UserRole UR ON  UR.UserId = u.Id 
		INNER JOIN Role R ON R.Id = UR.RoleId 
		WHERE U.[name] = @username
	END
	ELSE
	BEGIN
		RAISERROR('Not Authenticated', 16, 1)
	END
END
GO
