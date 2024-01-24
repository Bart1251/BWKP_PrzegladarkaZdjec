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

        private void ResizeImage()
        {
            if (!originalBtn.IsEnabled)
                OriginalFit();
            if (!fitBtn.IsEnabled)
                ScreenFit();
        }

        private void OriginalFit()
        {
            originalBtn.IsEnabled = false;
            fitBtn.IsEnabled = true;
            Image.Width = displayedImage.Width;
            Image.Height = displayedImage.Height;
        }

        private void ScreenFit()
        {
            originalBtn.IsEnabled = true;
            fitBtn.IsEnabled = false;

            if (displayedImage.Width > ImageGrid.ActualWidth)
            {
                Image.Width = ImageGrid.ActualWidth - 60;
                Image.Height = displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width;
                size = (int)((ImageGrid.ActualWidth - 60) / displayedImage.Width * 100);
                if (displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width > ImageGrid.Height)
                {
                    Image.Height = ImageGrid.ActualHeight;
                    Image.Width = displayedImage.Width * ImageGrid.ActualHeight / displayedImage.Height;
                    size = (int)(ImageGrid.ActualHeight / displayedImage.Height * 100);
                }
            }
            else if (displayedImage.Height > ImageGrid.ActualHeight)
            {
                Image.Height = ImageGrid.ActualHeight;
                Image.Width = displayedImage.Width * ImageGrid.ActualHeight / displayedImage.Height;
                size = (int)(ImageGrid.ActualHeight / displayedImage.Height * 100);
                if (displayedImage.Width * ImageGrid.ActualHeight / displayedImage.Height > ImageGrid.Width)
                {
                    Image.Width = ImageGrid.ActualWidth;
                    Image.Height = displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width;
                    size = (int)((ImageGrid.ActualWidth - 60) / displayedImage.Width * 100);
                }
            }
            else
            {
                Image.Width = ImageGrid.ActualWidth - 60;
                Image.Height = displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width;
                size = (int)((ImageGrid.ActualWidth - 60) / displayedImage.Width * 100);
                if (displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width > ImageGrid.Height)
                {
                    Image.Height = ImageGrid.ActualHeight;
                    Image.Width = displayedImage.Width * ImageGrid.ActualHeight / displayedImage.Height;
                    size = (int)(ImageGrid.ActualHeight / displayedImage.Height * 100);
                }
            }
        }
    }
}