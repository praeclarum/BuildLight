# Setup

Download arduino  
https://www.arduino.cc/en/Main/Software

Need to configure link(s) for additional boards

**Preferences**  
Additional Board Manager URLs: `http://arduino.esp8266.com/stable/package_esp8266com_index.json, https://dl.espressif.com/dl/package_esp32_index.json`

Tools | Board: "Arduino Uno" | Boards Manager...

Search for **esp32** | Install esp32 - 1.0.4

Search for **esp8266** | Install esp8266 - 2.7.1

**Tools**  
Port: `/dev/cu.SLAB_USBtoUART`

Need to install the driver for this to show,

cp2102 driver
- https://www.silabs.com/interface/usb-bridges/classic/device.cp2102
- https://www.silabs.com/products/development-tools/software/usb-to-uart-bridge-vcp-drivers
- https://www.silabs.com/documents/public/software/Mac_OSX_VCP_Driver.zip
- https://www.silabs.com/Support%20Documents/Software/Mac_OSX_VCP_Driver.zip

[nodemcu/nodemcu-devkit-v1.0#2](https://github.com/nodemcu/nodemcu-devkit-v1.0/issues/2)
- https://www.silabs.com/products/mcu/Pages/USBtoUARTBridgeVCPDrivers.aspx
