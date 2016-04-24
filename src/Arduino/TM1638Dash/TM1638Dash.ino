#include <TM1638.h>
#include <LiquidCrystal.h>

// TM1638
#define STROBE_PIN 7
#define DATA_PIN   8
#define CLOCK_PIN  9

/*
  The circuit:
 * LCD RS pin to digital pin 12
 * LCD Enable pin to digital pin 11
 * LCD D4 pin to digital pin 5
 * LCD D5 pin to digital pin 4
 * LCD D6 pin to digital pin 3
 * LCD D7 pin to digital pin 2
 * LCD R/W pin to ground
 * LCD VSS pin to ground
 * LCD VCC pin to 5V
 * 10K resistor:
 * ends to +5V and ground
 * wiper to LCD VO pin (pin 3)
 */
// 2004a
#define LCD_RS_PIN      12
#define LCD_ENABLE_PIN  11
#define LCD_D4_PIN      5
#define LCD_D5_PIN      4
#define LCD_D6_PIN      3
#define LCD_D7_PIN      2

// MISC.
#define BAUD 19200

// 2 for the leds, 2 for the dots, 8 for the digits, 80 for the display
#define TX_LENGTH 92

// TM1638 CODE

#ifdef REVERSE
   InvertedTM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#else
   TM1638 module(DATA_PIN, CLOCK_PIN, STROBE_PIN);
#endif

int lastLeds, lastDots;
char lastDisp[9];
bool blankLastTime;

// 2004a CODE

LiquidCrystal lcdScreen(LCD_RS_PIN, LCD_ENABLE_PIN, LCD_D4_PIN, LCD_D5_PIN, LCD_D6_PIN, LCD_D7_PIN);

void readInputValues(char* leds, char* dots, char* disp, char* lcd) {
  Serial.readBytes(leds, 2);
  Serial.readBytes(dots, 2);
  Serial.readBytes(disp, 8);
  Serial.readBytes(lcd, 80);

  // null-terminating strings is a nice thing to do.
  leds[2] = NULL;
  dots[2] = NULL;
  disp[8] = NULL;
  lcd[80] = NULL;
}

void displayInput() {
  char leds[3];
  char dots[3];
  char disp[9];
  char lcd[80];
  int ledBits, dotBits;

  readInputValues(leds, dots, disp, lcd);

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

  lcdScreen.setCursor(0, 0);
  lcdScreen.print(lcd);
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
  lcdScreen.begin(20, 4);
  module.setDisplayToString("hEy j");
  lcdScreen.print("how's it going");
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
