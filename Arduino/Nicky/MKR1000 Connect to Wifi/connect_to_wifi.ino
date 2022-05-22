#include <SPI.h>
#include <WiFi101.h>

#define SECRET_SSID "Velreine 2.4 GHz"
#define SECRET_PASS "1oktober"

#define SECONDS_10 10000

char ssid[] = SECRET_SSID;        // your network SSID (name)
char pass[] = SECRET_PASS;    // your network password (use for WPA, or use as key for WEP)
int status = WL_IDLE_STATUS;     // the Wifi radio's status

void setup() {
  
  // Initialize serial and wait for port to open:
  Serial.begin(9600);
  while(!Serial);

  // Attempt to connect to WiFi network:
  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to network: ");
    Serial.println(ssid);

    // Connect to WPA/WPA2 network:
    status = WiFi.begin(ssid, pass);

    // Wait 10 seconds for connection:
    delay(SECONDS_10);
  }

  // You're connected now, so print out the data:
  Serial.println("You're connected to the network");
  Serial.println("---------------------------------");
  printData();
  Serial.println("---------------------------------");


}

void loop() {
  // put your main code here, to run repeatedly:
  delay(SECONDS_10);
  printData();
  Serial.println("---------------------------------");

}

void printData() {
  Serial.println("Board Information:");
  // print your board's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  Serial.println();
  Serial.println("Network Information:");
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.println(rssi);

}
