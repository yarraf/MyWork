CREATE TABLE [dbo].[Programmations]
(
	[IdProgrammation] INT NOT NULL PRIMARY KEY IDENTITY, 
	[CleProgrammations] CHAR(10) NOT NULL ,
    [AnneeProgrammation] INT NOT NULL, 
    [PrixUnitaireTravaux] DECIMAL(18, 3) NULL, 
    [BudgetDisponible] DECIMAL(18, 3) NULL, 
    [DateCreation] DATETIME NULL, 
    [UserCreation] INT NULL, 
    [DateModification] DATETIME NULL, 
    [UserModification] INT NULL, 


    CONSTRAINT [AK_Programmations_Column] UNIQUE ([CleProgrammations]) 
    
)

GO

CREATE INDEX [IX_Programmations] ON [dbo].[Programmations] ([IdProgrammation])
