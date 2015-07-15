using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharpduino;
using Sharpduino.Constants;

            // Read an analog value 
            //float valueInVolts = arduino.ReadAnalog(ArduinoUnoAnalogPins.A0);

            // Read the state of a pin
            //arduino.GetCurrentPinState(ArduinoUnoPins.D8);

            // Write a digital value to an output pin
            //arduino.SetPinMode(ArduinoUnoPins.D13, PinModes.Output);
            //arduino.SetDO(ArduinoUnoPins.D13, true);

            //// Write an analog value (PWM) to a PWM pin
            //arduino.SetPinMode(ArduinoUnoPins.D3_PWM, PinModes.PWM);
            //arduino.SetPWM(ArduinoUnoPWMPins.D3_PWM, 90);

            //// Use a servo
            //arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            //arduino.SetServo(ArduinoUnoPins.D9_PWM, 90);


namespace OAH_Evaluation
{
    public partial class Form1 : Form
    {
        ArduinoUno arduino;
        public Form1()
        {
            InitializeComponent();

            // Create a new connection
            //var arduino = new ArduinoUno("COM3");
            arduino = new ArduinoUno("COM3");
            arduino.SetPinMode(ArduinoUnoPins.D13, PinModes.Output);

            this.FormClosed += Form1_FormClosed;
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Input);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Input);
            // dispose of the object
            arduino.Dispose();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            arduino.SetDO(ArduinoUnoPins.D13, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            arduino.SetDO(ArduinoUnoPins.D13, false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 180);
            arduino.SetServo(ArduinoUnoPins.D10_PWM,180);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 0);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, 0);

        }
    }
}
