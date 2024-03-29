USE [Tutorial]
GO
/****** Object:  StoredProcedure [dbo].[usp_SelectRoles]    Script Date: 7/10/2022 11:08:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SelectRoles] 
	@username	VARCHAR(50)
AS
	SET NOCOUNT ON

	SELECT R.Name FROM [UserRole] UR WITH(NOLOCK) 
	INNER JOIN [User] U WITH(NOLOCK) ON UR.UserId = U.Id
	INNER JOIN [Role] R WITH(NOLOCK) ON UR.RoleId = R.Id
	WHERE u.[name] = @username 
GO
