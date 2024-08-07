USE [master]
GO

CREATE DATABASE [DrinkStore]
GO

--Coupon
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Coupon]    Script Date: 8/8/2024 1:40:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Coupon](
	[coupon_id] [int] IDENTITY(1,1) NOT NULL,
	[coupon_code] [varchar](50) NOT NULL,
	[coupon_name] [varchar](255) NOT NULL,
	[discount_type] [varchar](50) NOT NULL,
	[discount_value] [decimal](10, 2) NOT NULL,
	[min_purchase_amount] [decimal](10, 2) NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[coupon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


--Customer
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 8/8/2024 12:59:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[customer_id] [int] IDENTITY(1,1) NOT NULL,
	[full_name] [nvarchar](100) NOT NULL,
	[phone_number] [varchar](15) NULL,
	[email] [varchar](100) NULL,
	[address] [nvarchar](max) NULL,
	[birth_date] [date] NULL,
	[is_registered] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [is_registered]
GO


--Promotion
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Promotion]    Script Date: 8/8/2024 1:00:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Promotion](
	[promotion_id] [int] IDENTITY(1,1) NOT NULL,
	[promotion_name] [varchar](255) NOT NULL,
	[promotion_type] [varchar](50) NOT NULL,
	[discount_value] [decimal](10, 2) NULL,
	[buy_quantity] [int] NULL,
	[free_quantity] [int] NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[description] [nvarchar](max) NULL,
	[priority] [int] NOT NULL,
	[status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[promotion_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Promotion] ADD  DEFAULT ((0)) FOR [priority]
GO

ALTER TABLE [dbo].[Promotion] ADD  DEFAULT ((1)) FOR [status]
GO


--Unit
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Unit]    Script Date: 8/8/2024 1:00:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Unit](
	[unit_id] [int] IDENTITY(1,1) NOT NULL,
	[unit_name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[unit_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--UserRole
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[UserRole]    Script Date: 8/8/2024 1:01:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRole](
	[role_id] [int] NOT NULL,
	[role_name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--User
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[User]    Script Date: 8/8/2024 1:41:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[display_name] [nvarchar](100) NULL,
	[username] [varchar](100) NOT NULL,
	[password] [nvarchar](max) NULL,
	[role_id] [int] NOT NULL,
	[phone_number] [varchar](15) NULL,
	[email] [varchar](100) NULL,
	[status] [tinyint] NOT NULL,
	[image] [nvarchar](max) NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[User] ADD  DEFAULT ((0)) FOR [status]
GO

ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [created_at]
GO

ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [updated_at]
GO

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserRole] FOREIGN KEY([role_id])
REFERENCES [dbo].[UserRole] ([role_id])
GO

ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserRole]
GO


--Product
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 8/8/2024 1:01:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[price] [decimal](10, 2) NOT NULL,
	[image] [nvarchar](max) NULL,
	[status] [tinyint] NOT NULL,
	[unit_id] [int] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product] ADD  DEFAULT ((1)) FOR [status]
GO

ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [created_at]
GO

ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [updated_at]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Unit] FOREIGN KEY([unit_id])
REFERENCES [dbo].[Unit] ([unit_id])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Unit]
GO


