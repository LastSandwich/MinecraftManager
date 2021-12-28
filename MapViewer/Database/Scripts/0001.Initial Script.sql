IF SCHEMA_ID(N'MapViewer') IS NULL EXEC(N'CREATE SCHEMA [MapViewer];');
GO

create table [MapViewer].World
(
    Id int identity,
    CreatedDate datetime2 not null,
    UpdatedDate datetime2 not null,
    RenderedDate datetime2 null,
    Name varchar(50) not null,
    BackupPath varchar(512) not null,
    OutputPath varchar(512) not null
)

go