﻿-- DROP TABLE IN INVERSE ORDER OF THEIR CREATION.
DROP TABLE IF EXISTS component_state;
DROP TABLE IF EXISTS device;
DROP TABLE IF EXISTS pin;
DROP TABLE IF EXISTS component;
DROP TABLE IF EXISTS hardware_layout;
DROP TABLE IF EXISTS person_home;
DROP TABLE IF EXISTS room;
DROP TABLE IF EXISTS home;
DROP TABLE IF EXISTS person;

-- Person.
CREATE TABLE person
(
    id   INT IDENTITY(1000,1) PRIMARY KEY,
    name NVARCHAR(64),
    /*ManyToMany Homes*/
);

-- Home.
CREATE TABLE home
(
    id   INT IDENTITY(1000,1) PRIMARY KEY,
    name NVARCHAR(64),
    /*OneToMany Rooms*/
);

-- Room.
CREATE TABLE room
(
    id      INT IDENTITY(1000,1) PRIMARY KEY,
    name    NVARCHAR(32),
    home_id INT,
    FOREIGN KEY (home_id) REFERENCES home (id)
    /*OneToMany Devices*/
);

-- Many persons, belong to many homes.
-- TODO: Considerations (home roles?, home administrator eg...)
CREATE TABLE person_home
(
    person_id INT,
    home_id   INT,
    FOREIGN KEY (person_id) REFERENCES person (id),
    FOREIGN KEY (home_id) REFERENCES home (id),
);

-- Hardware Layout
CREATE TABLE hardware_layout
(
    id           INT IDENTITY(1000,1) PRIMARY KEY,
    product_name NVARCHAR(128),
    model_number NVARCHAR(64),
    /*OneToManyComponents*/
);

-- Component
CREATE TABLE component
(
    id                 INT IDENTITY(1000,1) PRIMARY KEY,
    name               NVARCHAR(32),
    hardware_layout_id INT,
    FOREIGN KEY (hardware_layout_id) REFERENCES hardware_layout (id)
    /*OneToManyPins*/
);

-- Pin
CREATE TABLE pin
(
    id            INT IDENTITY(1000,1) PRIMARY KEY,
    descriptor    NVARCHAR(32),
    hw_pin_number INT,
    pin_direction BIT,
    component_id  INT,
    FOREIGN KEY (component_id) REFERENCES component (id),
    -- This constraint only means that a component cannot redefine the same hardware pin number.
    -- TODO: how to ensure that a hardware_layout cannot have components with overlapping hw_pin_numbers?
    CONSTRAINT unique_pin_no_per_component UNIQUE (hw_pin_number, component_id)
);

-- Device.
CREATE TABLE device
(
    id                 INT IDENTITY(1000,1) PRIMARY KEY,
    name               NVARCHAR(32),
    serial_number      NVARCHAR(64),
    hardware_layout_id INT,
    room_id            INT,
    FOREIGN KEY (hardware_layout_id) REFERENCES hardware_layout (id),
    FOREIGN KEY (room_id) REFERENCES room (id),
);

-- Component state.
CREATE TABLE component_state
(
    id           INT IDENTITY(1000,1) PRIMARY KEY,
    device_id    INT,
    component_id INT,
    is_on        BIT,     -- All components support this state.
    r_value      TINYINT, -- RGB components support this state.
    g_value      TINYINT, -- RGB components support this state.
    b_value      TINYINT, -- RGB components support this state.
    discriminant NVARCHAR(32),
    FOREIGN KEY (device_id) REFERENCES device (id),
    FOREIGN KEY (component_id) REFERENCES component (id),
    CONSTRAINT unique_component_state_per_device UNIQUE (device_id, component_id)
);


-- Example data.

-- Create three persons.
-- This SQL matches what EntityFramework creates not the SQL above.
DELETE FROM  ComponentStates;
DELETE FROM  Devices;
DELETE FROM  Pins;
DELETE FROM  Components;
DELETE FROM  HardwareLayouts;
DELETE FROM  HomePerson;
DELETE FROM  Rooms;
DELETE FROM  Homes;
DELETE FROM  Persons;

SET
IDENTITY_INSERT Persons ON
INSERT INTO Persons(Id,Name,Email,HashedPassword)
VALUES (1,'Jonas','jonas@example.com','MTIzNDU='), (2,'Anton','anton@example.com','MTIzNDU='), (3,'Nicky','nicky@example.com','MTIzNDU=')
;
SET
IDENTITY_INSERT Persons OFF

-- Create three homes.
SET IDENTITY_INSERT Homes ON
INSERT INTO Homes(Id,Name)
VALUES (1,'Mit Hjem'), (2,'Mitt Hem'), (3,'My Home')
SET IDENTITY_INSERT Homes OFF
;

-- Put people in their respective homes.
INSERT INTO HomePerson(ConnectedHomesId, ResidentsId)
VALUES (1, 1), -- Jonas => 'Mit Hjem'
       (2, 2), -- Anton => 'Mitt Hem'
       (3, 3) -- Nicky => 'My Home'
;

-- Create some rooms.
SET
IDENTITY_INSERT Rooms ON
INSERT INTO Rooms(Id,Name, HomeId) VALUES
-- Jonas hus rum.
(1, 'Stue', 1), (2, 'Soveværelse', 1),
-- Anton rum.
(3,'Stugan', 2), (4,'Sovrum', 2),
-- Nicky rum.
(5,'Living Room', 3), (6,'Office', 3)
SET IDENTITY_INSERT Rooms OFF
;


/** Begin create our proto-type hardware layout. **/
SET
IDENTITY_INSERT HardwareLayouts ON
INSERT INTO HardwareLayouts(Id, ProductName, ModelNumber) 
VALUES (1, 'Smart Uro V1', 'ABC123')
SET IDENTITY_INSERT HardwareLayouts OFF
;

SET
IDENTITY_INSERT Components ON
INSERT INTO Components(Id, Name, HardwareLayoutId, Type)
VALUES (1, 'RGB_DIODE_1', 1,1)
SET IDENTITY_INSERT Components OFF
;

SET
IDENTITY_INSERT Pins ON
INSERT INTO Pins(Id, Descriptor, HwPinNumber, Direction, ComponentId) VALUES 
(1, 'r_pin',10,1,1),
(2, 'g_pin',11,1,1),
(3, 'b_pin',12,1,1)
SET IDENTITY_INSERT Pins OFF
;
/** End create our proto-type hardware layout. **/


-- These are physical products shipped to the customer "registered" in the cloud.
SET
IDENTITY_INSERT Devices ON
INSERT INTO Devices(Id, Name, SerialNumber, LayoutId, RoomId) VALUES
(1, 'Jeanette 2022', 'jonas', 1, 2),
(2, 'Anette 2023', 'anton', 1, 4),
(3, 'Nanna 2022', 'nicky', 1, 6)
SET IDENTITY_INSERT Devices OFF
;

-- Component state is for saved configuration of components.
SET
IDENTITY_INSERT ComponentStates ON
INSERT INTO ComponentStates(Id, DeviceId, ComponentId, IsOn, RValue, GValue, BValue, Discriminator) VALUES 
(1, 1, 1, 1, 255,0,0,'RgbComponentState'),
(2, 2, 1, 1, 0,255,0,'RgbComponentState'),
(3, 3, 1, 1, 0,0,255,'RgbComponentState')
SET IDENTITY_INSERT ComponentStates OFF
;







