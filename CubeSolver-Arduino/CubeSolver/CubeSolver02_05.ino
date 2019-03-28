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
const int rs = 25, en = 27, d4 = 29, d5 = 31, d6 = 33, d7 = 35;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

char message;
char face;

// Motor steps per revolution. Most steppers are 200 steps or 1.8 degrees/step
#define MOTOR_STEPS 200
#define RPM 50

#define HT 180
#define QT 90
#define NQT -90

// Since microstepping is set externally, make sure this matches the selected mode
// If it doesn't, the motor will move at a different RPM than chosen
// 1=full step, 2=half step etc.
#define MICROSTEPS 16

// All the wires needed for full functionality
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

// 2-wire basic config, microstepping is hardwired on the driver
BasicStepperDriver stepper_X(MOTOR_STEPS, DIR_X, STEP_X);
BasicStepperDriver stepper_Y(MOTOR_STEPS, DIR_Y, STEP_Y);
BasicStepperDriver stepper_Z(MOTOR_STEPS, DIR_Z, STEP_Z);
BasicStepperDriver stepper_E0(MOTOR_STEPS, DIR_E0, STEP_E0);
BasicStepperDriver stepper_E1(MOTOR_STEPS, DIR_E1, STEP_E1);
BasicStepperDriver stepper_E2(MOTOR_STEPS, DIR_E2, STEP_E2);

//Uncomment line to use enable/disable functionality
//BasicStepperDriver stepper(MOTOR_STEPS, DIR, STEP, ENABLE);

void setup() {
    stepper_X.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_X, OUTPUT);
    digitalWrite(ENABLE_X, LOW);
    stepper_X.enable();
    
    stepper_Y.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_Y, OUTPUT);
    digitalWrite(ENABLE_Y, LOW);
    stepper_Y.enable();

    stepper_Z.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_Z, OUTPUT);
    digitalWrite(ENABLE_Z, LOW);
    stepper_Z.enable();

    stepper_E0.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E0, OUTPUT);
    digitalWrite(ENABLE_E0, LOW);
    stepper_E0.enable();

    stepper_E1.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E1, OUTPUT);
    digitalWrite(ENABLE_E1, LOW);
    stepper_E1.enable();
    
    stepper_E2.begin(RPM, MICROSTEPS);
    pinMode(ENABLE_E2, OUTPUT);
    digitalWrite(ENABLE_E2, LOW);
    stepper_E2.enable();

    pinMode(10, OUTPUT);
    digitalWrite(10, HIGH);

    Serial.begin(9600);

    lcd.begin(16, 2);
    lcd.setCursor(16, 1);
    lcd.autoscroll();

}

void loop() {

     if (Serial.available())
     {
      message = Serial.read();
      checkmessage();
     }


    // energize coils - the motor will hold position
    // stepper_E0.enable();
     
    /*
     * Moving motor one full revolution using the degree notation
     */
    //stepper_E0.rotate(QT);

    /*
     * Moving motor to original position using steps
     */
    //stepper.move(-MOTOR_STEPS*MICROSTEPS);

    // pause and allow the motor to be moved by hand
    // stepper.disable();

    //delay(1000);
}

void checkmessage()
{
  if (message == 'Z')
  {
      switch(face){
      
      case 'A':
        stepper_X.rotate(QT);
        break;
        
      case 'B':
        stepper_Y.rotate(QT);
        break;

      case 'C':
        stepper_Z.rotate(QT);
        break;

      case 'D':
        stepper_E0.rotate(QT);
        break;

      case 'E':
        stepper_E1.rotate(QT);
        break;

      case 'F':
        stepper_E2.rotate(QT);
        break;

      case 'G':
        stepper_X.rotate(NQT);
        break;
        
      case 'H':
        stepper_Y.rotate(NQT);
        break;

      case 'I':
        stepper_Z.rotate(NQT);
        break;

      case 'J':
        stepper_E0.rotate(NQT);
        break;

      case 'K':
        stepper_E1.rotate(NQT);
        break;

      case 'L':
        stepper_E2.rotate(NQT);
        break;

      case 'M':
        stepper_X.rotate(HT);
        break;
        
      case 'N':
        stepper_Y.rotate(HT);
        break;

      case 'O':
        stepper_Z.rotate(HT);
        break;

      case 'P':
        stepper_E0.rotate(HT);
        break;

      case 'Q':
        stepper_E1.rotate(HT);
        break;

      case 'R':
        stepper_E2.rotate(HT);
       // lcd.print('R');
        break;
      }
      Serial.write(message);
  }
  else
  {
    
    
    
    face = message;
    delay(5);
    lcd.print(face);
    Serial.write(face);
    
  }
}
