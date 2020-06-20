# Build Light

Display your IDE's build status using RGB LEDs and IoT devices!

Created on [Frank Krueger's Twitch Stream](https://twitch.tv/FrankKrueger).

## Installation

### Mac

1. Install the Visual Studio Extension called "Build Light" from the **Beta** Gallery.
2. Restart Visual Studio

### Windows

- [ ] Upload to [Visual Studio Marketplace](https://marketplace.visualstudio.com/)

1. Install the Visual Studio Extension called "Build Light" 
2. Restart Visual Studio

## Bill of Materials

1. ESP32 Dev Kit ([Amazon](https://www.amazon.com/HiLetgo-ESP-WROOM-32-Development-Microcontroller-Integrated/dp/B0718T232Z))

2. 5V RGB LED Strip ([Amazon](https://www.amazon.com/Backlight-eTopxizu-Multi-Colour-Controller-Background/dp/B01FJUMP6M))

3. Some wires/some solder.

## Construction Steps

1. Create a `Secrets.h` in the BuildLightDevice folder with the `SECRET_*` defines for your WiFi network.

2. Connect pins 27 & 26 to RED and GREEN on light strip.

3. Connect pin VIN to +5V on light strip.

## Extension Building Steps

### Mac

Load the extension in VS4Mac, then Run.

### Windows

Set the VS project as the startup project.

Run, open a new project in the experimental edition and build a project.

### Other

- [Setup](docs/Setup.md)
- [Testing](docs/Testing.md)
- [Errors](docs/Errors.md)
