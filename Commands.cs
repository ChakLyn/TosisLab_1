using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TosisLab_1.Infrastructure;

using Rand = TosisLab_1.Core.Core;

namespace TosisLab_1
{
    public partial class MainWindow : Window
    {
        private ICommand uniDistrCommand;
        private ICommand expDistrCommand;
        private ICommand normDistrCommand;
        private ICommand tringDistrCommand;
        private ICommand discDistrCommand;

        private void ConvertToText()
        {
            TbMtTr.Text = MtTeor.ToString();
            TbDtTr.Text = DtTeor.ToString();
            TbMtPr.Text = MtPr.ToString();
            TbDtPr.Text = DtPr.ToString();
        }

        private ICommand UniDistr
        {
            get
            {
                if (this.uniDistrCommand == null)
                {
                    this.uniDistrCommand = new Command(
                        this.ExecuteUniDistrCommand,
                        this.CanExecuteExecuteUniDistrCommand);
                }

                return this.uniDistrCommand;
            }
        }

        private double a;
        private double b;
        private int n;
        private double lamda;
        private double alfa;
        private double sigma;
        private double alfaTring;

        private bool CanExecuteExecuteUniDistrCommand(object obj)
        {
            return (TbAParam.Text != "" && Double.TryParse(TbAParam.Text, out a))&&
                (TbBParam.Text != "" && Double.TryParse(TbBParam.Text, out b)) &&
                (TbN.Text != "" && Int32.TryParse(TbN.Text, out n) && n>0);
        }

        private void ExecuteUniDistrCommand(object obj)
        {
            Rand.UniDistr(a, b, n, ref MtTeor, ref DtTeor, ref MtPr, ref DtPr);
            ConvertToText();
        }

        private ICommand ExpDistr
        {
            get
            {
                if (this.expDistrCommand == null)
                {
                    this.expDistrCommand = new Command(
                        this.ExecuteExpDistrCommand,
                        this.CanExecuteExpDistrCommand);
                }

                return this.expDistrCommand;
            }
        }

        private bool CanExecuteExpDistrCommand(object obj)
        {
            return (TbLamdaParam.Text != "" && Double.TryParse(TbLamdaParam.Text, out lamda) && lamda>0) &&
            (TbN.Text != "" && Int32.TryParse(TbN.Text, out n) && n > 0);
        }

        private void ExecuteExpDistrCommand(object obj)
        {
            double lamda = Convert.ToDouble(TbLamdaParam.Text);

            Rand.ExpDistr(lamda, n, ref MtTeor, ref DtTeor, ref MtPr, ref DtPr);
            ConvertToText();
        }

        private ICommand NormDistr
        {
            get
            {
                if (this.normDistrCommand == null)
                {
                    this.normDistrCommand = new Command(
                        this.ExecuteNormDistrCommand,
                        this.CanExecuteNormDistrCommand);
                }

                return this.normDistrCommand;
            }
        }

        private bool CanExecuteNormDistrCommand(object obj)
        {
            return (TbAlfaParam.Text != "" && Double.TryParse(TbAlfaParam.Text, out alfa)) &&
            (TbSigmParam.Text != "" && Double.TryParse(TbSigmParam.Text, out sigma) && sigma >= 0) &&
            (TbN.Text != "" && Int32.TryParse(TbN.Text, out n) && n > 0);
        }

        private void ExecuteNormDistrCommand(object obj)
        {
            double k = 0;
            if (Rb6.IsChecked == true)
                k = 6;
            if (Rb12.IsChecked == true)
                k = 12;
            if (Rb18.IsChecked == true)
                k = 18;

            Rand.NormDistr(alfa, sigma, k, n, ref MtTeor, ref DtTeor, ref MtPr, ref DtPr);
            ConvertToText();
        }

        private ICommand TringDistr
        {
            get
            {
                if (this.tringDistrCommand == null)
                {
                    this.tringDistrCommand = new Command(
                        this.ExecuteTringDistrCommand,
                        this.CanExecuteTringDistrCommand);
                }

                return this.tringDistrCommand;
            }
        }

        private bool CanExecuteTringDistrCommand(object obj)
        {
            return (TbAlfaTrigParam.Text != "" && Double.TryParse(TbAlfaTrigParam.Text, out alfaTring)) &&
            (TbN.Text != "" && Int32.TryParse(TbN.Text, out n) && n > 0);
        }

        private void ExecuteTringDistrCommand(object obj)
        {
            double alfa = Convert.ToDouble(TbAlfaTrigParam.Text);

            Rand.TrinDistr(alfa, n, ref MtTeor, ref DtTeor, ref MtPr, ref DtPr);
            ConvertToText();
        }

        private ICommand DiscDistr
        {
            get
            {
                if (this.discDistrCommand == null)
                {
                    this.discDistrCommand = new Command(
                        this.ExecuteDiscDistrCommand,
                        this.CanExecuteDiscDistrCommand);
                }

                return this.discDistrCommand;
            }
        }

        private bool CanExecuteDiscDistrCommand(object obj)
        {
            bool errors = (from c in
                     (from object i in DiscTable.ItemsSource
                      select DiscTable.ItemContainerGenerator.ContainerFromItem(i))
                          where c != null
                          select Validation.GetHasError(c))
                .Any(x => x);
            return _dicsList.Sum(pi => pi.P) == 1 && !errors 
                && (TbN.Text != "" && Int32.TryParse(TbN.Text, out n) && n > 0);
        }

        private void ExecuteDiscDistrCommand(object obj)
        {
            List<double> p = _dicsList.Select(pi => pi.P).ToList();
            List<double> y = _dicsList.Select(pi => pi.Y).ToList();
            Rand.DiscrDistr(y, p, n, ref MtTeor, ref DtTeor, ref MtPr, ref DtPr);
            ConvertToText();
        }

    }
}
