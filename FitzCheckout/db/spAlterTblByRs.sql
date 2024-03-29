USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[spAlterTblByRs]    Script Date: 3/12/2024 10:09:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAlterTblByRs]
    @ErrCode  INT OUT,
    @ErrMsg   VARCHAR(4000) OUT,    
    @Sql      NVARCHAR(MAX),         /* Query stmt  */
    @Params   NVARCHAR(MAX) = NULL,  /* Query parameters (like in sp_executesql) */
    @Tbl      NVARCHAR(256),         /* Table name */
    @DummyCol NVARCHAR(256),         /* Dummy column name (will be removed) */
    @PopulateTable BIT = NULL        /* If 1, then populate altered table by @Sql query data */ 
AS
    /* Alters table by recordset to be used. Populates data, if required. */    
BEGIN
    SET NOCOUNT ON;
    SET ARITHABORT ON;

    BEGIN TRY

        DECLARE @ERR_CODE_OK        INT =   0
            ,   @ERR_CODE_FAILURE   INT =   50000;

        SET @ErrCode = @ERR_CODE_OK;

        
        IF NULLIF(LTRIM(RTRIM(@Tbl)), '') IS NULL THROW @ERR_CODE_FAILURE, '@Tbl is empty', 1;
        IF NULLIF(LTRIM(RTRIM(@DummyCol)), '') IS NULL THROW @ERR_CODE_FAILURE, '@DummyCol is empty', 1;


        IF [dbo].[fnValidateDynamicSql](@Sql, @Params) IS NOT NULL
        BEGIN
            SET @ErrMsg = 'Invalid @Sql received: ' + [dbo].[fnValidateDynamicSql](@Sql, @Params);
            ;THROW @ERR_CODE_FAILURE, @ErrMsg, 1;
        END;


        DECLARE @AlterStmt      NVARCHAR(MAX) = SPACE(0);
        DECLARE @RemColStmt     NVARCHAR(MAX) = SPACE(0);   
             
        --  prepare existing table alter Stmt by previuos rs structure
        SET @AlterStmt = 'ALTER TABLE ' + @tbl + ' ADD ' + CHAR(13);

        ;WITH [rsStructure] AS (
            SELECT
                    [name]
                ,   [system_type_name]
                ,   [is_nullable]            
            FROM [sys].[dm_exec_describe_first_result_set](
                            @Sql
                        ,   @Params
                        ,   0
            )       
        )
        SELECT 
             @AlterStmt += QUOTENAME([name]) + SPACE(1) + [system_type_name] + IIF([is_nullable] = 0, ' NOT NULL' , SPACE(0)) + ',' + CHAR(13)
        FROM [rsStructure];

        SET @AlterStmt = LEFT(@AlterStmt, LEN(@AlterStmt) - 2);

        --  finally update table structure
        EXEC [sys].[sp_executesql] @AlterStmt; 
        

        --  remove dummy column
        SET @RemColStmt = 'ALTER TABLE ' + @tbl + ' DROP COLUMN ' + @DummyCol;
        EXEC [sys].[sp_executesql] @RemColStmt; 

        --  populate table with @Sql statement data
        IF @PopulateTable = 1
        BEGIN
            EXEC('INSERT INTO ' + @tbl + ' ' + @sql);
        END;
        
        
    END TRY
    BEGIN CATCH
    
        /* Use some error formatting sp instead */
        SELECT  @ErrCode    =   ERROR_NUMBER()
            ,   @ErrMsg     =   ERROR_MESSAGE();

    END CATCH

    RETURN @ErrCode;

END


