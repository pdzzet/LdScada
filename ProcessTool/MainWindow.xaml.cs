using System.Windows;

namespace ProcessTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private  readonly  ProcessViewModel vm=null;
        public MainWindow()
        {
            InitializeComponent();
            if (vm==null)
            {
                vm = new ProcessViewModel();
            }
            this.DataContext = vm;
        }
    }
}
