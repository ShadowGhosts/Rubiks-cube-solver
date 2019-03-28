
/*
 * Simple demo, should work with any driver board
 *
 * Connect STEP, DIR as indicated
 *
 * Copyright (C)2015-2017 Laurentiu Badea
 *
 * This file may be redistributed under the terms of the MIT license.
 * A copy of this license has been included with this distribution in the file LICENSE.
 */
 
#include <Arduino.h>
#include "BasicStepperDriver.h"
#include <LiquidCrystal.h>
#include <msTask.h>
#include <TimerOne.h>

//Sets pins for LCD
const int rs = 25, en = 27, d4 = 29, d5 = 31, d6 = 33, d7 = 35;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

char message; //Every char sent from PC gets put in 'message'
char face; //If message is anyhting but 'z' its saved in 'face'

int m; //minutes
int s; //seconds
int ms; //one tenth of a second

// Motor steps per revolution. Most steppers are 200 steps or 1.8 degrees/step
#define MOTOR_STEPS 200//All motors used have 200 steps
#define RPM 30//Sets speed of the steppers

#define HT 180 //Half turn
#define QT 90 //Quarter turn
#define NQT -90 //Quarter turn in opposite direction

// Since microstepping is set externally, make sure this matches the selected mode
// If it doesn't, the motor will move at a different RPM than chosen
// 1=full step, 2=half step etc.
#define MICROSTEPS 16 //All 3 jumpers are being used which gives us 16 microsteps

#define qsd 500//Delay used inbetween the quarter stepper motor turns so LCD can keep up (qsd = quarter stepper delay)
#define hsd 1000//Delay used inbetween the half stepper motor turns so LCD can keep up (hsd = half stepper delay)

//Pins Used to control steppers
#define DIR_X A1
#define STEP_X A0
#define ENABLE_X 38

#define DIR_Y A7
#define STEP_Y A6
#define ENABLE_Y A2

#define DIR_Z 48
#define STEP_Z 46
#define ENABLE_Z A8

#define DIR_E0 28
#define STEP_E0 26
#define ENABLE_E0 24

#define DIR_E1 34
#define STEP_E1 36
#define ENABLE_E1 30

#define DIR_E2 16
#define STEP_E2 17
#define ENABLE_E2 23

void checkmessage();
void timer();

// 2-wire basic config, microstepping is hardwired on the driver
BasicStepperDriver stepper_X(MOTOR_STEPS, DIR_X, STEP_X);
BasicStepperDriver stepper_Y(MOTOR_STEPS, DIR_Y, STEP_Y);
BasicStepperDriver stepper_Z(MOTOR_STEPS, DIR_Z, STEP_Z);
BasicStepperDriver stepper_E0(MOTOR_STEPS, DIR_E0, STEP_E0);
BasicStepperDriver stepper_E1(MOTOR_STEPS, DIR_E1, STEP_E1);
BasicStepperDriver stepper_E2(MOTOR_STEPS, DIR_E2, STEP_E2);

//Uncomment line to use enable/disable functionality
//BasicStepperDriver stepper(MOTOR_STEPS, DIR, STEP, ENABLE);

msTask timertask(100, timer); //Interrupt is triggered every 100 milliseconds

void setup() {

    //Enabling of all steppers
    stepper_X.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_X, OUTPUT);
    digitalWrite(ENABLE_X, HIGH);
    stepper_X.disable(); //Unlocks the steppers
    
    stepper_Y.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_Y, OUTPUT);
    digitalWrite(ENABLE_Y, HIGH);
    stepper_Y.disable();

    stepper_Z.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_Z, OUTPUT);
    digitalWrite(ENABLE_Z, HIGH);
    stepper_Z.disable();

    stepper_E0.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E0, OUTPUT);
    digitalWrite(ENABLE_E0, HIGH);
    stepper_E0.disable();

    stepper_E1.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E1, OUTPUT);
    digitalWrite(ENABLE_E1, HIGH);
    stepper_E1.disable();
    
    stepper_E2.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E2, OUTPUT);
    digitalWrite(ENABLE_E2, HIGH);
    stepper_E2.disable();

    //Enables 12V power on 'D10' on the ramps board to power E2
    pinMode(10, OUTPUT);
    digitalWrite(10, HIGH);

    //Enables 12V power on 'D9' on the ramps board to power LEDs
    pinMode(9, OUTPUT);
    digitalWrite(9, HIGH);

    Serial.begin(9600); //Same serial port as the USB RX = D2 TX = D1

    msTask::init();
    timertask.stop(); //Timer stops after it is initilatized so it can be started as soon as PC starts sending stuff
    
    lcd.begin(16, 2);

    /* Used for displaying turn sequence on the LCD
    lcd.setCursor(16, 1);
    lcd.autoscroll();
    */
}

