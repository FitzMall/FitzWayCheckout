USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[spQueryResultAsHtmlTable]    Script Date: 3/12/2024 10:09:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spQueryResultAsHtmlTable]
    @ErrCode    INT             OUT
,   @ErrMsg     NVARCHAR(4000)  OUT
,   @Sql        NVARCHAR(MAX)
,   @Params     NVARCHAR(MAX)
,   @HtmlTable  NVARCHAR(MAX)   OUT
AS
/*  Makes Html table by result, returned by provided @Query 
 */
BEGIN
    
    SET NOCOUNT ON;
    SET ARITHABORT ON;
        
    BEGIN TRY

        DECLARE @ERR_CODE_OK        INT =   0
            ,   @ERR_CODE_FAILED    INT =   50000;          

        SET @ErrCode = @ERR_CODE_OK;

        DECLARE @HtmlAsHml  XML
            ,   @ColumnList NVARCHAR(MAX)   =   SPACE(0);       

        IF NULLIF(LTRIM(RTRIM(@Sql)), SPACE(0)) IS NULL THROW @ERR_CODE_FAILED, 'Empty @Query received', 1;
                    
                
        IF OBJECT_ID('tempdb..#QueryResult') IS NOT NULL DROP TABLE [#QueryResult];
        CREATE TABLE [#QueryResult] ([dummy_col] BIT);

        EXEC [dbo].[spAlterTblByRs]
            @ErrCode        =   @ErrCode        OUT
        ,   @ErrMsg         =   @ErrMsg         OUT
        ,   @Sql            =   @Sql            
        ,   @Params         =   @Params         
        ,   @Tbl            =   '#QueryResult'  
        ,   @DummyCol       =   'dummy_col'     
        ,   @PopulateTable  =   1;

        IF @ErrCode <> 0 THROW @ErrCode, @ErrMsg, 1;
                    
            
        SELECT @ColumnList += IIF([column_ordinal] = 1, SPACE(0), ',') + '[td] = [' + [name] + ']'
        FROM [sys].[dm_exec_describe_first_result_set](
            @Sql    /*  @tsql                       */
        ,   @Params /*  @params                     */
        ,   0       /*  @browse_information_mode    */      
        )       
        ORDER BY [column_ordinal] ASC;


        DECLARE @h  XML
        ,       @d  XML;    

        /* Prepare headers */
        ;WITH [headers] AS (
            SELECT [h] = CONVERT(XML, (SELECT 
                [th] = [name] 
            FROM [sys].[dm_exec_describe_first_result_set](
                @Sql    /*  @tsql                       */
            ,   @Params /*  @params                     */
            ,   0       /*  @browse_information_mode    */      
            )   
            ORDER BY [column_ordinal] ASC
            FOR XML PATH(''), ROOT('tr')))
        )
        SELECT @h = [h] FROM [headers];
            
        
        /* Prepare rows */
        SET @sql = N'
        ;WITH [data] AS (
            SELECT [d] = (SELECT    
                ' + @ColumnList + '
            FROM [#QueryResult] 
            FOR XML RAW (''tr''), ELEMENTS XSINIL, TYPE)
        )       
        SELECT @d = [d] FROM [data]';

        SET @params = N'@d xml output';

        EXECUTE [sp_executesql] 
            @stmt   =   @sql
        ,   @params =   @params     
        ,   @d      =   @d      OUTPUT;
        

        /* Make table html */
        SET  @HtmlAsHml = CONVERT(XML, (SELECT [*] = @h, [*] = @d FOR XML PATH('table')));

        SET @HtmlAsHml.modify('insert attribute cellpadding {"2"} into (table)[1]')
        SET @HtmlAsHml.modify('insert attribute cellspacing {"2"} into (table)[1]')
        SET @HtmlAsHml.modify('insert attribute border {"1"} into (table)[1]')
    
        
        /* Prepare value to be returned */
        SET @HtmlTable = CONVERT(NVARCHAR(MAX), @HtmlAsHml);
        
               
    END TRY
    BEGIN CATCH             

        /* Use some error formatting sp instead */
        SELECT  @ErrCode    =   ERROR_NUMBER()
            ,   @ErrMsg     =   ERROR_MESSAGE();

    END CATCH;
    
    RETURN @ErrCode;
    
END;
