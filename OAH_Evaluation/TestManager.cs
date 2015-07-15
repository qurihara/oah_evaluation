using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OAH_Evaluation
{
    public class TestBlock
    {
        //�����F
        //�C���^�t�F�[�X�@�ʏ�piemenu, hiddenpiemenu   2
        //�����ʒu �����菇�����A������t�����A����    3
        //�n���x�@�n���A���S��  2
        //�R�}���h 4����   ������
        //1�����J��Ԃ��@10

        // [canceled]���݂̃^�X�N�̏����ɂ��ẮA�ʓ|�Ȃ̂�EnviromentTable�i�������͂��̃��[�J���Łj�ōs���B

        public static string[] interFaceIDString = { "��Pie", "�BPie" };
        public static string[] tachiichiIDString = { "���菇�ʒu", "����t�ʒu", "���ʈʒu" };
        public static string[] jukurenIDString = { "�����_���z�u", "�Œ�z�u" };


        protected bool isDoingTask= false;
        public bool IsDoingTask
        {
            get { return isDoingTask; }
        }
        protected bool isReadyTask = false;
        public bool IsReadyTask
        {
            get { return isReadyTask; }
        }

        protected DateTime totalStartTime;
        protected bool isFirst = true;
        protected TimeSpan totalTimeElapsed;
        public TimeSpan TotalTimeElapsed
        {
            get { return totalTimeElapsed; }
        }
        protected DateTime startTime;
        protected string logPath;
        protected string pieLogPath;

        protected Queue<Task> taskQueue;
        protected Task curTask;

        protected string nameID;

        protected int nShuffleFactor = 5; //���X�g�v�f����5�{�V���b�t������B

        public TestBlock(string nameID, string logpath,string pielogpath)
        {
            this.nameID = nameID;
            logPath = logpath;
            pieLogPath = pielogpath;
            taskQueue = new Queue<Task>();
            isFirst = true;
        }
        public TestBlock(string nameID, string path,string pielogpath,int intID,int tachiID, int jukuID, int nD, int nRep) :this(nameID,path,pielogpath)
        {
            AddQuestion(intID, tachiID, jukuID, nD, nRep);
        }
        public TestBlock(string nameID, string path,string pielogpath, int[] intID, int[] tachiID, int[] jukuID, int nD, int nRep) : this(nameID, path,pielogpath)
        {
            int qCount = 0;
            for (int k = 0; k < jukuID.Length; k++)
            {
                for (int i = 0; i < intID.Length; i++)
                {
                    for (int j = 0; j < tachiID.Length; j++)
                    {
                        qCount += AddQuestion(intID[i], tachiID[j], jukuID[k], nD, nRep);
                        if (i == intID.Length - 1 && j == tachiID.Length - 1 && k == jukuID.Length - 1)
                        {
                            // do nothing
                        }
                        else
                        {
                            taskQueue.Enqueue(new Task(0, 0, 0, 0, 0, true));
                        }

                    }
                }
            }
            Console.WriteLine("Number of Questions : " + qCount.ToString());
        }
        public TestBlock(string nameID, string path, string pielogpath, int[] intID, int[] tachiID, int[] jukuID, int[] nD, int nRep)
            : this(nameID, path, pielogpath)
        {
            int qCount = 0;
            for (int l = 0; l < nD.Length; l++)
            {
                for (int k = 0; k < jukuID.Length; k++)
                {
                    for (int i = 0; i < intID.Length; i++)
                    {
                        for (int j = 0; j < tachiID.Length; j++)
                        {
                            int nR = nRep / nD[l];
                            if (nR < 1) nR = 1;
                            qCount += AddQuestion(intID[i], tachiID[j], jukuID[k], nD[l], nR);
                            if (i == intID.Length - 1 && j == tachiID.Length - 1 && k == jukuID.Length - 1 && l == nD.Length -1)
                            {
                                // do nothing
                            }
                            else
                            {
                                taskQueue.Enqueue(new Task(0, 0, 0, 0, 0, true));
                            }

                        }
                    }
                }
            }
            Console.WriteLine("Number of Questions : " + qCount.ToString());
        }

        protected int AddQuestion(int intID, int tachiID, int jukuID, int nD, int nRep)
        {
            //��萶��
            int count = 0;
            List<Task> tmpList = new List<Task>();
            for (int i = 0; i < nRep; i++)
            {
                for (int j = 1; j <= nD; j++)
                {
                    tmpList.Add(new Task(intID, tachiID, jukuID, nD, j, false));
                }
            }
            Shuffle(tmpList);
            foreach (Task t in tmpList)
            {
                taskQueue.Enqueue(t);
                count++;
            }
            tmpList.Clear();
            tmpList = null;
            return count;
        }
        protected void Shuffle(List<Task> list)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < nShuffleFactor * list.Count; i++)
            {
                int x = r.Next(list.Count);
                int y = r.Next(list.Count);
                Swap(list, x, y);
            }
        }
        protected void Swap(List<Task> list,int x, int y){
            Task a = list[x];
            Task b = list[y];
            list.RemoveAt(x);
            list.Insert(x, b);
            list.RemoveAt(y);
            list.Insert(y, a);
        }

        public bool GetNextTask(out Task task)
        {
            if (taskQueue.Count == 0)
            {
                task = new Task();
                return false;
            }
            curTask = taskQueue.Dequeue();
            task = curTask;
            isReadyTask = true;
            return true;
        }
        public void StartTask()
        {
            isReadyTask = false;
            isDoingTask = true;
            startTime = DateTime.Now;
            if (isFirst)
            {
                totalStartTime = startTime;
                isFirst = false;
            }
        }
        public void EndTask(int answer)
        {
            TimeSpan elapsed = DateTime.Now.Subtract(startTime);
            totalTimeElapsed = DateTime.Now.Subtract(totalStartTime);
            isDoingTask = false;
            WriteResult(elapsed, answer);
        }



        protected void WriteResult(TimeSpan elapsed, int answer){
            StringBuilder sb = new StringBuilder();
            sb.Append(nameID); sb.Append(",");
            sb.Append(curTask.interfaceID.ToString()); sb.Append(",");
            sb.Append(curTask.tachiichiID.ToString()); sb.Append(",");
            sb.Append(curTask.jukurenID.ToString()); sb.Append(",");
            sb.Append(curTask.nDiv.ToString()); sb.Append(",");
            sb.Append(curTask.question.ToString()); sb.Append(",");
            sb.Append(answer.ToString()); sb.Append(",");
            sb.Append(((bool)(curTask.question == answer)).ToString()); sb.Append(",");
            sb.Append(elapsed.ToString()); sb.Append(",");
            for(int i=0;i<curTask.nDiv;i++){
                sb.Append(curTask.stateMap[i]); sb.Append(",");
            }
            sb.Append("\n");

            string contents = sb.ToString();
            File.AppendAllText(logPath, contents,Encoding.GetEncoding("shift-jis"));      
        }

        //public void WritePieLog(HiddenPieHandler.PieMenuEventArgs e)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(DateTime.Now.Ticks.ToString()); sb.Append(",");
        //    sb.Append(e.Visible.ToString()); sb.Append(",");
        //    sb.Append(e.Position.X.ToString()); sb.Append(",");
        //    sb.Append(e.Position.Y.ToString()); sb.Append(",");
        //    sb.Append(e.Radius.ToString());
        //    sb.Append("\n");

        //    string contents = sb.ToString();
        //    File.AppendAllText(pieLogPath, contents, Encoding.GetEncoding("shift-jis"));      

        //}


        public struct Task
        {
            public int interfaceID;
            public int tachiichiID;
            public int jukurenID;
            public int nDiv;
            public int question;
            public bool isSeparator;
            public int[] stateMap;
            public Task(int intID,int tachiID,int jukuID,int nD,int que,bool isSepa)
            {
                interfaceID = intID;
                tachiichiID = tachiID;
                jukurenID = jukuID;
                nDiv = nD;
                question = que;
                isSeparator = isSepa;
                stateMap = new int[nD];
            }

            public void SetStateMap(int[] map)
            {
                for (int i = 0; i < nDiv; i++)
                {
                    stateMap[i] = map[i];
                }
            }

            public String Message(int nCount)
            {
                return
                    nCount.ToString() + "���:\n" +
                    "�y" +
                    TestBlock.jukurenIDString[jukurenID] +
                    "," + TestBlock.interFaceIDString[interfaceID] + // interfaceID.ToString() + 
                    "," + TestBlock.tachiichiIDString[tachiichiID] + // tachiichiID.ToString() +
                    "�z\n" + // jukurenID.ToString() + "\n" +
                    "�ۑ�:\n�y" + question.ToString() + "�z�Ԃ̃R�}���h�����s���Ă��������B\n\n" +
                    "��ʂ�3�{�w�Ń^�b�`����ƊJ�n���܂��B";
            }
        }
    }
}