void loop() {

     //Waits for PC to send something over the serial port
     if (Serial.available())
     {
      message = Serial.read();
      checkmessage();
     }

    // pause and allow the motor to be moved by hand
    // stepper.disable();
}

void checkmessage()
{
  if (message == 'Z')
  {
      //Locks the steppers
      stepper_X.enable();
      stepper_Y.enable();
      stepper_Z.enable();
      stepper_E0.enable();
      stepper_E1.enable();
      stepper_E2.enable();

      digitalWrite(ENABLE_X, LOW);
      digitalWrite(ENABLE_Y, LOW);
      digitalWrite(ENABLE_Z, LOW);
      digitalWrite(ENABLE_E0, LOW);
      digitalWrite(ENABLE_E1, LOW);
      digitalWrite(ENABLE_E2, LOW);
    
      switch(face){

      case 'A': //U
        stepper_X.rotate(QT);
        delay(qsd); //Delay must be set so all the serial messaging doesn't interfere with interrupt
        break;
        
      case 'B': //F
        stepper_Y.rotate(QT);
        delay(qsd);
        break;

      case 'C': //L
        stepper_Z.rotate(QT);
        delay(qsd);
        break;

      case 'D': //R
        stepper_E0.rotate(QT);
        delay(qsd);
        break;

      case 'E': //D
        stepper_E1.rotate(QT);
        delay(qsd);
        break;

      case 'F': //B
        stepper_E2.rotate(QT);
        delay(qsd);
        break;

      case 'G': //U'
        stepper_X.rotate(NQT);
        delay(qsd);
        break;
        
      case 'H': //F'
        stepper_Y.rotate(NQT);
        delay(qsd);
        break;

      case 'I': //L'
        stepper_Z.rotate(NQT);
        delay(qsd);
        break;

      case 'J': //R'
        stepper_E0.rotate(NQT);
        delay(qsd);
        break;

      case 'K': //D'
        stepper_E1.rotate(NQT);
        delay(qsd);
        break;

      case 'L': //B'
        stepper_E2.rotate(NQT);
        delay(qsd);
        break;

      case 'M': //U2
        stepper_X.rotate(HT);
        delay(hsd);
        break;
        
      case 'N': //F2
        stepper_Y.rotate(HT);
        delay(hsd);
        break;

      case 'O': //L2
        stepper_Z.rotate(HT);
        delay(hsd);
        break;

      case 'P': //R2
        stepper_E0.rotate(HT);
        delay(hsd);
        break;

      case 'Q': //D2
        stepper_E1.rotate(HT);
        delay(hsd);
        break;

      case 'R': //B2
        stepper_E2.rotate(HT);
        delay(hsd);
        break;
        
 
      }
    
      Serial.write(message); //Send back a 'Z' to the PC so it can send the next char
      
  }
  
  //The last char the PC will send will be a 'Y' to indicate end of the turn sequence, this will stop the timer
  else if(message == 'Y')
  {
     timertask.stop();
     ms=0;
     s=0;
     m=0;

     //Disables the steppers after a 'Y' is sent
     digitalWrite(ENABLE_X, HIGH);
     digitalWrite(ENABLE_Y, HIGH);
     digitalWrite(ENABLE_Z, HIGH);
     digitalWrite(ENABLE_E0, HIGH);
     digitalWrite(ENABLE_E1, HIGH);
     digitalWrite(ENABLE_E2, HIGH);
  }
      
  else
  {    
    timertask.start(); //Starts the timer
    face = message;
    
    //delay(5);
    
    //lcd.print(face); Used for displaying turn sequence on the LCD
    
    Serial.write(face); //Send back turn sequence char back to PC so it knows it got the correct char
  }
}

void timer()
{
  //lcd.clear(); //Clears the LCD everytime interrupt is called
  
  lcd.setCursor(0,0);
  lcd.print("Solve Timer");
  
  ms++;

  if(ms==10)
  {
    ms=0;
    s++;
  }

  if(s==60)
  {
    s=0;
    m++;
  }

  //Everytime interupt is called 'ms' gets incredmented and LCD is updated
  lcd.setCursor(0,1);
  lcd.print(m);
  lcd.print(":");
  lcd.print(s);
  lcd.print(".");
  lcd.print(ms);
  
}
