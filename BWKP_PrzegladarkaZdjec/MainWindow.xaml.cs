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
                    DisplayImage(0);
                }
                else
                {
                    displayedImageIndex = 0;
                    Image.Source = null;
                }
            }
        }

        private void DisplayImage(int i)
        {
            displayedImageIndex = i;
            Name.Content = paths[i];
            displayedImage = new BitmapImage();
            displayedImage.BeginInit();
            displayedImage.UriSource = new Uri(paths[i]);
            displayedImage.Rotation = rotation;
            displayedImage.EndInit();
            ResizeImage();
        }

        private void ShowPrevious(object sender, RoutedEventArgs e)
        {
            if(displayedImageIndex > 0)
            {
                rotation = 0;
                DisplayImage(displayedImageIndex - 1);
            }
        }

        private void ShowNext(object sender, RoutedEventArgs e)
        {
            if (displayedImageIndex < paths.Count - 1)
            {
                rotation = 0;
                DisplayImage(displayedImageIndex + 1);
            }
        }
        private void OriginalSize(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0) return;
            fitBtn.IsEnabled = true;
            originalBtn.IsEnabled = false;
            ResizeImage();
        }

       

        private void FitScreen(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0) return;
            fitBtn.IsEnabled = false;
            originalBtn.IsEnabled = true;
            ResizeImage();
        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            if(paths.Count == 0) return;
            rotation = (Rotation)((int)(rotation + 1) % 4);
            DisplayImage(displayedImageIndex);
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0) return;
            if ((int)Math.Round(size / 20.0) * 20 < 200)
            {
                fitBtn.IsEnabled = true;
                originalBtn.IsEnabled = true;
                size = (int)Math.Round(size / 20.0) * 20;
                size += 20;
                Image.Width = displayedImage.Width * size / 100;
                Image.Height = displayedImage.Height * size / 100;
                Size.Content = size.ToString() + "%";
            }
        }

        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0) return;
            if ((int)Math.Round(size / 20.0) * 20 > 20)
            {
                fitBtn.IsEnabled = true;
                originalBtn.IsEnabled = true;
                size = (int)Math.Round(size / 20.0) * 20;
                size -= 20;
                Image.Width = displayedImage.Width * size / 100;
                Image.Height = displayedImage.Height * size / 100;
                Size.Content = size.ToString() + "%";
            }
        }
    }
}