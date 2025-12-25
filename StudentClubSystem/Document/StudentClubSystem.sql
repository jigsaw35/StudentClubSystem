IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Clubs] (
    [Id] int NOT NULL IDENTITY,
    [Ad] nvarchar(100) NOT NULL,
    [Aciklama] nvarchar(max) NOT NULL,
    [KurulusTarihi] datetime2 NOT NULL,
    [AktifMi] bit NOT NULL,
    CONSTRAINT [PK_Clubs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [AdSoyad] nvarchar(100) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [Sifre] nvarchar(max) NOT NULL,
    [Rol] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [KulupId] int NOT NULL,
    [Baslik] nvarchar(200) NOT NULL,
    [Aciklama] nvarchar(max) NOT NULL,
    [BaslangicTarihi] datetime2 NOT NULL,
    [BitisTarihi] datetime2 NOT NULL,
    [Kontenjan] int NOT NULL,
    [Konum] nvarchar(max) NOT NULL,
    [Durum] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Events_Clubs_KulupId] FOREIGN KEY ([KulupId]) REFERENCES [Clubs] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [EventRegistrations] (
    [Id] int NOT NULL IDENTITY,
    [EtkinlikId] int NOT NULL,
    [KullaniciId] int NOT NULL,
    [KayitTarihi] datetime2 NOT NULL,
    [OnayDurumu] nvarchar(max) NOT NULL,
    [KatilimDurumu] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_EventRegistrations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EventRegistrations_Events_EtkinlikId] FOREIGN KEY ([EtkinlikId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EventRegistrations_Users_KullaniciId] FOREIGN KEY ([KullaniciId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_EventRegistrations_EtkinlikId] ON [EventRegistrations] ([EtkinlikId]);
GO

CREATE INDEX [IX_EventRegistrations_KullaniciId] ON [EventRegistrations] ([KullaniciId]);
GO

CREATE INDEX [IX_Events_KulupId] ON [Events] ([KulupId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251208142620_V1_ModelDuzeltme', N'8.0.0');
GO

COMMIT;
GO

