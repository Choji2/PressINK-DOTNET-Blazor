insert into CATEGORIES values ('PROD', 'PRODUCTION','Production Printers',SYSDATETIME(),SYSDATETIME());
insert into CATEGORIES values ('TC', 'TEAM CORDINATOR','TC printer',SYSDATETIME(),SYSDATETIME());
insert into CATEGORIES values ('ADMIN', 'ADMIN','Admin Printer',SYSDATETIME(),SYSDATETIME());


INSERT INTO [dbo].[PLANT] ([PLANT_ID], [NAME], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'AAPL1', N'AAP Frame Line 1', N'Line 1 of AAP ', N'2023-03-23 05:41:38', SYSDATETIME())
INSERT INTO [dbo].[PLANT] ([PLANT_ID], [NAME], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'AAPL2', N'AAP Frame Line 2', N'Line 2 of AAP', N'2023-03-23 05:41:01', SYSDATETIME())
INSERT INTO [dbo].[PLANT] ([PLANT_ID], [NAME], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'AAPL5', N'AAP AEI', N'Engine Assembly of AAP', N'2023-03-23 05:42:46', SYSDATETIME())
INSERT INTO [dbo].[PLANT] ([PLANT_ID], [NAME], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'AAPL6', N'AAP Engine Plant', N'Engine Plant of AAP', N'2023-03-23 05:43:35', SYSDATETIME())


INSERT INTO [dbo].[MODEL] ([MODEL_ID], [MODEL_NAME], [MANUFACTOR_ID], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'M5255', N'M5255', N'LEX', N'Custom Paper Tray Printer. v', N'2023-03-09 23:04:27', SYSDATETIME())

INSERT INTO [MANUFACTURE] ([MANUFACTOR_ID], [NAME], [DESC], [CreatedDate], [UpdatedDate]) VALUES (N'LEX', N'Lexmark', N'Laser Printer', N'2023-03-09 23:02:26', SYSDATETIME())