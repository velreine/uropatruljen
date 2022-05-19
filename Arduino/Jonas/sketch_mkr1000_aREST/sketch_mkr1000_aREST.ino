// Import required libraries
#include <SPI.h>
#include <WiFi101.h>

#define DEBUG_MODE 0

#define rgbRedPin 12
#define rgbGreenPin 11
#define rgbBluePin 10

#define connectedPin 7
#define disconnectedPin 8

#include <aREST.h>
#include <sstream>

// Create aREST instance
aREST rest = aREST();

char ssid[] = "prog";  // your network SSID (name)
char pass[] = "Alvorlig5And";              // your network password
int keyIndex = 0;              // your network key Index number (needed only for WEP)

int status = WL_IDLE_STATUS;

// int connectedPin = 6;
// int disconnectedPin = 8;

// int redPin = 12;
// int greenPin = 11;
// int bluePin = 10;

class Pin {
public:
    ushort PinNumber;
    char* Descriptor;

    std::basic_string<char> toJson() {

        std::stringstream ss;

        ss << "{"; // Begin JSON object.
        ss << "\"pinNumber\": " << this->PinNumber;
        ss << ",\"descriptor\": \"" << this->Descriptor << "\"";
        ss << "}"; // Begin JSON object.

        return ss.str();
    }

};

enum ComponentType {
    DIODE,
    RGB_DIODE,
    CAMERA,
    MICROPHONE,
};

class Component {
public:
    ComponentType Type;
    Pin* Pins;
    int PinCount;

    std::basic_string<char> toJson() {

        std::stringstream ss;

        ss << "{"; // Begin JSON object.
        ss << "\"type\": " << this->Type;
        ss << ",\"pinCount\": " << this->PinCount;
        ss << ",\"pins\": ["; // Start pins array.

        for(int i = 0; i < this->PinCount; i++) {
            Pin current = this->Pins[i];

            ss << current.toJson();

            // If not the last iteration, add trailing comma.
            if(i != this->PinCount-1) {
                ss << ",";
            }

        }

        ss << "]"; // End pins array.

        ss << "}"; // End JSON object.

        return ss.str();
    }

};

class HardwareConfiguration {
public:
    char* Name;
    char* SerialNumber;
    Component* Components;
    int ComponentCount;

    std::basic_string<char> toJson() {

        const char* _name = this->Name;


        std::stringstream ss;

        ss << "{"; // Begin JSON object.
        ss << "\"name\": \"" << this->Name << "\"";
        ss << ",\"serialNumber\": \"" << this->SerialNumber << "\"";


        ss << ",\"components\": ["; // Begin component array:

        // Iterate over all components and convert them to JSON as well.
        for(int i = 0; i < this->ComponentCount; i++) {
            Component current = this->Components[i];
            ss << current.toJson();

            // If not the last element add a trailing comma.
            if(i != this->ComponentCount-1) {
                ss << ",";
            }

        }


        ss << "]"; // End component array.

        ss << "}"; // End JSON object.

        return ss.str();
    }

};



WiFiServer restServer(80);

void setup(void) {
  // Function to be exposed
  rest.function("led", ledControl);
  rest.function("blue", blue);
  rest.function("control-rgb", controlRgbComponent);
  rest.function("get-board-layout", getBoardLayout);
  

  // Give name and ID to device
  rest.set_id("008");
  rest.set_name("lorteUro");
  
  // Pinmode
  pinMode(connectedPin, OUTPUT);
  pinMode(disconnectedPin, OUTPUT);
  pinMode(rgbRedPin,OUTPUT);
  pinMode(rgbGreenPin,OUTPUT);
  pinMode(rgbBluePin,OUTPUT);



  // Start Serial
  Serial.begin(9600);



  WiFi.disconnect();

  while (!Serial) {
    ;  // wait for serial port to connect. Needed for native USB port only
  }

  // check for the presence of the shield:
  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("WiFi shield not present");
    // don't continue:
    while (true)
      ;
  }

  // attempt to connect to Wifi network:
  while (status != WL_CONNECTED) {
    digitalWrite(disconnectedPin, HIGH);
    digitalWrite(connectedPin, LOW);

    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    delay(10000);
  }

  Serial.println();

  // you're connected now, so print out the status:
  printWifiStatus();
  digitalWrite(disconnectedPin, LOW);
  digitalWrite(connectedPin, HIGH);

  // Start server
  restServer.begin();
  Serial.println(F("Listening for connections..."));

  // Enable watchdog
  //wdt_enable(WDTO_4S);
}

