#include <TM1638.h>

#define DATA_PIN 8
#define CLOCK_PIN 9
#define STROBE_PIN 7
#define BAUD 9600

// 2 for the leds, 2 for the dots, 8 for the digits
#define TX_LENGTH 12

#ifdef REVERSE
   InvertedTM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#else
   TM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#endif

int lastLeds, lastDots;
char lastDisp[9]

void setup() {
  Serial.begin(BAUD);
  module.setDisplayToString("hEy j");
  delay(2000);
}

void readInputValues(char* leds, char* dots, char* disp) {
  Serial.readBytes(leds, 2);
  Serial.readBytes(dots, 2);
  Serial.readBytes(disp, 8);

  leds[2] = NULL;
  dots[2] = NULL;
  disp[8] = NULL;
}

void displayInput() {
  char leds[3];
  char dots[3];
  char disp[9];
  int ledBits, dotBits;

  readInputValues(leds, dots, disp);

  ledBits = strtol(leds, NULL, 16);
  dotBits = strtol(dots, NULL, 16);

  Serial.print("LEDS: ");
  Serial.print(ledBits);
  Serial.print(" DOTS: ");
  Serial.print(dotBits);
  Serial.print(" DISPLAY: '");
  Serial.print(disp);
  Serial.println("'");

  if (ledBits != lastLeds) {
    module.setLEDs(ledBits);
  }
  if (dotBits != lastDots || strcmp(disp, lastDisp) != 0) {
    module.setDisplayToString(disp, dotBits);
  }

  lastLeds = ledBits;
  lastDots = dotBits;
  strcpy(lastDisp, disp);
}

void handleInput() {
  while (Serial.available() >= TX_LENGTH) {
    displayInput();
  }
}

void handleNoInput() {
//  module.clearDisplay();
}

void loop() {
  if (Serial.available() > 0) {
    handleInput();
  } else {
    handleNoInput();
  }
}
