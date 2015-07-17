using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Sharpduino;
using Sharpduino.Constants;

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
            logPath = "log" + DateTime.Now.Ticks.ToString() + ".csv";
            this.user_id = user_id;
            arduino = arduinouno;
            Manager.tDisplay = tDisplay;
            tDisplay.buttonOK.Click += buttonOK_Click;

            taskQueue = new Queue<Task>();
            curTask = null;

            List<Task> taskList = new List<Task>();
            int id = 1;
            for (int i = 0; i < iteration; i++)
            {
                for (int j = 0; j < degreeList.Length; j++)
                {
                    taskList.Add(new Task(id,degreeList[j],taskDesc,labelLeftMost,labelRightMost));
                    id++;
                }
            }
            Shuffle(taskList);

            foreach (Task t in taskList)
            {
                taskQueue.Enqueue(t);
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
            //TODO
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
        static bool debug = true;

        int id;
        protected int degree;
        protected int scale;
        public int Scale
        {
            set { scale = value; }
        }

        protected string taskDesc, labelLeftMost, labelRightMost;
        public Task(int id, int deg,string taskDesc, string labelLeftMost, string labelRightMost)
        {
            this.id = id;
            degree = deg;
            scale = -1;

            this.taskDesc = taskDesc;
            this.labelLeftMost = labelLeftMost;
            this.labelRightMost = labelRightMost;
        }

        public void GetReady()
        {
            if (!debug)
            {
                ArduinoUno arduino = Manager.arduino;
                arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
                arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
                arduino.SetServo(ArduinoUnoPins.D9_PWM, degree);
                arduino.SetServo(ArduinoUnoPins.D10_PWM, degree);
            }
            Manager.tDisplay.labelTaskDesc.Text = "[" + id.ToString() + "] " +  taskDesc;
            Manager.tDisplay.labelLeftMost.Text = labelLeftMost;
            Manager.tDisplay.labelRightMost.Text = labelRightMost;
            Manager.tDisplay.Visible = true;

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