void loop() {

  // Handle REST calls
  WiFiClient client = restServer.available();
  rest.handle(client);
}

int getBoardLayout(String command) {

    // !!!START HARDWARE_BOARD_LAYOUT
    // RGB_DIODE_1
    Pin p10;
    p10.Descriptor = "r";
    p10.PinNumber =  10;
    Pin p11;
    p11.Descriptor = "g";
    p11.PinNumber =  11;
    Pin p12;
    p12.Descriptor = "b";
    p12.PinNumber =  12;

    Component rgbDiode1;
    Pin pins[] = {p10,p11,p12};
    rgbDiode1.Pins = &pins[0];
    rgbDiode1.PinCount = 3;
    rgbDiode1.Type = RGB_DIODE;


    // DIODE_1
    Pin p7;
    p7.Descriptor = "input";
    p7.PinNumber = 7;

    Component diode1;
    diode1.Pins = &p7;
    diode1.PinCount = 1;
    diode1.Type = DIODE;

    // DIODE_2
    Pin p8;
    p8.Descriptor = "input";
    p8.PinNumber = 8;

    Component diode2;
    diode2.Pins = &p8;
    diode2.PinCount = 1;
    diode2.Type = DIODE;

    // Overall Configuration.
    HardwareConfiguration config;
    config.Name = "SMART_URO_V1";
    config.SerialNumber = "ABC-123-123";
    Component components[] = {rgbDiode1, diode1, diode2};
    config.Components = components;
    config.ComponentCount = 3;
  // !!!END HARDWARE_BOARD_LAYOUT


  // Print the hardware configuration.
  Serial.println(config.toJson().c_str());


  return 1;
}

int controlRgbComponent(String command) {

  // Expected input is like: "1 100 50 200".
  // Respectively meaning: ComponentId=1, R=100, G=50, B=200.

  // Get a pointer to the command string.
  const char* commandPtr = command.c_str();

  // The endPtr starts at NULL as we have no end.
  char* endPtr = NULL;

  // Invoke the strtol function 3 times to keep finding the "next" number,
  // passing the new endPtr along.
  auto r = strtol(commandPtr, &endPtr, 10);
  auto g = strtol(endPtr, &endPtr, 10);
  auto b = strtol(endPtr, &endPtr, 10);

  Serial.println("Command received: ");
  Serial.println(command);
  Serial.println("R: " + (String)r);
  Serial.println("G: " + (String)g);
  Serial.println("B: " + (String)b);

  return 1;
}


// Custom function accessible by the API
int ledControl(String command) {
  
  // http://192.168.4.1/led/woqdjiqwjdoqjwdiqwjdqiowjd

  Serial.println(command);

  // Get state from command
  int state = command.toInt();

  digitalWrite(connectedPin, state);
  if (state = 1){
    digitalWrite(disconnectedPin, 0);
  } else if (state = 0){
    digitalWrite(disconnectedPin, 1);
  }

  return 1;
}

int blue(String command) {
  Serial.println("blue called!");

  int state = command.toInt();
  if(state == 1){
    RGB_color(0,0,255);
  }
  if(state == 0) {
   RGB_color(0,0,0); 
  }
}
// Custom function accessible by the API. Not added yet
int rgbControl(String command) {
  Serial.println(command);
  // Get state from command
  int state = command.toInt();
  digitalWrite(connectedPin, state);

  return 1;
}

void RGB_color(int red_light_value, int green_light_value, int blue_light_value)
 {
  analogWrite(rgbRedPin, red_light_value);
  analogWrite(rgbGreenPin, green_light_value);
  analogWrite(rgbBluePin, blue_light_value);
}

void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your WiFi shield's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  IPAddress subnet = WiFi.subnetMask();
  Serial.print("Netmask: ");
  Serial.println(subnet);

  IPAddress gateway = WiFi.gatewayIP();
  Serial.print("Gateway: ");
  Serial.println(gateway);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}