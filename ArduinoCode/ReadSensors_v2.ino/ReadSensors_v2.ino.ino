#include <Arduino_MKRENV.h>
#include <MKRWAN.h>

#define DEVEUI  "A8610A3233368603"
#define APPEUI "0000000000000001"
#define APPKEY "25DEEF447FED62FBEF41F3758428ADDA"

LoRaModem modem;

void setup() {
  //setting up serial communication for debbuging
  Serial.begin(9600);
  while (!Serial && millis() < 5000);// only wait 5 sec

  if (!ENV.begin()) {
    Serial.println("Failed to initialize MKR ENV Shield!");
    while (1);
  }

  //Starting LoRa module
  if(!modem.begin(EU868)){
    Serial.println("Failed to initialise LoRa module!");
    while(1);
  }

  //Setting up our own device to the network:
  int connected = modem.joinOTAA(APPEUI,APPKEY,DEVEUI);
  if (!connected){
    Serial.println("Something went wrong with connecting to the node. Are you near a window? Move closer and try again :)");
    while(1);
  }
   

  //setting poll interval to 60 seconds. Max is one message every 2 minutes
  modem.minPollInterval(60);
}

void loop() {
  // read all the sensor values
  float temperature = ENV.readTemperature();
  float humidity    = ENV.readHumidity();
  float pressure    = ENV.readPressure();
  float illuminance = ENV.readIlluminance();

  uint8_t payload[8];
  int temp = (int)(temperature * 100);  // Multiply to avoid floats
  int hum = (int)(humidity * 100);
  int press = (int)(pressure * 100);
  int illum = (int)(illuminance * 100);

  //OxFF is 11111111
  //the first half of the binary value (& 0xFF ensures it is retained)
  payload[0] = (temp >> 8) & 0xFF;  // MSB of temperature
  //second part of the binary value (& bitwise comparison of the whole value with 00000000 11111111 gets only the second part)
  payload[1] = temp & 0xFF;         // LSB of temperature
  payload[2] = (hum >> 8) & 0xFF;   // MSB of humidity
  payload[3] = hum & 0xFF;          // LSB of humidity
  payload[4] = (press >> 8) & 0xFF; // MSB of pressure
  payload[5] = press & 0xFF;        // LSB of pressure
  payload[6] = (illum >> 8) & 0xFF; // MSB of illuminance
  payload[7] = illum & 0xFF;

  modem.beginPacket();
  modem.write(payload, sizeof(payload)); // Send binary data
  modem.endPacket(true); 

// print each of the sensor values
  Serial.print("Temperature = ");
  Serial.print(temperature);
  Serial.println(" °C");

  Serial.print("Humidity    = ");
  Serial.print(humidity);
  Serial.println(" %");

  Serial.print("Pressure    = ");
  Serial.print(pressure);
  Serial.println(" kPa");

  Serial.print("Illuminance = ");
  Serial.print(illuminance);
  Serial.println(" lx");

  // print an empty line
  Serial.println();

  // wait 1 second to print again
  delay(300000);
}


 



