CREATE TABLE [dbo].[Reservation]
(
	[ReservationID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BoatID] INT NOT NULL, 
    [MemberID] INT NOT NULL, 
    [Start] DATETIME NOT NULL, 
    [End] DATETIME NOT NULL, 
    
)
