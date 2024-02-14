# ANT BMS Windows App

A simple application for communication between the Ant-BMS system and a computer, using Arduino. This is an old application and is not finished; it only reads data frames and displays 4 basic parameters. May contains some bugs.

# Usage:

## Arduino

In the arduino-code folder, you will find the code for the Arduino platform. Just upload it.

The connection is established through pins 9 and 10; respectively RX and TX. You need to cross-connect them with the TX and RX pins from the BMS.

**Important Note:** You have to connect all of 4 pins between BMS and Arduino; including VCC pin. That's because if the BMS does not receive a +5V signal on the VCC pin, it will not start up and will not transmit any data.

## Visual Studio

Just run the program, select the port, and press 'Connect'.

## Comatabile devices

**ANT-BLE20AAUB**

It should work with other devices from ANT, but I haven't tested it. Most of these devices use the same data protocol.

## App Screen

![test1](https://github.com/khskh/ANT-BMS-Windows-App/assets/26013076/1ebd092f-db48-4f65-b025-10f9d08d359c)
