using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class Program
    {
        public static double F_min(double t_k, double N, double n)
        {
            if ( t_k < N - n)
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
            List<int> A_ikn = new List<int>(); // {0 or 1} первоначальный план
            List<int> Mi = new List<int>(); // {2,1,4}
            List<double> C_k = new List<double>(); //трудоемкость
            List<double> R_n = new List<double>();//коэф. коррект. трудоемкости
            List<double> MY_ik = new List<double>();//время экспл.  на начало планового периода
            List<int> L_k = new List<int>();
            List<double> Vn = new List<double>();
            List<int> B_ikn = new List<int>();
            List<int> X_iknp = new List<int>();
            List<double> t_1k = new List<double>();
            List<double> t_2k = new List<double>();
            double V_sr = 0;
            int n = 1; // единица планового периода
            int M; //кол-во типов различных устройств
            int N = 3; // кол-во единиц планового периода
            int SumMachines = 0;// сумма устр-в
            int s;
            int ss;
            double w;
            double d;
            Random rand = new Random();     // генерируем число от 0 до 1
            double alpha;                   // по равномерному закону распределения
            List<double> V_alpha = new List<double>();
            M = Int32.Parse(Console.ReadLine());

            /* п.1 */
            for (int i = 0; i < M; i++)// число устройств k-го типа
            {
                int temp = Int32.Parse(Console.ReadLine());
                Mi.Add(temp);
                SumMachines += temp;

            }
            for (int i = 0; i < SumMachines; i++) // add L_k //add MY_ik // add Aikn
            {
                int tempL_k = Int32.Parse(Console.ReadLine());
                L_k.Add(tempL_k);
                for (int j = 0; j < Mi[i]; j++)
                {
                    int tempMY_ik = Int32.Parse(Console.ReadLine());
                    MY_ik.Add(tempMY_ik);
                    int tempAik = 0;
                    if ((n + tempMY_ik - 1) % tempL_k == 0)
                    {
                        tempAik = 1;
                    }
                    A_ikn.Add(tempAik);
                }
            }
            double tempVn;
            for (int i = 0; i < N; i++)
            {
                tempVn = 0;
                for (int j = 0; j < M; j++) // add C_k // Vn 
                {
                    double tempC_k = Double.Parse(Console.ReadLine());
                    C_k.Add(tempC_k);
                    double tempSumAi = 0;
                    for (int q = 0; q < Mi[j]; q++)
                    {
                        tempSumAi += A_ikn[q];
                    }
                    tempVn += tempC_k * tempSumAi;// трудоемкость в n-ом интервале планового периода
                }
                Vn.Add(tempVn);
            }
            tempVn = 0;
            for (int i = 0; i < N; i++)
            {
                tempVn += Vn[i];
            }
            V_sr = 1 / N * tempVn;//средняя трудоемкость 
            for (int i = 0; i < A_ikn.Count; i++) 
            {
                B_ikn.Add(A_ikn[i]);
            }
            for (int p = 0; p < N; p++)
            {
                if (p == n) //????
                {
                    X_iknp.Add(A_ikn[p]);
                }
                else
                {
                    X_iknp.Add(0);
                }
            }
            for (int i = 0; i < N - 1; i++)
            {
                double tempR_n = Double.Parse(Console.ReadLine());
                R_n.Add(tempR_n);
            }
            for ( int i = 0; i < M; i++ )
            {
                double tempt1_k = Double.Parse(Console.ReadLine());
                t_1k.Add(tempt1_k);
                double tempt2_k = Double.Parse(Console.ReadLine());
                t_2k.Add(tempt2_k);
            }
            /* п.2 */
            for (int k = 0; k < M; k++) 
            {
                /* п.3 */
                for (n = 0; n < N - 1; n++)
                {
                    /* п.4 */
                    s = (int)(V_sr * R_n[n] / C_k[k]);
                    ss = (int)((V_sr * R_n[n]) % C_k[k]);
                    alpha = rand.NextDouble();
                    if (alpha > s - ss)
                    {
                        V_alpha.Add(ss * C_k[k]);
                    }
                    else
                    {
                        V_alpha.Add((ss + 1) * C_k[k]);
                    }
                    w = V_alpha[n] * R_n[n] - Vn[n];
                    while (Math.Abs(w) < C_k[k])
                    {
                        //перейти к п.3 +++
                        n++;
                        s = (int)(V_sr * R_n[n] / C_k[k]);
                        ss = (int)((V_sr * R_n[n]) % C_k[k]);
                        alpha = rand.NextDouble();
                        if (alpha > s - ss)
                        {
                            V_alpha.Add(ss * C_k[k]);
                        }
                        else
                        {
                            V_alpha.Add((ss + 1) * C_k[k]);
                        }
                        w = V_alpha[n] * R_n[n] - Vn[n];
                    }
                    if (w > 0)
                    {
                        //перейти к п.5 
                    }
                    else if (w <= 0)
                    {
                        //перейти к п.8
                    }
                    /* п.5  */
                    for ( int j = 0; j < F_min(t_1k[k],N, n); j++)
                    {
                        /* п.6 */
                        for (int i = 0; i < Mi[k]; i++) //????
                        {
                            /* п.7 */
                            while (B_ikn[n] == 1 || B_ikn[n + j] == 0) 
                            {
                                for (int lambda = n; lambda < N; lambda += L_k[k]) 
                                {
                                    B_ikn[lambda] = 1;
                                    V_alpha[lambda] = V_alpha[lambda] + C_k[k]; // ????
                                }
                                for (int lambda = n + j; lambda < N; lambda += L_k[k])
                                {
                                    X_iknp[lambda] = 0;
                                    X_iknp[lambda - j] = 1;
                                    B_ikn[lambda] = 0;
                                    V_alpha[lambda] = V_alpha[lambda] - C_k[k];
                                }
                                d = w - C_k[k];
                                if (d < C_k[k])
                                {
                                    //перейти к п.3 
                                }
                                else
                                {
                                    w = d;
                                }
                                break; //перейти к п.6 +++
                            }
                            /* п. 8 */
                            for (i = 0; i < Mi[k]; i++)
                            {
                                /* п. 9 */
                                for (j = 1; j < F_min(t_2k[k], N, n); j++)
                                {
                                    if (B_ikn[n] == 0)
                                    {
                                        //перейти к п.8
                                    }
                                    else
                                    {
                                        while (X_iknp[n + j - 1] == 0)
                                        {
                                            //перейти к п.9 
                                            break;//????????????
                                        }
                                        d = w + C_k[k];
                                    }
                                    for (int lambda = n + 1; lambda < N; lambda += L_k[k])
                                    {
                                        X_iknp[lambda + j] = 1;
                                        B_ikn[lambda] = 1;
                                        V_alpha[lambda] = V_alpha[lambda] + C_k[k];
                                    }
                                    for (int lambda = n; lambda < N; lambda += L_k[k])
                                    {
                                        X_iknp[lambda - 1] = 0;
                                        B_ikn[lambda] = 0;
                                        V_alpha[lambda] = V_alpha[lambda] - C_k[k];
                                    }
                                    if (d < C_k[k]) // ????
                                    { 
                                        //перейти к п.3
                                    }
                                    else
                                    {
                                        w = d;
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }            
        }
    }
}
