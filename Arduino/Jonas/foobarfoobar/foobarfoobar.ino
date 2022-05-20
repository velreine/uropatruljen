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


// Create aREST instance
aREST rest = aREST();

//char ssid[] = "prog";  // your network SSID (name)
//char pass[] = "Alvorlig5And";              // your network password
char ssid[] = "Linksys00115";  // your network SSID (name)
char pass[] = "jonas1234";              // your network password
int keyIndex = 0;              // your network key Index number (needed only for WEP)

int status = WL_IDLE_STATUS;

// ALL PIN STATES.
int P0 = 0;
int P1 = 0;
int P2 = 0;
int P3 = 0;
int P4 = 0;
int P5 = 0;
int P6 = 0;
int P7 = 0;
int P8 = 0;
int P9 = 0;
int P10 = 0;
int P11 = 0;
int P12 = 0;
int P13 = 0;
int P14 = 0;
int P15 = 0;
int P16 = 0;
int P17 = 0;
int P18 = 0;
int P19 = 0;
int P20 = 0;
int P21 = 0;
int P22 = 0;
int P23 = 0;
int P24 = 0;
int P25 = 0;
int P26 = 0;
int P27 = 0;
int P28 = 0;

WiFiServer restServer(80);

void setup(void) {
  // Function to be exposed
  //rest.function("led", ledControl);
  //rest.function("blue", blue);
  rest.function("control-rgb", controlRgbComponent);
  
  // Give name and ID to device
  rest.set_id("008");
  rest.set_name("lorteUro");
  
  // Rest variables.
  rest.variable("P0", &P0);
  rest.variable("P1", &P1);
  rest.variable("P2", &P2);
  rest.variable("P3", &P3);
  rest.variable("P4", &P4);
  rest.variable("P5", &P5);
  rest.variable("P6", &P6);
  rest.variable("P7", &P7);
  rest.variable("P8", &P8);
  rest.variable("P9", &P9);
  rest.variable("P10", &P10);
  rest.variable("P11", &P11);
  rest.variable("P12", &P12);
  rest.variable("P13", &P13);
  rest.variable("P14", &P14);
  rest.variable("P15", &P15);
  rest.variable("P16", &P16);
  rest.variable("P17", &P17);
  rest.variable("P18", &P18);
  rest.variable("P19", &P19);
  rest.variable("P20", &P20);
  rest.variable("P21", &P21);
  rest.variable("P22", &P22);
  rest.variable("P23", &P23);
  rest.variable("P24", &P24);
  rest.variable("P25", &P25);
  rest.variable("P26", &P26);
  rest.variable("P27", &P27);
  rest.variable("P28", &P28);

  // Pinmode
  pinMode(connectedPin, OUTPUT);
  pinMode(disconnectedPin, OUTPUT);
  pinMode(rgbRedPin,OUTPUT);
  pinMode(rgbGreenPin,OUTPUT);
  pinMode(rgbBluePin,OUTPUT);

  // Start Serial
  Serial.begin(9600);


  // Disconnect self in-case we were connected already, because otherwise we might be in an unknown state with the AP.
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



int controlRgbComponent(String command) {

  // Expected input is like: "10 11 12 1 255 180 170".
  // Expected input is like: "<PIN_R> <PIN_G> <PIN_B> <ON/OFF> <R_VALUE> <G_VALUE> <B_VALUE>"
  
  // Get a pointer to the command string.
  const char* commandPtr = command.c_str();

  // The endPtr starts at NULL as we have no end.
  char* endPtr = NULL;

  // Invoke the strtol function 7 times to keep finding the "next" number,
  // passing the new endPtr along.
  auto r_pin = strtol(commandPtr, &endPtr, 10);
  auto g_pin = strtol(endPtr, &endPtr, 10);
  auto b_pin = strtol(endPtr, &endPtr, 10);
  auto onOrOff = strtol(endPtr, &endPtr, 10);
  auto r = strtol(endPtr, &endPtr, 10);
  auto g = strtol(endPtr, &endPtr, 10);
  auto b = strtol(endPtr, &endPtr, 10);

  // Write the configuration to the pins.
  if(onOrOff == 1) {
    setPinVar(r_pin, r);
    setPinVar(g_pin, g);
    setPinVar(b_pin, b);
  digitalWrite(r_pin, r);
  digitalWrite(g_pin, g);
  digitalWrite(b_pin, b);//
  } else {
    setPinVar(r_pin, LOW);
    setPinVar(g_pin, LOW);
    setPinVar(b_pin, LOW);
  digitalWrite(r_pin, LOW);
  digitalWrite(g_pin, LOW);
  digitalWrite(b_pin, LOW);
  }

  Serial.println("Command received: ");
  Serial.println(command);
  Serial.println("R: " + (String)r);
  Serial.println("G: " + (String)g);
  Serial.println("B: " + (String)b);
  Serial.println("On/Off: " + (String)onOrOff);
  Serial.println("R_PIN: " + (String)r_pin);
  Serial.println("G_PIN: " + (String)g_pin);
  Serial.println("B_PIN: " + (String)b_pin);

  return 1;
}

void setPinVar(int pinNumber, int HIGH_OR_LOW) {
  switch(pinNumber) {
      case 0:
      P0 = HIGH_OR_LOW;
      break;
      case 1:
      P1 = HIGH_OR_LOW;
      break;
      case 2:
      P2 = HIGH_OR_LOW;
      break;
      case 3:
      P3 = HIGH_OR_LOW;
      break;
      case 4:
      P4 = HIGH_OR_LOW;
      break;
      case 5:
      P5 = HIGH_OR_LOW;
      break;
      case 6:
      P6 = HIGH_OR_LOW;
      break;
      case 7:
      P7 = HIGH_OR_LOW;
      break;
      case 8:
      P8 = HIGH_OR_LOW;
      break;
      case 9:
      P9 = HIGH_OR_LOW;
      break;
      case 10:
      P10 = HIGH_OR_LOW;
      break;
      case 11:
      P11 = HIGH_OR_LOW;
      break;
      case 12:
      P12 = HIGH_OR_LOW;
      break;
      case 13:
      P13 = HIGH_OR_LOW;
      break;
      case 14:
      P14 = HIGH_OR_LOW;
      break;
      case 15:
      P15 = HIGH_OR_LOW;
      break;
      case 16:
      P16 = HIGH_OR_LOW;
      break;
      case 17:
      P17 = HIGH_OR_LOW;
      break;
      case 18:
      P18 = HIGH_OR_LOW;
      break;
      case 19:
      P19 = HIGH_OR_LOW;
      break;
      case 20:
      P20 = HIGH_OR_LOW;
      break;
      case 21:
      P21 = HIGH_OR_LOW;
      break;
      case 22:
      P22 = HIGH_OR_LOW;
      break;
      case 23:
      P23 = HIGH_OR_LOW;
      break;
      case 24:
      P24 = HIGH_OR_LOW;
      break;
      case 25:
      P25 = HIGH_OR_LOW;
      break;
      case 26:
      P26 = HIGH_OR_LOW;
      break;
      case 27:
      P27 = HIGH_OR_LOW;
      break;
      case 28:
      P28 = HIGH_OR_LOW;
      break;


      return;
  }
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