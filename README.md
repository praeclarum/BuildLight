# Build Light

Display your IDE's build status using RGB LEDs and IoT devices!

Created on [Frank Krueger's Twitch Stream](https://twitch.tv/FrankKrueger).

## Installation

Install the Visual Studio Extension called "Build Light" from the **Beta** Gallery.

## Bill of Materials

1. ESP32 Dev Kit ([Amazon](https://www.amazon.com/HiLetgo-ESP-WROOM-32-Development-Microcontroller-Integrated/dp/B0718T232Z))

2. 5V RGB LED Strip ([Amazon](https://www.amazon.com/Backlight-eTopxizu-Multi-Colour-Controller-Background/dp/B01FJUMP6M))

3. Some wires/some solder.

## Construction Steps

1. Create a `Secrets.h` in the BuildLightDevice folder with the `SECRET_*` defines for your WiFi network.

2. Connect pins 27 & 26 to RED and GREEN on light strip.

3. Connect pin VIN to +5V on light strip.

## Extension Building Steps

Load the extension in VS4Mac, then Run.

