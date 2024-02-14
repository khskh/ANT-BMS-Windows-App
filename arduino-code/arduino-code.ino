#include <SoftwareSerial.h>

SoftwareSerial mySerial(9, 10); // RX, TX

int data[160]; // Array to store received data
int dataSize = 0; // Counter for received data

void setup() {
  Serial.begin(19200);   // Initialize serial communication
  mySerial.begin(19200);
  delay(500);
}

void loop() {
  byte message[] = {0x5A, 0x5A, 0x00, 0x00, 0x00, 0x00 }; // HEX code (request for ANT BMS screen data)
  mySerial.write(message, sizeof(message));

  unsigned long startTime = millis();

  // Wait for data for approximately 1 second
  while (millis() - startTime < 400) {
    if (mySerial.available()) {
      if (dataSize < 140) {
        // Read data and store it in the array
        byte receivedByte = mySerial.read();
        data[dataSize] = receivedByte;
        dataSize++;
      }
    }
  }

  //Display the contents of the array in the serial port
  for (int i = 0; i < dataSize; i++) {
    if (data[i] < 0x10) {
        Serial.print("0"); 
    }
    Serial.print(data[i], HEX);
  }
Serial.println();





  // Serial.print("SOC ");
  // Serial.print(data[74]);
  // Serial.println(' ');



  // Serial.print("Temp ");
  // Serial.print((data[91] << 8) | data[92]);
  // Serial.println(' ');


  // Serial.print("Number of cells ");
  // Serial.print(data[123]);
  // Serial.println(' ');

  // Reset the data counter and wait for 1 second before sending the request again
  dataSize = 0;
  delay(200);
}