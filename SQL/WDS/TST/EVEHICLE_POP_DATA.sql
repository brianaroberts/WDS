CREATE TABLE [tst].[Electric_Vehicle_Population_Data](
	[VIN_1_10] [nvarchar](50) NOT NULL,
	[County] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Postal_Code] [int] NULL,
	[Model_Year] [int] NULL,
	[Make] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
	[Electric_Vehicle_Type] [nvarchar](50) NULL,
	[Clean_Alternative_Fuel_Vehicle_CAFV_Eligibility] [nvarchar](100) NULL,
	[Electric_Range] [nvarchar](50) NULL,
	[Base_MSRP] [nvarchar](50) NULL,
	[Legislative_District] [nvarchar](50) NULL,
	[DOL_Vehicle_ID] [int] NULL,
	[Vehicle_Location] [nvarchar](50) NULL,
	[Electric_Utility] [nvarchar](150) NULL,
	[_2020_Census_Tract] [float] NULL
) ON [PRIMARY]
GO
