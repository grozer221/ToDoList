create table [dbo].[Users]
(
	[Id] int identity(1, 1) not null primary key,
	[Email] nvarchar(255) not null,
	constraint AK_Users_Email unique([Email]),
	[Password] nvarchar(255) not null,
	[IsEmailConfirmed] tinyint not null,
	[CreatedAt] datetime not null,
	[UpdatedAt] datetime not null,
)

create table [dbo].[Categories]
(
	[Id] int identity(1, 1) not null primary key,
	[Name] nvarchar(255) not null,
	constraint AK_Categories_Name unique([Name]),
	[CreatedAt] datetime not null,
	[UpdatedAt] datetime not null,
	[UserId] int foreign key ([UserId]) references Users(Id) on delete set null,
)

create table [dbo].[ToDos]
(
	[Id] int identity(1, 1) not null primary key,
	[Name] nvarchar(255) not null,
	[Deadline] datetime,
	[IsDone] tinyint default 0,
	[CreatedAt] datetime not null,
	[UpdatedAt] datetime not null,
	[CategoryId] int foreign key ([CategoryId]) references Categories(Id) on delete set null,
	[UserId] int foreign key ([UserId]) references Users(Id) on delete set null,
)