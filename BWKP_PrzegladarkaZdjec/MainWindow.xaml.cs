using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BWKP_PrzegladarkaZdjec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> paths = new List<string>();
        private BitmapImage displayedImage;
        private int displayedImageIndex = 0;
        private Rotation rotation = 0;
        private int size = 100;

        public MainWindow()
        {
            InitializeComponent();
        }


    }
}