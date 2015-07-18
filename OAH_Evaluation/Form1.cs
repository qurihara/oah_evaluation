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
        TaskDisplay tDisplay;

        ArduinoUno arduino;
        public Form1()
        {
            InitializeComponent();

            // Create a new connection
            if (!Task.debug) { 
                arduino = new ArduinoUno("COM4");
                arduino.SetPinMode(ArduinoUnoPins.D13, PinModes.Output);
            }
            this.FormClosed += Form1_FormClosed;

            tDisplay = new TaskDisplay();
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Task.debug) {
                arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Input);
                arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Input);
                // dispose of the object
                arduino.Dispose();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxId.Text = DateTime.Now.Ticks.ToString();
        }

        private void Initialize()
        {
            if (!Task.debug)
            {
                arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
                arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
                Qurihara.Anm.WaitAnm anm = new Qurihara.Anm.WaitAnm(1000);
                anm.AnmFinishedHandler += anm_AnmFinishedHandler;
                anm.Start();
            }
        }

        void anm_AnmFinishedHandler(object sender, EventArgs e)
        {
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 180);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, 180);
            Qurihara.Anm.WaitAnm anm = new Qurihara.Anm.WaitAnm(1000);
            anm.AnmFinishedHandler += anm_AnmFinishedHandler2;
            anm.Start();
        }
        void anm_AnmFinishedHandler2(object sender, EventArgs e)
        {
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 0);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, 0);
            Qurihara.Anm.WaitAnm anm = new Qurihara.Anm.WaitAnm(1000);
            anm.AnmFinishedHandler += anm_AnmFinishedHandler3;
            anm.Start();
        }
        void anm_AnmFinishedHandler3(object sender, EventArgs e)
        {
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Input);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Input);
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

        int maxDegree = 180;
        private void button5_Click(object sender, EventArgs e)
        {
            string id = textBoxId.Text;
            int[] list = { 
                             (int)(maxDegree * 0), 
                             (int)(maxDegree * 0.25), 
                             (int)(maxDegree * 0.5), 
                             (int)(maxDegree * 0.75), 
                             (int)(maxDegree * 1) 
                         };
            Manager manager = new Manager(id, 10, list
//            Manager manager = new Manager(id, 1, list
                ,"現在のヘッドフォンによる聴覚の「開放と閉塞の感覚の度合い」をスライダーで選んでOKを押してください．"
                ,"開放的"
                ,"閉塞的"
                ,arduino,tDisplay);
            manager.Initialize();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string id = textBoxId.Text;

            axWindowsMediaPlayer1.URL = "1.mp3";
            int[] list = { 0, maxDegree };
            Manager manager = new Manager(id, 1, list
                , "現在のヘッドフォンによる音楽鑑賞の「爽快感の度合い」をスライダーで選んでOKを押してください．"
                , "高い"
                , "低い"
                , arduino, tDisplay);
            manager.Initialize();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string id = textBoxId.Text;

            axWindowsMediaPlayer1.URL = "1.mp3";
            int[] list = { 0, maxDegree };
            Manager manager = new Manager(id, 1, list
                , "現在のヘッドフォンによる音楽鑑賞の「音楽への没入感の度合い」をスライダーで選んでOKを押してください．"
                , "高い"
                , "低い"
                , arduino, tDisplay);
            manager.Initialize();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string id = textBoxId.Text;

            axWindowsMediaPlayer1.URL = "2.mp3";
            int[] list = { 0, maxDegree };
            Manager manager = new Manager(id, 1, list
                , "現在の音楽鑑賞の「爽快感の度合い」をスライダーで選んでOKを押してください．"
                , "高い"
                , "低い"
                , arduino, tDisplay);
            manager.Initialize();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string id = textBoxId.Text;

            axWindowsMediaPlayer1.URL = "2.mp3";
            int[] list = { 0, maxDegree };
            Manager manager = new Manager(id, 1, list
                , "現在の音楽鑑賞の「音楽への没入感の度合い」をスライダーで選んでOKを押してください．"
                , "高い"
                , "低い"
                , arduino, tDisplay);
            manager.Initialize();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Initialize();
        }

    }
}