--Recipe
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Recipe]    Script Date: 8/8/2024 1:39:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Recipe](
	[recipe_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[recipe_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[role_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[recipe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Recipe] ADD  DEFAULT (getdate()) FOR [created_at]
GO

ALTER TABLE [dbo].[Recipe] ADD  DEFAULT (getdate()) FOR [updated_at]
GO

ALTER TABLE [dbo].[Recipe] ADD  DEFAULT ((1)) FOR [role_id]
GO

ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO

ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_Product]
GO

ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_Role] FOREIGN KEY([role_id])
REFERENCES [dbo].[UserRole] ([role_id])
GO

ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_Role]
GO


--Ingredient
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Ingredient]    Script Date: 8/8/2024 1:01:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ingredient](
	[ingredient_id] [int] IDENTITY(1,1) NOT NULL,
	[ingredient_name] [nvarchar](100) NOT NULL,
	[quantity] [decimal](10, 2) NOT NULL,
	[min_quantity] [decimal](10, 2) NOT NULL,
	[unit_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ingredient_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ingredient]  WITH CHECK ADD  CONSTRAINT [FK_Ingredient_Unit] FOREIGN KEY([unit_id])
REFERENCES [dbo].[Unit] ([unit_id])
GO

ALTER TABLE [dbo].[Ingredient] CHECK CONSTRAINT [FK_Ingredient_Unit]
GO


--Import
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Import]    Script Date: 8/8/2024 1:02:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Import](
	[import_id] [int] IDENTITY(1,1) NOT NULL,
	[import_date] [datetime] NOT NULL,
	[amount_taken] [decimal](10, 2) NOT NULL,
	[total_cost] [decimal](10, 2) NULL,
	[amount_remaining] [decimal](10, 2) NULL,
	[description] [nvarchar](max) NULL,
	[created_by] [int] NOT NULL,
	[approved_by] [int] NULL,
	[status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[import_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Import] ADD  DEFAULT (getdate()) FOR [import_date]
GO

ALTER TABLE [dbo].[Import]  WITH CHECK ADD  CONSTRAINT [FK_Import_Manager] FOREIGN KEY([approved_by])
REFERENCES [dbo].[User] ([user_id])
GO

ALTER TABLE [dbo].[Import] CHECK CONSTRAINT [FK_Import_Manager]
GO

ALTER TABLE [dbo].[Import]  WITH CHECK ADD  CONSTRAINT [FK_Import_Staff] FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([user_id])
GO

ALTER TABLE [dbo].[Import] CHECK CONSTRAINT [FK_Import_Staff]
GO


--Order
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Order]    Script Date: 8/8/2024 1:40:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Order](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NOT NULL,
	[created_by] [int] NOT NULL,
	[order_date] [datetime] NOT NULL,
	[delivery_date] [datetime] NULL,
	[status] [tinyint] NOT NULL,
 CONSTRAINT [PK_Order_1] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [order_date]
GO

ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [status]
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Customer] ([customer_id])
GO

ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([user_id])
GO

ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO


--OrderDetail
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[OrderDetail]    Script Date: 8/8/2024 1:02:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderDetail](
	[order_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
	[promotion_applied] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[order_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderDetail] ADD  DEFAULT ((0)) FOR [promotion_applied]
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Order] ([order_id])
GO

ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO

ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO


--Payment
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[Payment]    Script Date: 8/8/2024 1:02:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[payment_date] [datetime] NOT NULL,
	[payment_type] [nvarchar](50) NOT NULL,
	[amount_paid] [decimal](10, 2) NOT NULL,
	[total_amount] [decimal](10, 2) NOT NULL,
	[change_due] [decimal](10, 2) NOT NULL,
	[note] [nvarchar](max) NULL,
	[payment_status] [tinyint] NOT NULL,
	[coupon_applied] [tinyint] NOT NULL,
	[transaction_id] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Payment__465962284C60391E] UNIQUE NONCLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [payment_date]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0)) FOR [payment_status]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0)) FOR [coupon_applied]
GO

ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Order] ([order_id])
GO

ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Order]
GO


