-- DROP TABLE IN INVERSE ORDER OF THEIR CREATION.
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
    id   INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(64),
    /*ManyToMany Homes*/
);

-- Home.
CREATE TABLE home
(
    id   INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(64),
    /*OneToMany Rooms*/
);

-- Room.
CREATE TABLE room
(
    id      INT IDENTITY(1,1) PRIMARY KEY,
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
    id           INT IDENTITY(1,1) PRIMARY KEY,
    product_name NVARCHAR(128),
    model_number NVARCHAR(64),
    /*OneToManyComponents*/
);

-- Component
CREATE TABLE component
(
    id                 INT IDENTITY(1,1) PRIMARY KEY,
    name               NVARCHAR(32),
    hardware_layout_id INT,
    FOREIGN KEY (hardware_layout_id) REFERENCES hardware_layout (id)
    /*OneToManyPins*/
);

-- Pin
CREATE TABLE pin
(
    id            INT IDENTITY(1,1) PRIMARY KEY,
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
    id                 INT IDENTITY(1,1) PRIMARY KEY,
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
    id           INT IDENTITY(1,1) PRIMARY KEY,
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
INSERT INTO person(id, name)
VALUES (1, 'Jonas'),
       (2, 'Anton'),
       (3, 'Nicky')
;

-- Create three homes.
INSERT INTO home(id, name)
VALUES (1, 'Mit Hjem'),
       (2, 'Mitt Hem'),
       (3, 'My Home')
;

-- Put people in their respective homes.
INSERT INTO person_home(person_id, home_id)
VALUES (1, 1), -- Jonas => 'Mit Hjem'
       (2, 2), -- Anton => 'Mitt Hem'
       (3, 3) -- Nicky => 'My Home'
;

-- Create some rooms.
INSERT INTO room(id, name, home_id)
VALUES
-- Jonas hus rum.
(1, 'Stue', 1),
(2, 'Soveværelse', 1),
-- Anton rum.
(3, 'Stugan', 2),
(4, 'Sovrum', 2),
-- Nicky rum.
(5, 'Living Room', 3),
(6, 'Office', 3)
;


/** Begin create our proto-type hardware layout. **/
INSERT INTO hardware_layout(id, product_name, model_number)
VALUES (1, 'Smart Uro V1', 'ABC123')
;

INSERT INTO component(id, name, hardware_layout_id)
VALUES (1, 'RGB_DIODE_1', 1)
;

INSERT INTO pin(id, descriptor, hw_pin_number, pin_direction, component_id)
VALUES (1, 'r_pin', 10, 1, 1),
       (1, 'g_pin', 11, 1, 1),
       (1, 'b_pin', 12, 1, 1)
;
/** End create our proto-type hardware layout. **/


-- These are physical products shipped to the customer "registered" in the cloud.
INSERT INTO device(id, name, serial_number, hardware_layout_id, room_id)
VALUES (1, 'Lise 2022', 'jonas', 1, 2),
       (2, 'Lise 2022', 'anton', 1, 4),
       (3, 'Lise 2022', 'nicky', 1, 6)
;

-- Component state is for saved configuration of components.
INSERT INTO component_state(id, device_id, component_id, is_on, r_value, g_value, b_value, discriminant)
VALUES (1, 1, 1, 1, 255, 0, 0, 'rgb_state'),
       (1, 2, 1, 1, 0, 255, 0, 'rgb_state'),
       (1, 3, 1, 1, 0, 0, 255, 'rgb_state')

