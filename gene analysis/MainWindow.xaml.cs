using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gene_analysis
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]

        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }
    

        public MainWindow()
        {
            InitializeComponent();
            f.push_back(2);
            f.push_back(3);
            for (int i = 2; i < 37; i++)
            {
                f.push_back(f.At(i-2)+f.At(i-1));
            }
          
        }

        Vector<int> f = new Vector<int>();
        Vector<int> genes = new Vector<int>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FlushMemory();
                System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
                var r = new StreamReader("genes.txt");
                var wr = new StreamWriter("damage.txt");
                string[] info = r.ReadToEnd().Split('\n');
                Vector<Tuple<int, string>> data = new Vector<Tuple<int, string>>();
                MessageBox.Show("The process is started!");
                myStopwatch.Start(); //запуск
              
                for (int i = 0; i < info.Length; i++)
                {
                    string[] ex = info[i].Split(' ');
                    data.push_back(new Tuple<int, string>(i,id(ex)));
                }
                for(int i = 1; i < data.Count; i++)
                {
                    if(data.At(i).Item2 != data.At(0).Item2)
                    {
                        wr.WriteLine("[" + i.ToString() + "] " + info[i]);
                    }
                }
                wr.Close();
                myStopwatch.Start();
                MessageBox.Show("Successfully! "+myStopwatch.Elapsed.ToString());
                System.Diagnostics.Process.Start("damage.txt");
                r.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
       
       string id(string [] ex)
        {
            int k = ex.Length;
                while (k != 2)
                {
                    Vector<string> res = new Vector<string>();
                    for (int i = 0; i < k - 1; i += 2)
                    {
                    
                        res.push_back((f.At(Convert.ToInt32(ex[i])) + f.At(Convert.ToInt32(ex[i + 1]))).ToString());
                   
                    }
               
                ex = new string[res.Count];
                k = res.Count;
                    ex = res.ToArray();
                }

            if (ex[0] == ex[1])
            {
                return ex[0];
            }
            else
            {
                return "damage";
            }
        }
        
    }
    class Vector<T>
    {
        T[] array;
        public Vector()
        {
            array = new T[0];
        }
        public Vector(int count)
        {
            array = new T[count];
        }
        public T At(int index)
        {

            return array[index];
        }

        public T back
        {
            get
            {
                return array[array.Length - 1];
            }
            set
            {
                array[array.Length - 1] = value;
            }
        }
        public void isat(int index, T value)
        {

            array[index] = value;

        }
        public T Front
        {
            get
            {
                return array[0];
            }
            set
            {
                array[0] = value;
            }
        }
        public void Clear()
        {
            array = new T[0];
        }

        public void Insert(int pos, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            for (int i = array.Length - 1; i > pos; i--)
                array[i] = array[i - 1];
            array[pos] = item;
        }
        public void push_back(T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }

        public void pop_back()
        {
            Array.Resize(ref array, array.Length - 1);

        }
        public void push_front(T item)
        {
            Insert(0, item);
        }
        public int Count
        {
            get
            {
                return array.Length;
            }
        }
        public T[] ToArray()
        {
            return array;
        }
    }
}
