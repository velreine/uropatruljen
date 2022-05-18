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

char ssid[] = "prog";  // your network SSID (name)
char pass[] = "Alvorlig5And";              // your network password
int keyIndex = 0;              // your network key Index number (needed only for WEP)

int status = WL_IDLE_STATUS;

// int connectedPin = 6;
// int disconnectedPin = 8;

// int redPin = 12;
// int greenPin = 11;
// int bluePin = 10;

WiFiServer restServer(80);

void setup(void) {
  // Function to be exposed
  rest.function("led", ledControl);
  rest.function("blue", blue);

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
  Serial.begin(115200);

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


// Custom function accessible by the API
int ledControl(String command) {

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