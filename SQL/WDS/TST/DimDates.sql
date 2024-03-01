
CREATE TABLE [tst].[DimDates] (
    [CalDate]          DATE          NULL,
    [DayName]          NVARCHAR (30) NULL,
    [DayOfMonth]       INT           NULL,
    [MonthName]        NVARCHAR (30) NULL,
    [YearNum]          INT           NULL,
    [MonthNum]         INT           NULL,
    [FirstOfMonth]     DATE          NULL,
    [LastOfMonth]      DATE          NULL,
    [CalQuarter]       INT           NULL,
    [GovQuarter]       INT           NULL,
    [GovYear]          INT           NULL,
    [Style101]         CHAR (10)     NULL,
    [Style106]         CHAR (10)     NULL,
    [Style112]         CHAR (8)      NULL,
    [DayOfYear]        INT           NULL,
    [DayOfWeek]        INT           NULL,
    [DayOfWeekInMonth] TINYINT       NULL,
    [WeekNum]          INT           NULL,
    [FirstOfYear]      DATE          NULL,
    [LastOfYear]       DATE          NULL,
    [IsWeekend]        INT           NOT NULL,
    [IsLeapYear]       BIT           NULL
);



