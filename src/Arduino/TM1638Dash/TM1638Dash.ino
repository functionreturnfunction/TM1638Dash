#include <TM1638.h>

#define STROBE_PIN 7
#define DATA_PIN   8
#define CLOCK_PIN  9
#define BAUD 19200

// 2 for the leds, 2 for the dots, 8 for the digits
#define TX_LENGTH 12

#ifdef REVERSE
   InvertedTM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#else
   TM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#endif

int lastLeds, lastDots;
char lastDisp[9];
bool blankLastTime;

void readInputValues(char* leds, char* dots, char* disp) {
  Serial.readBytes(leds, 2);
  Serial.readBytes(dots, 2);
  Serial.readBytes(disp, 8);

  // null-terminating strings is a nice thing to do.
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

  module.setLEDs(ledBits == lastLeds && lastLeds == 0xFF && !blankLastTime ? 0 : ledBits);
  blankLastTime = !blankLastTime;
  module.setDisplayToString(disp, blankLastTime ? dotBits : 0);

  module.setDisplayToString(disp, dotBits);

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

void setup() {
  Serial.begin(BAUD);
  module.setDisplayToString("hEy j");
}

void loop() {
  if (!Serial) {
    while (!Serial) {}
  }
  
  if (Serial.available() > 0) {
    handleInput();
  } else {
    handleNoInput();
  }
}
