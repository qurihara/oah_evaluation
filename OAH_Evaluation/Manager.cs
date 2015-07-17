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

        protected Queue<Task> taskQueue;
        protected Task curTask;
        protected static int nShuffleFactor = 5; //リスト要素数の5倍シャッフルする。

        protected string user_id = "";

        public Manager(string user_id, int iteration, int[] degreeList, ArduinoUno arduinouno)
        {
            logPath = "log" + DateTime.Now.Ticks.ToString() + ".csv";
            this.user_id = user_id;
            arduino = arduinouno;

            taskQueue = new Queue<Task>();
            curTask = null;

            List<Task> taskList = new List<Task>();
            for (int i = 0; i < iteration; i++)
            {
                for (int j = 0; j < degreeList.Length; j++)
                {
                    taskList.Add(new Task(degreeList[j]));
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

        public void Initialize()
        {
            //TODO モーターを動かして慣らす．
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
        public void EndTask(int scale)
        {
            curTask.Scale = scale;
            Dump();
        }

        public void Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("user_id, trial_id, " + Task.DumpLegend());
            int trial_id = 1;
            foreach (Task t in taskQueue)
            {
                sb.AppendLine(user_id + ", " + trial_id.ToString() + ", " + t.Dump());
                trial_id++;
            }
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
        protected int degree;
        protected int scale;
        public int Scale
        {
            set { scale = value; }
        }

        public Task(int deg)
        {
            degree = deg;
            scale = -1;
        }

        public void GetReady()
        {
            ArduinoUno arduino = Manager.arduino;
            arduino.SetPinMode(ArduinoUnoPins.D9_PWM, PinModes.Servo);
            arduino.SetPinMode(ArduinoUnoPins.D10_PWM, PinModes.Servo);
            arduino.SetServo(ArduinoUnoPins.D9_PWM, degree);
            arduino.SetServo(ArduinoUnoPins.D10_PWM, degree);
        }

        public static string DumpLegend()
        {
            return "degree, scale";
        }
        public string Dump()//StreamWriter sw)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(degree.ToString());
            sb.Append(",");
            sb.Append(scale.ToString());
            //sb.Append("\n");
            //sw.Write(sb.ToString());
            return sb.ToString();
        }

    }
}