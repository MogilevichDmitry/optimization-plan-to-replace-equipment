using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        public static double F_min(double t_k, double N, double n)
        {
            if (t_k < N - n)
            {
                return t_k;
            }
            else
            {
                return N - n;
            }
        }
        static void Main(string[] args)
        {
            int N;
            Console.WriteLine("Введите  кол-во единиц периодов: ");
            N = Int32.Parse(Console.ReadLine());
            int M;
            Console.WriteLine("Введите кол-во типов различных устройств: ");
            M = Int32.Parse(Console.ReadLine());
            List<int> Mk = new List<int>();
            List<int> L_k = new List<int>();
            List<double> C_k = new List<double>();
            List<double> R_n = new List<double>();
            List<double> t_1k = new List<double>();
            List<double> t_2k = new List<double>();
            List<double> V_n = new List<double>();
            string strC_k;
            string strL_k;
            string strt1_k;
            string strt2_k;
            string strMk;
            string strR_n;
            string strMU_ik;
            string[] parametrC_k;
            string[] parametrL_k;
            string[] parametrt1_k;
            string[] parametrt2_k;
            string[] parametrMk;
            string[] parametrR_n;
            string[] parametrMU_ik;
            double V_sr;
            Random rand = new Random();     
            double alpha;
            double s, ss;
            double w;
            double V_alpha;
            double d;
            bool logic_p3 = false;
            bool logic_p3_2 = false;

            Console.WriteLine("Введите кол-во устройств k-го типа: ");
            strMk = Console.ReadLine();
            parametrMk = strMk.Split(' ', ';');
            int sumApparatus = 0;
            for (int i = 0; i < M; i++)
            {
                Mk.Add(Int32.Parse(parametrMk[i]));
                sumApparatus += Mk[i];
            }
            List<double> MU_ik = new List<double>();
            for (int i = 0; i < M; i++)
            {
                Console.WriteLine("Введите время эксплуатации i-го устройства " + (i + 1) + "-го типа:");
                strMU_ik = Console.ReadLine();
                parametrMU_ik = strMU_ik.Split(' ', ';');
                for (int j = 0; j < Mk[i]; j++)
                {
                    MU_ik.Add(Double.Parse(parametrMU_ik[j]));
                }               
            }
             Console.WriteLine("Введите нормативный срок эксплуатации устройства k-го типа: ");            
             strL_k = Console.ReadLine();
             parametrL_k = strL_k.Split(' ', ';');
             Console.WriteLine("Введите трудоемкость k-го типа: ");
             strC_k = Console.ReadLine();
             parametrC_k = strC_k.Split(' ', ';');   
             Console.WriteLine("Введите предельный допуск на уменьшение срока замены k-го типа: ");
             strt1_k = Console.ReadLine();
             parametrt1_k = strt1_k.Split(' ', ';');
             Console.WriteLine("Введите предельный допуск на увеличение срока замены k-го типа: ");
             strt2_k = Console.ReadLine();
             parametrt2_k = strt2_k.Split(' ', ';');
             for (int i = 0; i < M; i++)
             {
                 L_k.Add(Int32.Parse(parametrL_k[i]));
                 C_k.Add(Double.Parse(parametrC_k[i]));
                 t_1k.Add(Double.Parse(parametrt1_k[i]));
                 t_2k.Add(Double.Parse(parametrt2_k[i]));
             }
             Console.WriteLine("Введите коэффициент корректировки трудоёмкости: ");
             strR_n = Console.ReadLine();
             parametrR_n = strR_n.Split(' ', ';');
             for (int i = 0; i < N; i++)
             {
                 R_n.Add(Double.Parse(parametrR_n[i]));
             }
            
            int[,] Aikn = new int[sumApparatus, N + 1];
            int k = 1, count = 0, tempCount = 0;
            for (int j = 0; j < M; j++)
            {
                tempCount = 0;
                for (int i = 0; i < Mk[j]; i++)
                {
                    Aikn[i + count, 0] = k;                  
                    tempCount++;
                }
                k++;
                count += tempCount;
            }

           /* Aikn for (int j = 0; j < Aikn.GetLength(0); j++)
            {
                for (int i = 0; i < Aikn.GetLength(1); i++)
                {
                    Console.Write(Aikn[j, i]);
                }
                Console.WriteLine();
            }*/
            for (int ki = 0; ki < Aikn.GetLength(0); ki++)
            {
                int tempAik = 0;
                for (int month = 1; month <= N; month++)
                {
                    if ((month + MU_ik[ki] - 1) % L_k[Aikn[ki, 0] - 1] == 0)
                    {
                        tempAik = 1;
                    }
                    else
                    {
                        tempAik = 0;
                    }                   
                    Aikn[ki, month] = tempAik;                  
                }
            }
            int[,] Bikn = new int[Aikn.GetLength(0), Aikn.GetLength(1)];
            Console.WriteLine("Aikn = ");
            for (int i = 0; i < Aikn.GetLength(0); i++) // bikn = aikn;
            {
                for (int j = 0; j < Aikn.GetLength(1); j++)
                {
                    Console.Write(Aikn[i, j] + " ");
                    Bikn[i, j] = Aikn[i, j];
                }
                Console.WriteLine();                
            }
            
            /* Vn */
            double tempSumAikn = 0;
            for (int i = 1; i < Aikn.GetLength(1); i++)
                {
                    for (int j = 0; j < Aikn.GetLength(0); j++)
                    {
                        tempSumAikn += C_k[Aikn[j,0] - 1]*Aikn[j, i];
                    }                    
                V_n.Add(tempSumAikn);
                tempSumAikn = 0;
                Console.WriteLine(V_n[i - 1] + " ");
            }
            V_sr = (1.0 / N) * (V_n.Sum());
            Console.WriteLine("V_sr: " + V_sr);
            /* Xiknp */
            int[,,] Xiknp = new int[sumApparatus, N + 1, N + 1];
            for ( int i = 0; i < Xiknp.GetLength(0); i++)
            {
                for( int n = 1; n < Xiknp.GetLength(1); n++)
                {
                    for(int p = 1; p <Xiknp.GetLength(2); p++)
                    {
                        if ( p == n)
                        {
                            Xiknp[i, n, p] = Aikn[i, n];
                        }
                        else
                        {
                            Xiknp[i, n, p] = 0;
                        }
                        Xiknp[i, 0, 0] = Aikn[i, 0];
                    }
                }
            }
            for (int i = 0; i < Xiknp.GetLength(0); i++)
            {
                for (int n = 0; n < Xiknp.GetLength(1); n++)
                {
                    for (int p = 0; p < Xiknp.GetLength(2); p++)
                    {
                        Console.Write(Xiknp[i, n, p] +" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine("-------------------------------");
            count = 0;
            int count2 = 0;
            /* п. 2 */
            for (k = 1; k <= M; k++)
            {
                /* п.3 */
                for (int n = 1; n <= N - 1; n++)
                {
                    logic_p3_2 = false;
                    logic_p3 = false;
                    s = V_sr * (R_n[n - 1] / C_k[k - 1]);
                    ss = (V_sr * R_n[n - 1]) % C_k[k - 1];

                    alpha = rand.NextDouble();
                    Console.WriteLine("alpha: " + alpha);                   
                    if (alpha > s - ss)
                    {
                        V_alpha = ss * C_k[k - 1];
                    }
                    else
                    {
                        V_alpha = (ss + 1) * C_k[k - 1];
                    }
                    w = V_alpha * R_n[n - 1] - V_n[n - 1];
                    if (Math.Abs(w) < C_k[k - 1])
                    {
                        // =>>> p3; ++++ 
                    }
                    else if (w > 0)
                    {
                        // =>>> p5; ++++
                        for (int j = 1; j <= F_min(t_1k[k - 1], N, n); j++)
                        {
                            tempCount = 0;
                            /* п.6 */
                            for (int i = 1; i <= Mk[k - 1]; i++)
                            {
                                tempCount++;
                                /* п. 7 */
                                if (Bikn[i + count - 1, n] == 1 || Bikn[i - 1, n + j] == 0)
                                {

                                }
                                else
                                {
                                    for (int lamda = n; lamda < N; lamda += L_k[k - 1])
                                    {
                                        Bikn[i + count - 1, lamda] = 1;
                                        V_n[lamda - 1] += C_k[k - 1];
                                    }
                                    for (int lamda = n + j; lamda < N; lamda += L_k[k - 1])
                                    {
                                        Xiknp[i + count - 1, lamda, lamda] = 0;//? index
                                        Xiknp[i + count - 1, lamda, lamda - j] = 1;//? index
                                        Bikn[i + count - 1, lamda] = 0;
                                        V_n[lamda - 1] -= C_k[k - 1];
                                    }
                                    d = w - C_k[k - 1];
                                    if (d < C_k[k - 1])
                                    {
                                        // =>>> p.3;
                                        logic_p3 = true;
                                        break;
                                    }
                                    else
                                    {
                                        w = d;
                                        Console.WriteLine("finish");
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            count += Mk[k - 1]+1;
                            if (logic_p3)
                            {
                                break;
                            }
                        }
                    }
                    else if (w <= 0)
                    {
                        // =>>> p8;
                        for (int i = 1; i <= Mk[k - 1]; i++)
                        {
                            tempCount = 0;
                            /* п.9 */
                            for (int j = 1; j <= F_min(t_2k[k - 1], N, n); j++)
                            {
                                tempCount++;
                                if (Bikn[i + count2 - 1, n] == 0)
                                {
                                    // k p.8++++
                                    break;
                                }
                                else
                                {

                                    if (Xiknp[i + count2 - 1, n, n + j - 1] == 0) 
                                    {
                                        // k p.9 +++
                                    }
                                    else
                                    {
                                        d = w + C_k[k - 1];

                                        for (int lamda = n + 1; lamda < N; lamda += L_k[k - 1])
                                        {
                                            Xiknp[i + count2 - 1, lamda, lamda + j] = 1;
                                            Bikn[i + count2 - 1, lamda] = 1;
                                            V_n[lamda - 1] += C_k[k - 1];
                                        }

                                        for (int lamda = n; lamda < N; lamda += L_k[k - 1])
                                        {
                                            Xiknp[i + count2 - 1, lamda, lamda - 1] = 0;
                                            Bikn[i + count2 - 1, lamda] = 0;
                                            V_n[lamda - 1] -= C_k[k - 1];
                                        }
                                        if (d < C_k[k - 1])
                                        {
                                            // k p.3
                                            logic_p3_2 = true;
                                            break;
                                        }
                                        else
                                        {
                                            w = d;
                                            Console.WriteLine("finish");
                                            Environment.Exit(0);
                                        }
                                    }
                                }
                            }
                           
                            if(logic_p3_2)
                            {
                                break;
                            }
                        }
                    }
                }
                count2 += Mk[k - 1];
            }

            for (int i = 0; i < Xiknp.GetLength(0); i++)
            {
                for (int n = 0; n < Xiknp.GetLength(1); n++)
                {
                    for (int p = 0; p < Xiknp.GetLength(2); p++)
                    {
                        Console.Write(Xiknp[i, n, p] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------------");
            Console.WriteLine("Bikn = ");
            for (int i = 0; i < Bikn.GetLength(0); i++) 
            {
                for (int j = 0; j < Bikn.GetLength(1); j++)
                {
                    Console.Write(Bikn[i, j] + " ");                    
                }
                Console.WriteLine();
            }

        }
    }
}
