# Errors

I got a fun error due to where Python was installed:

> Arduino: 1.8.12 (Mac OS X), Board: "NodeMCU 1.0 (ESP-12E Module), 80 MHz, Flash, Legacy (new can return nullptr), All SSL ciphers (most compatible), 4MB (FS:2MB OTA:~1019KB), 2, v2 Lower Memory, Disabled, None, Only Sketch, 115200"

> env: python3: No such file or directory
> exit status 127
> Error compiling for board NodeMCU 1.0 (ESP-12E Module).

> This report would have more information with "Show verbose output during compilation" option enabled in File -> Preferences.

Went searching and found:

- [esp8266/Arduino#6931](https://github.com/esp8266/Arduino/issues/6931)

> My workaround that seems to work:
> cd ~/Library/Arduino15/packages/esp8266/tools/python3/3.7.2-post1
> sudo unlink python3
> sudo ln -s /usr/bin/python3 /usr/local/bin/python3
> sudo ln -s /usr/local/bin/python3 python3
