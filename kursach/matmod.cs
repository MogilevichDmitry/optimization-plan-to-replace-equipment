using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
    class matmod
    {
        static void Main(string[] args)
        {
            List<int> A_ikn = new List<int>(); // {0 or 1} первоначальный план
            List<int> Mi = new List<int>(); // {2,1,4}
            List<double> C_k = new List<double>(); //трудоемкость
            List<double> MY_ik = new List<double>(); //время экспл.  на начало планового периода
            List<double> L_k = new List<double>();
            int n = 1; // единица планового периода
            int M; //кол-во типов различных устройств
            int N = 3; // кол-во единиц планового периода
            M = Int32.Parse(Console.ReadLine());
            int K; // тип устройства
            double Vn = 0;// трудоемкость в n-ом интервале планового периода 
            double Vn_s = 0; // средняя трудоемкость за весь плановый период
            int SumMachines = 0;// сумма устр-в

            for (int i = 0; i < M; i++)// число устройств k-го типа
            {
                int temp = Int32.Parse(Console.ReadLine());
                Mi.Add(temp);
                SumMachines += temp;

            }

            for (int i = 0; i < SumMachines; i++) // add L_k //add MY_ik // add Aikn
            {
                double tempL_k = Double.Parse(Console.ReadLine());
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
            /*----------------(6)-------------*/
            for (int i = 0; i < M; i++) // add C_k // Vn 
            {
                double tempC_k = Double.Parse(Console.ReadLine());
                C_k.Add(tempC_k);
                double tempSumAi = 0;
                for (int j = 0; j < Mi[i]; j++)
                {
                    tempSumAi += A_ikn[j];
                }
                Vn += tempC_k * tempSumAi;// трудоемкость в n-ом интервале планового периода
            }

            for (int i = 0; i < N; i++)
            {
                Vn_s += 1 / N * Vn; // средняя трудоемкость за весь период эксплуатации
            }
            /* ---------- составление нового плана -------------------*/
            double Vn_b = 0; // трудоемкость в соответсвии с новым ПЗ 
            List<int> X_iknj = new List<int>(); // [0;1]
            List<double> t_1k = new List<double>(); //предельный допуск на уменьшение срока замены 
            List<double> t_2k = new List<double>();//предельный допуск на увеличение срока замены 
            for ( int i = 0; i < M; i ++) 
            {
                t_1k.Add(Double.Parse(Console.ReadLine()));
                t_2k.Add(Double.Parse(Console.ReadLine()));

            }
            List<double> J_kn_max = new List<double>();
            List<double> J_kn_min = new List<double>();
            List<double> S_kn = new List<double>();
            List<double> S_kn_2 = new List<double>();
            List<double> T_kn_max = new List<double>();
            List<double> T_kn_min = new List<double>();

            List<int> B_ikn = new List<int>();
            for( int i = 0;  i < A_ikn.Count + 1; i++) // первоначально  B_ikn = A_ikn 
            {
                B_ikn.Add(A_ikn[i]);
            }
            /* ---------------------Xiknp --------------*/
            for (int p = 0; p < N; p++)
            {
                if ( p == n)
                {
                    X_iknj.Add(A_ikn[p]);
                }
                else
                {
                    X_iknj.Add(0);
                }
            }
                        
            /*-------------- Jkn(1)-------------------*/
            for ( int i = 0; i < M; i++)
            {
                if ( n - t_1k[i] > 1)
                {
                    J_kn_max.Add(n - t_1k[i]);
                }
                else
                {
                    J_kn_max.Add(1);
                }
            }
            /*-------------- Jkn(2)-------------------*/
            for (int i = 0; i < M; i++)
            {
                if (n + t_2k[i] < N)
                {
                    J_kn_min.Add(n + t_2k[i]);
                }
                else
                {
                    J_kn_min.Add(N);
                }
            }
            /*-------------- Skn(1)-------------------*/
            for ( int i = 0; i < M; i++ )
            {
                if (n - t_1k[i] - L_k[i] > 1)
                {
                    S_kn.Add(n - t_1k[i] - L_k[i]);
                }
                else
                {
                    S_kn.Add(1);
                }
            }
            /*-------------- Skn(2)-------------------*/
            for (int i = 0; i< M; i++)
            {
                S_kn_2.Add(n + t_1k[i] - L_k[i]);
            }
            /*--------------------------(11)--------------??----*/
            int tempXiknj = 0;
            for (int i = 0; i < J_kn_max.Count; i++)
            {
                for (int j = (int)J_kn_max[i]; j < J_kn_min[i + 1]; j++)
                {
                    tempXiknj = 0;
                    if (n <= L_k[j])
                    {
                        tempXiknj = A_ikn[j];
                    }
                    else
                    {
                        for (int s = (int)S_kn[j]; s < S_kn_2[j + 1]; s++) 
                        {
                            tempXiknj += X_iknj[s];
                        }
                    }
                    tempXiknj += tempXiknj;
                }
                X_iknj.Add(tempXiknj);
            }               
          
            /*--------------------- Tkn(1)---------------------*/
            for ( int i = 0; i < M; i++)
            {
                if (n - t_2k[i] > 1)
                {
                    T_kn_max.Add(n - t_2k[i]);
                }
                else
                {
                    T_kn_max.Add(1);
                }
            }
            /*--------------------- Tkn(2)---------------------*/
            for (int i = 0; i < M; i++)
            {
                if (n + t_1k[i] < N)
                {
                    T_kn_min.Add(n + t_1k[i]);
                }
                else
                {
                    T_kn_min.Add(N);
                }
            }
            /*----------------------(12)------------------??-----*/
            int tempX_ikn;
            for( int i = 0; i < M; i++)
            {
                tempX_ikn = 0;
                for (int j = ((int)T_kn_max[i]); j < (int)(T_kn_min[i + 1]); j++) 
                {
                    tempX_ikn += X_iknj[j];
                }
                B_ikn.Add(tempX_ikn);
            }
            List<double> R_n = new List<double>();// коэф. корректировки трудоемкости
            for ( int i = 0; i < N; i++) 
            {
                R_n.Add(Double.Parse(Console.ReadLine()));
            }
           
            double tempSum1 = 0;
            double tempSum2 = 0;
            double F_B = 0;
            /*-----------------(13)----------------*/
            for (int i = 0; i < N; i++)
            {
                tempSum2 = 0;
                for (int j = 0; j < M; j++)
                {
                    tempSum1 = 0;
                    for (int k = 0; k < Mi[j]; k++)
                    {
                        tempSum1 += C_k[j] * B_ikn[k];
                    }
                    tempSum2 += tempSum1;
                }
                F_B += (tempSum2 - R_n[i] * Vn_s) * (tempSum2 - R_n[i] * Vn_s);
            }
        }
    }
}