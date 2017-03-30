using System;
using System.Collections.Generic;
using System.Linq;

namespace TosisLab_1.Core
{
    public static class Core
    {
        private static Random nParametr = new Random();

        public static void UniDistr(double a, double b, int n, ref double mtT, ref double dtT,
            ref double mtPr, ref double dtPr)
        {
            mtT = (a + b) / 2;
            dtT = Math.Pow(b - a, 2) / 12;

            List<double> listOfXi = new List<double>();
            for (int i = 0; i < n; i++)
            {
                listOfXi.Add(a + ((b - a) * nParametr.NextDouble()));
            }

            MtAndDtPr(n, listOfXi, ref mtPr, ref dtPr);
        }

        public static void ExpDistr(double lamda, int n, ref double mtT, ref double dtT,
            ref double mtPr, ref double dtPr)
        {
            mtT = 1 / lamda;
            dtT = 1 / Math.Pow(lamda, 2);
            List<double> listOfXi = new List<double>();
            for (int i = 0; i < n; i++)
            {
                listOfXi.Add(-((1 / lamda) * Math.Log(nParametr.NextDouble())));
            }

            MtAndDtPr(n, listOfXi, ref mtPr, ref dtPr);
        }

        public static void NormDistr(double a, double sigm, double k, int n, ref double mtT, ref double dtT,
            ref double mtPr, ref double dtPr)
        {
            mtT = a;
            dtT = Math.Pow(sigm, 2);
            List<double> listOfXi = new List<double>();
            for (int i = 0; i < n; i++)
            {
                listOfXi.Add(n0_1(k) * sigm + a);
            }
            MtAndDtPr(n, listOfXi, ref mtPr, ref dtPr);
        }

        public static void TrinDistr(double a, int n, ref double mtT, ref double dtT,
            ref double mtPr, ref double dtPr)
        {
            mtT = (2 * a) / 3;
            dtT = Math.Pow(a, 2) / 18;
            List<double> listOfXi = new List<double>();
            for (int i = 0; i < n; i++)
            {
                listOfXi.Add(a * Math.Sqrt(nParametr.NextDouble()));
            }
            MtAndDtPr(n, listOfXi, ref mtPr, ref dtPr);
        }

        public static void DiscrDistr(List<double> yRow, List<Double> pRow, int n, ref double mtT, ref double dtT,
            ref double mtPr, ref double dtPr)
        {
            mtT = 0.0;
            dtT = 0.0;
            List<double> listOfXi = new List<double>();
            List<double> sum = new List<double>();
            for (int i = 0; i < yRow.Count; i++)
            {
                mtT += yRow[i] * pRow[i];
            }
            for (int i = 0; i < yRow.Count; i++)
            {
                dtT += (Math.Pow(yRow[i], 2) * pRow[i]);
                // dtT += Math.Pow((yRow[i]-mtT),2)*pRow[i]
                if (i == 0)
                    sum.Add(pRow[i]);
                else
                    sum.Add(sum[i - 1] + pRow[i]);
            }
            dtT -= Math.Pow(mtT, 2);
            for (int k = 0; k < n; k++)
            {
                int index = 0;
                double tempN = nParametr.NextDouble();
                for (int j = 0; j < sum.Count; j++)
                {
                    if (j == 0 && tempN <= sum[j])
                    {
                        index = j;
                        break;
                    }
                    if (j > 0 && (sum[j - 1] < tempN && tempN <= sum[j]))
                    {
                        index = j;
                        break;
                    }
                }
                listOfXi.Add(yRow[index]);
            }
            MtAndDtPr(n, listOfXi, ref mtPr, ref dtPr);
        }

        private static double n0_1(double k)
        {
            double sum = 0;
            for (int i = 0; i < k; i++)
            {
                sum += nParametr.NextDouble() - 0.5;
            }
            return Math.Sqrt(12 / k) * sum;
        }

        private static void MtAndDtPr(double n, List<Double> x, ref double mt, ref double dt)
        {
            mt = (1.0 / n) * x.Sum(xi => xi);

            // for avoiding error in lamda expression
            double loaclMt = mt;
            dt = (1.0 / n) * x.Sum(xi => Math.Pow(xi - loaclMt, 2));
        }
    }
}
