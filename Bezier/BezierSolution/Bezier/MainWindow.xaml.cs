using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Bezier
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BezierDrawing_Base.ParentWnd = this;
            BezierDrawing_Method_de_Castelljo.ParentWnd = this;
        }

        public bool isVisibleBDB = false;
        public bool isVisibleBDMC = false;

        private BezierDrawing_Base windB;
        private BezierDrawing_Method_de_Castelljo windC;

        private void ButtonBezier_Click(object sender, RoutedEventArgs e)
        {
            windB = new BezierDrawing_Base();
            if(!isVisibleBDB)
            {
                isVisibleBDB = true;
                windB.Show();
            }
        }

        private void ButtonBezierCast_Click(object sender, RoutedEventArgs e)
        {
            windC = new BezierDrawing_Method_de_Castelljo();
            if (!isVisibleBDMC)
            {
                isVisibleBDMC = true;
                windC.Show();
            }
        }
    }
}
