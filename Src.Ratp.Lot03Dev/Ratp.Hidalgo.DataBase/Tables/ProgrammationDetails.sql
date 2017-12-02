CREATE TABLE [dbo].[ProgrammationDetails]
(
	[IdProgrammationDetails] INT NOT NULL PRIMARY KEY IDENTITY, 
	[CleProgrammationDetails] CHAR(10) NOT NULL, 
    [IdProgrammation] INT NOT NULL, 
    [Annee] INT NULL, 
    [Date] DATETIME NULL, 
    [IdLigne] INT NULL, 
    [IdLieu] INT NULL, 
    [IdNatureTravauxExternes] INT NULL, 
    [IdCategorie] INT NOT NULL,
	[DateCreation] DATETIME NULL, 
    [UserCreation] INT NULL, 
    [DateModification] DATETIME NULL, 
    [UserModification] INT NULL, 


    CONSTRAINT [AK_ProgrammationDetails_Column] UNIQUE ([CleProgrammationDetails]),
	CONSTRAINT [FK_ProgrammationDetails_Categorie] FOREIGN KEY ([IdCategorie]) REFERENCES  [Lvf_Categorie]([IdCategorie]),
    CONSTRAINT [FK_ProgrammationDetails_Programmation] FOREIGN KEY ([IdProgrammation]) REFERENCES  [Programmations](IdProgrammation), 
    CONSTRAINT [FK_ProgrammationDetails_Lignes] FOREIGN KEY ([IdLigne]) REFERENCES [Lignes]([IdLigne]), 
    CONSTRAINT [FK_ProgrammationDetails_Lieux] FOREIGN KEY ([IdLieu]) REFERENCES [Lieux]([IdLieu]), 
    CONSTRAINT [FK_ProgrammationDetails_NatureTravauxEx] FOREIGN KEY ([IdNatureTravauxExternes]) REFERENCES [Lvf_NatureTravauxExternes]([IdNatureTravauxExternes]),
)

GO

CREATE INDEX [IX_ProgrammationDetails_Column] ON [dbo].[ProgrammationDetails] ([IdProgrammationDetails])

GO

CREATE INDEX [IX_ProgrammationDetails_Categorie] ON [dbo].[Lvf_Categorie] ([IdCategorie])

GO

CREATE INDEX [IX_ProgrammationDetails_Programmations] ON [dbo].[Programmations] ([IdProgrammation])

GO

CREATE INDEX [IX_ProgrammationDetails_Lignes] ON [dbo].[Lignes] ([IdLigne])

GO

CREATE INDEX [IX_ProgrammationDetails_Lieux] ON [dbo].[Lieux] ([IdLieu])

GO

CREATE INDEX [IX_ProgrammationDetails_NatureTravauxEx] ON [dbo].[Lvf_NatureTravauxExternes] ([IdNatureTravauxExternes])
