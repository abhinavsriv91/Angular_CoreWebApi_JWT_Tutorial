USE [Tutorial]
GO
/****** Object:  StoredProcedure [dbo].[usp_AddUser]    Script Date: 7/10/2022 11:08:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddUser] 
	@username	VARCHAR(50),
	@password	VARCHAR(50),
	@FullName	VARCHAR(100),
	@EmailAddress VARCHAR(50)
AS
BEGIN

	--SET NOCOUNT ON
	IF EXISTS(SELECT 1 FROM [User] WITH(NOLOCK) WHERE [name] = @username)
	BEGIN
		RAISERROR('User exist.', 16, 1)
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[User] VALUES(@username, @password, @FullName, @EmailAddress)
		SELECT @@ROWCOUNT
	END
END
GO
