using System.IO;
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

        private void OpenDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Wybierz folder";
            dialog.UseDescriptionForTitle = true;

            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                paths.Clear();
                string[] extensions = new string[] { ".png", ".gif", ".jpg", ".jpeg", ".bmp" };
                foreach( string extension in extensions )
                    Directory.GetFiles(dialog.SelectedPath, "*" + extension);

                if(paths.Count > 0 )
                {
                    rotation = 0;
                    displayedImage();
                }
                else
                {
                    displayedImageIndex = 0;
                    Image.Source = null;
                }
            }
        }

        private void ShowPrevious(object sender, RoutedEventArgs e)
        {
            if(displayedImageIndex > 0)
            {
                rotation = 0;
                displayedImage(displayedImageIndex - 1);
            }
        }

        private void ShowNext(object sender, RoutedEventArgs e)
        {
            if (displayedImageIndex < paths.Count - 1)
            {
                rotation = 0;
                displayedImage(displayedImageIndex + 1);
            }
        }
    }
}