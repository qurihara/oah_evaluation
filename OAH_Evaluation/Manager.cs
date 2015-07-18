using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Sharpduino;
using Sharpduino.Constants;

using Qurihara.Anm;

namespace OAH_Evaluation
{
    public class Manager
    {
        public static string logPath = "log.csv";
        public static ArduinoUno arduino;
        public static TaskDisplay tDisplay;

        protected Queue<Task> taskQueue;
        protected Task curTask;
        protected static int nShuffleFactor = 5; //リスト要素数の5倍シャッフルする。

        protected string user_id = "";

        public Manager(string user_id, int iteration, int[] degreeList,string taskDesc, string labelLeftMost, string labelRightMost, ArduinoUno arduinouno,TaskDisplay tDisplay)
        {
            logPath = "log_" + user_id + ".csv";
            this.user_id = user_id;
            arduino = arduinouno;
            Manager.tDisplay = tDisplay;
            tDisplay.buttonOK.Click += buttonOK_Click;

            taskQueue = new Queue<Task>();
            curTask = null;

            List<Task> taskList = new List<Task>();
            for (int i = 0; i < iteration; i++)
            {
                for (int j = 0; j < degreeList.Length; j++)
                {
                    taskList.Add(new Task(degreeList[j],taskDesc,labelLeftMost,labelRightMost));
                }
            }
            Shuffle(taskList);

            int id = 1;
            foreach (Task t in taskList)
            {
                t.Id = id;
                taskQueue.Enqueue(t);
                id++;
            }
            taskList.Clear();
            taskList = null;

        }

        void buttonOK_Click(object sender, EventArgs e)
        {
            if (curTask != null)
            {
                EndTask();
            }
        }

        public void Initialize()
        {
            //TODO モーターを動かして慣らす．
            DumpLegend();
            StartNextTask();
        }
        protected void Finish()
        {
            Manager.tDisplay.labelTaskDesc.Text = "実験終了！ありがとうございました！";
            Manager.tDisplay.labelLeftMost.Text = "";
            Manager.tDisplay.labelRightMost.Text = "";
            Manager.tDisplay.buttonOK.Enabled = false;
            Manager.tDisplay.trackBarScale.Value = 500;
            Manager.tDisplay.Visible = true;
            WaitAnm anm = new WaitAnm(3000);
            anm.AnmFinishedHandler += close_app;
            anm.Start();
        }

        void close_app(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public bool StartNextTask()
        {
            if (taskQueue.Count == 0)
            {
                return false;
            }
            curTask = taskQueue.Dequeue();
            curTask.GetReady();
            return true;
        }
        protected void EndTask()
        {
            curTask.Scale = tDisplay.trackBarScale.Value;
            Dump();
            bool hasNext = StartNextTask();
            if (!hasNext) Finish();
        }

        protected void DumpLegend()
        {
            File.AppendAllText(logPath, "user_id, " + Task.DumpLegend()+"\n", Encoding.GetEncoding("shift-jis"));
        }
        public void Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(user_id + ", " + curTask.Dump());
            File.AppendAllText(logPath, sb.ToString(), Encoding.GetEncoding("shift-jis"));      
        }
        
        protected static void Shuffle(List<Task> list)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < nShuffleFactor * list.Count; i++)
            {
                int x = r.Next(list.Count);
                int y = r.Next(list.Count);
                Swap(list, x, y);
            }
        }
        protected static void Swap(List<Task> list, int x, int y)
        {
            Task a = list[x];
            Task b = list[y];
            list.RemoveAt(x);
            list.Insert(x, b);
            list.RemoveAt(y);
            list.Insert(y, a);
        }

    }

    public class Task
    {
        public static bool debug = false;//true;

        protected int id;
        public int Id
        {
            set { id = value; }
        }
        protected int degree;
        protected int scale;
        public int Scale
        {
            set { scale = value; }
        }

        protected string taskDesc, labelLeftMost, labelRightMost;
        public Task(int deg,string taskDesc, string labelLeftMost, string labelRightMost)
        {
            degree = deg;
            scale = -1;

            this.taskDesc = taskDesc;
            this.labelLeftMost = labelLeftMost;
            this.labelRightMost = labelRightMost;
        }

        public void GetReady()
        {
            Manager.tDisplay.labelTaskDesc.Text = "準備中";
            Manager.tDisplay.labelLeftMost.Text = "";
            Manager.tDisplay.labelRightMost.Text = "";
            Manager.tDisplay.buttonOK.Enabled = false;
            Manager.tDisplay.trackBarScale.Value = 500;
            Manager.tDisplay.Visible = true;

            WaitAnm anm = new WaitAnm(1000);
            if (!debug)
            {
                Servo_reset();
//                anm.AnmFinishedHandler += Servo_set;
            }
            else
            {
                anm.AnmFinishedHandler += DisplayTask;
            }
            anm.Start();
        }

        void DisplayTask(object sender, EventArgs e)
        {
            //debug
            //Manager.tDisplay.labelTaskDesc.Text = "[" + degree.ToString() + "] " + taskDesc;
            Manager.tDisplay.labelTaskDesc.Text = "[" + id.ToString() + "] " + taskDesc;
            Manager.tDisplay.labelLeftMost.Text = labelLeftMost;
            Manager.tDisplay.labelRightMost.Text = labelRightMost;
            Manager.tDisplay.buttonOK.Enabled = true;
        }
        void Servo_reset()
        {
            ArduinoUno arduino = Manager.arduino;
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
            //arduino.SetServo(ArduinoUnoPins.D9_PWM, 90);
            //arduino.SetServo(ArduinoUnoPins.D10_PWM, 90);
            WaitAnm anm = new WaitAnm(1000);
            anm.AnmFinishedHandler += Servo_reset2;
            anm.Start();
        }
        void Servo_reset2(object sender, EventArgs e)
        {
            ArduinoUno arduino = Manager.arduino;
            arduino.SetServo(ArduinoUnoPins.D9_PWM, 90);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, 90);
            WaitAnm anm = new WaitAnm(1000);
            anm.AnmFinishedHandler += Servo_set;
            anm.Start();
        }
        void Servo_set(object sender, EventArgs e)
        {
            ArduinoUno arduino = Manager.arduino;
            //arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            //arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
            arduino.SetServo(ArduinoUnoPins.D9_PWM, degree);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, degree);
            WaitAnm anm = new WaitAnm(1000);
            anm.AnmFinishedHandler += Servo_off;
            anm.AnmFinishedHandler += DisplayTask;
            anm.Start();
        }
        void Servo_off(object sender, EventArgs e)
        {
            ArduinoUno arduino = Manager.arduino;
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Input);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Input);
        }

        public static string DumpLegend()
        {
            return "trial_id, degree, scale";
        }
        public string Dump()//StreamWriter sw)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(id.ToString());
            sb.Append(",");
            sb.Append(degree.ToString());
            sb.Append(",");
            sb.Append(scale.ToString());
            //sb.Append("\n");
            //sw.Write(sb.ToString());
            return sb.ToString();
        }

    }
}