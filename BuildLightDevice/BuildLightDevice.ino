#include <WiFi.h>
#include <WiFiClient.h>
#include <WebServer.h>
#include <ESPmDNS.h>
#include <fauxmoESP.h>

// Step 1. Create SECTRET_* defines
#include "Secrets.h"
const char* ssid = SECRET_SSID;
const char* password = SECRET_PASSWORD;

// Step 2. Connect pins 27 & 26 to RED and GREEN on light strip
const int redPin = 27;
const int greenPin = 26;

uint8_t redIntensity = 0;
uint8_t greenIntensity = 255;

bool isOn = true;

const char* host = "buildlight";

#define ALEXA_ID "Build"

void updateLight ()
{
  if (isOn) {
      digitalWrite(redPin, redIntensity > 0 ? 0 : 1);
      digitalWrite(greenPin, greenIntensity > 0 ? 0 : 1);
  }
  else {
      digitalWrite(redPin, 1);
      digitalWrite(greenPin, 1);
  }
}

WebServer server(80);

void handleIndex() {
  server.send(200, "text/plain", "hello chat room!");
}

void handleColor() {
  uint8_t red = 0;
  uint8_t green = 0;
  uint8_t blue = 0;
  for (uint8_t i = 0; i < server.args(); i++) {
    auto name = server.argName(i);
    auto value = atoi(server.arg(i).c_str());
    if (name.length() > 0) {
      switch (name[0]) {
        case 'r':
          red = value;
          break;
        case 'g':
          green = value;
          break;
        case 'b':
          blue = value;
          break;
      }
    }
  }

  redIntensity = red;
  greenIntensity = green;
  updateLight();
  
  Serial.printf("SET COLOR red=%d, green=%d, blue=%d\n",
    (int)red, (int)green, (int)blue);
  server.send(200, "text/plain", "color set");
}

void handleNotFound() {
  String message = "File Not Found\n\n";
  message += "URI: ";
  message += server.uri();
  message += "\nMethod: ";
  message += (server.method() == HTTP_GET) ? "GET" : "POST";
  message += "\nArguments: ";
  message += server.args();
  message += "\n";
  for (uint8_t i = 0; i < server.args(); i++) {
    message += " " + server.argName(i) + ": " + server.arg(i) + "\n";
  }
  server.send(404, "text/plain", message);
}

void setup(void) {
  pinMode(redPin, OUTPUT);
  digitalWrite(redPin, 1);
  pinMode(greenPin, OUTPUT);
  digitalWrite(greenPin, 1);
  updateLight();

  Serial.begin(115200);

  //
  // Connect to WiFi
  //
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  Serial.println("");
  // Wait for connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to ");
  Serial.println(ssid);
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  if (MDNS.begin(host)) {
    Serial.println("MDNS responder started");
  }

  //
  // Setup Alexa Server
  //
  setupAlexa ();

  //
  // Setup Color API Server
  //
  server.on("/", handleIndex);

  server.on("/color", HTTP_POST, handleColor);

  server.onNotFound(handleNotFound);

  server.begin();
  Serial.println("HTTP server started");
}

void loop(void) {
  server.handleClient();
}

/* ======= ALEXA SUPPORT =============*/

fauxmoESP fauxmo;

#if CONFIG_FREERTOS_UNICORE
#define ARDUINO_RUNNING_CORE 0
#else
#define ARDUINO_RUNNING_CORE 1
#endif

void setupAlexa()
{
    fauxmo.createServer(true); // not needed, this is the default value
//    fauxmo.setPort(80); // This is required for gen3 devices
    fauxmo.enable(true);
    fauxmo.addDevice(ALEXA_ID);
    fauxmo.onSetState([](unsigned char device_id, const char * device_name, bool state, unsigned char value) {
        isOn = state;
        Serial.println("Changed light from Alexa");
        updateLight();
    });
    fauxmo.setState(ALEXA_ID, true, 255);
    xTaskCreatePinnedToCore(loopBackground, "loopBackground", 4096, NULL, 1, NULL, ARDUINO_RUNNING_CORE);
}

void loopBackground(void *)
{
  for (;;) {
    fauxmo.handle();
  }
}
