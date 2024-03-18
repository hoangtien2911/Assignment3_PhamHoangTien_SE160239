USE master
GO
IF EXISTS(SELECT * FROM sys.databases WHERE name = 'ClockApplication')
	BEGIN
    	DROP DATABASE [ClockApplication]
  	END
GO 	
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ClockApplication')
	BEGIN
    	CREATE DATABASE [ClockApplication]
  	END
GO
USE [ClockApplication]   
GO 
CREATE TABLE ClockApplication.dbo.alarm (
	alarm_id int IDENTITY(1,1) NOT NULL,
	alarm_name varchar(200) NOT NULL,
	alarm_time datetime NOT NULL,
	enabled bit NOT NULL,
	CONSTRAINT system_config_pk PRIMARY KEY (alarm_id)
);
INSERT INTO ClockApplication.dbo.alarm (alarm_name, alarm_time, enabled)
VALUES 
    ('Morning Alarm', '2024-03-17 16:17:00', 1), -- Enabled
    ('Meeting Reminder', '2024-03-17 16:19:00', 1), -- Enabled
    ('Exercise Reminder', '2024-03-17 18:00:00', 0); -- Disabled