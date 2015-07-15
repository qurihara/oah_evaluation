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

namespace OAH_Evaluation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a new connection
            var arduino = new ArduinoUno("COM4");

            // Read an analog value 
            float valueInVolts = arduino.ReadAnalog(ArduinoUnoAnalogPins.A0);

            // Read the state of a pin
            arduino.GetCurrentPinState(ArduinoUnoPins.D8);

            // Write a digital value to an output pin
            arduino.SetPinMode(ArduinoUnoPins.D4, PinModes.Output);
            arduino.SetDO(ArduinoUnoPins.D4, true);

            // Write an analog value (PWM) to a PWM pin
            arduino.SetPinMode(ArduinoUnoPins.D3_PWM, PinModes.PWM);
            arduino.SetPWM(ArduinoUnoPWMPins.D3_PWM, 90);

            // Use a servo
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 90);

            // dispose of the object
            arduino.Dispose();
        }
    }
}
