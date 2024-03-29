USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[EmailUnCheckedOutCarsToManagers]    Script Date: 3/12/2024 10:08:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[EmailUnCheckedOutCarsToManagers]
	@LOC varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @vErrCode int;
DECLARE @vErrMsg nvarchar(4000);
DECLARE @vHTMLTable nvarchar(max);

DECLARE @vSQL  nvarchar(max);
DECLARE @vParams nvarchar(max);
DECLARE @EmailTitle nvarchar(300);

set @vParams = '';

SET @vSQL = 'SELECT TOP (1000) [ID]
      ,[FullName]
      ,[PermissionCode]
      ,[LocCode]
      ,[Mall]
      ,[State]
  FROM [Checklists].[dbo].[Locations_lkup] ' ;

	  print @vSQL;

EXEC spQueryResultAsHtmlTable @vErrCode OUTPUT, @vErrMsg OUTPUT, @vSQL, @vParams, @vHTMLTable OUTPUT

print @vHTMLTable;

set @EmailTitle = 'Cars to Be Checked Out: ' + @LOC;

select @vHTMLTable

EXEC msdb.dbo.sp_send_dbmail
   @recipients = 'burroughsd@fitzmall.com'
   ,@subject = @EmailTitle
   ,@body_format = 'HTML'
   ,@body = @vHTMLTable

END
