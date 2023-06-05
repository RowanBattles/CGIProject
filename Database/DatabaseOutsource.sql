create table dbo.Categories
(
    Vehicle_ID   int identity
        primary key,
    Vehicle_Name nvarchar(255) not null,
    Emission     int           not null
)
go

create table dbo.Groups
(
    id   int identity
        primary key,
    name varchar(255) not null
)
go

create table dbo.Journeys
(
    Journey_ID     int identity
        primary key,
    User_ID        int  not null,
    Total_Distance int,
    Total_Emission int,
    Start          varchar(255),
    [End]          varchar(255),
    Date           date not null,
    Score          int
)
go

create table dbo.Stopovers
(
    Stopover_ID int identity
        primary key,
    Vehicle_ID  int          not null
        references dbo.Categories
            on delete cascade,
    Journey_ID  int          not null
        references dbo.Journeys
            on delete cascade,
    Distance    int          not null,
    Start       varchar(255) not null,
    [End]       varchar(255) not null,
    Emission    int          not null
)
go

create table dbo.Users
(
    User_ID  int identity
        constraint PK_Users
            primary key,
    UUID     nvarchar(50) not null,
    FullName nvarchar(50) not null,
    Score    int          not null
)
go

create table dbo.GroupUsers
(
    user_id       int           not null
        constraint FK_GroupUsers_Users
            references dbo.Users,
    group_id      int           not null
        constraint FK_GroupUsers_Groups
            references dbo.Groups,
    user_is_admin bit default 0 not null,
    constraint PK_GroupUsers
        primary key (user_id, group_id)
)
go