--RecipeDetail
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[RecipeDetail]    Script Date: 8/8/2024 1:02:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RecipeDetail](
	[recipe_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[recipe_id] [int] NOT NULL,
	[ingredient_id] [int] NOT NULL,
	[quantity] [decimal](10, 2) NOT NULL,
	[unit_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[recipe_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RecipeDetail]  WITH CHECK ADD  CONSTRAINT [FK_RecipeDetail_Ingredient] FOREIGN KEY([ingredient_id])
REFERENCES [dbo].[Ingredient] ([ingredient_id])
GO

ALTER TABLE [dbo].[RecipeDetail] CHECK CONSTRAINT [FK_RecipeDetail_Ingredient]
GO

ALTER TABLE [dbo].[RecipeDetail]  WITH CHECK ADD  CONSTRAINT [FK_RecipeDetail_Recipe] FOREIGN KEY([recipe_id])
REFERENCES [dbo].[Recipe] ([recipe_id])
GO

ALTER TABLE [dbo].[RecipeDetail] CHECK CONSTRAINT [FK_RecipeDetail_Recipe]
GO

ALTER TABLE [dbo].[RecipeDetail]  WITH CHECK ADD  CONSTRAINT [FK_RecipeDetail_Unit] FOREIGN KEY([unit_id])
REFERENCES [dbo].[Unit] ([unit_id])
GO

ALTER TABLE [dbo].[RecipeDetail] CHECK CONSTRAINT [FK_RecipeDetail_Unit]
GO


--ProductPromotion
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[ProductPromotion]    Script Date: 8/8/2024 1:02:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductPromotion](
	[product_promotion_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[promotion_id] [int] NOT NULL,
	[order_detail_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_promotion_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProductPromotion]  WITH CHECK ADD  CONSTRAINT [FK_ProductPromotion_OrderDetail] FOREIGN KEY([order_detail_id])
REFERENCES [dbo].[OrderDetail] ([order_detail_id])
GO

ALTER TABLE [dbo].[ProductPromotion] CHECK CONSTRAINT [FK_ProductPromotion_OrderDetail]
GO

ALTER TABLE [dbo].[ProductPromotion]  WITH CHECK ADD  CONSTRAINT [FK_ProductPromotion_Product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO

ALTER TABLE [dbo].[ProductPromotion] CHECK CONSTRAINT [FK_ProductPromotion_Product]
GO

ALTER TABLE [dbo].[ProductPromotion]  WITH CHECK ADD  CONSTRAINT [FK_ProductPromotion_Promotion] FOREIGN KEY([promotion_id])
REFERENCES [dbo].[Promotion] ([promotion_id])
GO

ALTER TABLE [dbo].[ProductPromotion] CHECK CONSTRAINT [FK_ProductPromotion_Promotion]
GO


--ImportDetail
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[ImportDetail]    Script Date: 8/8/2024 1:03:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ImportDetail](
	[import_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[import_id] [int] NOT NULL,
	[ingredient_id] [int] NOT NULL,
	[quantity] [decimal](10, 2) NOT NULL,
	[unit_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[import_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ImportDetail]  WITH CHECK ADD  CONSTRAINT [FK_ImportDetail_Import] FOREIGN KEY([import_id])
REFERENCES [dbo].[Import] ([import_id])
GO

ALTER TABLE [dbo].[ImportDetail] CHECK CONSTRAINT [FK_ImportDetail_Import]
GO

ALTER TABLE [dbo].[ImportDetail]  WITH CHECK ADD  CONSTRAINT [FK_ImportDetail_Ingredient] FOREIGN KEY([ingredient_id])
REFERENCES [dbo].[Ingredient] ([ingredient_id])
GO

ALTER TABLE [dbo].[ImportDetail] CHECK CONSTRAINT [FK_ImportDetail_Ingredient]
GO

ALTER TABLE [dbo].[ImportDetail]  WITH CHECK ADD  CONSTRAINT [FK_ImportDetail_Unit] FOREIGN KEY([unit_id])
REFERENCES [dbo].[Unit] ([unit_id])
GO

ALTER TABLE [dbo].[ImportDetail] CHECK CONSTRAINT [FK_ImportDetail_Unit]
GO


--CustomerCoupon
USE [DrinkStore]
GO

/****** Object:  Table [dbo].[CustomerCoupon]    Script Date: 8/8/2024 1:03:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerCoupon](
	[customer_coupon_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NOT NULL,
	[coupon_id] [int] NOT NULL,
	[order_id] [int] NULL,
	[status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[customer_coupon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CustomerCoupon] ADD  DEFAULT ((1)) FOR [status]
GO

ALTER TABLE [dbo].[CustomerCoupon]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCoupon_Coupon] FOREIGN KEY([coupon_id])
REFERENCES [dbo].[Coupon] ([coupon_id])
GO

ALTER TABLE [dbo].[CustomerCoupon] CHECK CONSTRAINT [FK_CustomerCoupon_Coupon]
GO

ALTER TABLE [dbo].[CustomerCoupon]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCoupon_Customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Customer] ([customer_id])
GO

ALTER TABLE [dbo].[CustomerCoupon] CHECK CONSTRAINT [FK_CustomerCoupon_Customer]
GO

ALTER TABLE [dbo].[CustomerCoupon]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCoupon_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Order] ([order_id])
GO

ALTER TABLE [dbo].[CustomerCoupon] CHECK CONSTRAINT [FK_CustomerCoupon_Order]
GO

