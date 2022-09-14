--use dapperDB 

--go

--create proc create_user_table
--	as
--	begin
--		CREATE TABLE users (id int NOT NULL IDENTITY(1,1) PRIMARY KEY,userName varchar(255) NOT NULL ,firstName varchar(255) NOT NULL,lastName varchar(255) NOT NULL, email varchar(255) NOT NULL)
--	end

--go

--create proc drop_user_table
--	as
--	begin
--		DROP TABLE users
--	end

--go

--create proc insert_user
--	@Id int = NULL,
--	@FirstName nvarchar(100),
--	@LastName nvarchar(100),
--	@UserName nvarchar(100),
--	@Email nvarchar(100)
--	as
--	begin
--		insert into [dbo].[Users] ( firstName , lastName , userName , email ) values (@FirstName, @LastName, @UserName, @Email);
--	end

--go

--create proc update_user
--	(
--	@Id int,
--	@FirstName nvarchar(100),
--	@LastName nvarchar(100),
--	@UserName nvarchar(100),
--	@Email nvarchar(100)
--	)
--	as
--	begin
--		UPDATE [dbo].[Users] SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, Email=@Email WHERE Id=@Id
--	end

--go

--create proc get_all_or_single
--	@Id int = NULL
--	as
--	if(@Id IS NOT NULL)
--		begin
--			SELECT * FROM [dbo].[Users] WHERE Id=@Id
--		end
--	else
--		begin
--			SELECT * FROM [dbo].[Users]
--		end

--go

--create proc delete_user
--	@Id int
--	as
--	begin
--		DELETE FROM [dbo].[Users] WHERE Id=@Id
--	end

--go

--exec [dbo].[create_user_table]
--exec [dbo].[insert_user] @FirstName=N'Vahid', @LastName=N'Nikbakht' , @UserName=N'nvhub' , @Email=N'vniki32@gmail.com'
--exec [dbo].[insert_user] @FirstName=N'Reza', @LastName=N'Rezai' , @UserName=N'Rez' , @Email=N'Reza@gmail.com'
--exec [dbo].[update_user] @Id=2 ,@FirstName=N'Reza new', @LastName=N'Rezai' , @UserName=N'Rez' , @Email=N'Reza@gmail.com'
--exec [dbo].[get_all_or_single] 
--exec [dbo].[get_all_or_single] @Id=2
--exec [dbo].[delete_user] @Id=1
--exec [dbo].[get_all_or_single] 
----exec [dbo].[drop_table]

--go
