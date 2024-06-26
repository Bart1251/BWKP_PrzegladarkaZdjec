﻿using System.IO;
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
using MessageBox = System.Windows.MessageBox;

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
                foreach (string extension in extensions)
                    paths.AddRange(Directory.GetFiles(dialog.SelectedPath, "*" + extension));
                

                if (paths.Count > 0 )
                {
                    fitBtn.IsEnabled = false;
                    originalBtn.IsEnabled = true;
                    rotation = 0;
                    DisplayImage(0);
                }
                else
                {
                    displayedImageIndex = 0;
                    Name.Content = "";
                    Size.Content = "100%";
                    Image.Source = null;
                }
            }
        }
        private void Delete(object sender, EventArgs e)
        {
            if(paths.Count == 0) return;
            if (MessageBox.Show("Czy na pewno chcesz usunąć ten plik?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            string pathToRemove = paths[displayedImageIndex];
            paths.RemoveAt(displayedImageIndex);
            if (paths.Count == displayedImageIndex)
                displayedImageIndex--;
            if (paths.Count > 0)
            {
                fitBtn.IsEnabled = false;
                originalBtn.IsEnabled = true;
                rotation = 0;
                DisplayImage(displayedImageIndex);
            }
            else
            {
                displayedImageIndex = 0;
                Image.Source = null;
            }
            File.Delete(pathToRemove);
        }

        private void DisplayImage(int i)
        {
            displayedImageIndex = i;
            Name.Content = paths[i].Split("\\").Last();
            displayedImage = new BitmapImage();
            displayedImage.BeginInit();
            displayedImage.CacheOption = BitmapCacheOption.OnLoad;
            displayedImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            displayedImage.UriSource = new Uri(paths[i]);
            displayedImage.Rotation = rotation;
            displayedImage.EndInit();
            Image.Source = displayedImage;
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

        private void WindowResize(object sender, RoutedEventArgs e)
        {
            if (paths.Count == 0) return;
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

        private void ResizeImage()
        {
            if (!originalBtn.IsEnabled)
                OriginalFit();
            if (!fitBtn.IsEnabled)
                ScreenFit();
            Size.Content = size.ToString() + "%";
        }

        private void OriginalFit()
        {
            originalBtn.IsEnabled = false;
            fitBtn.IsEnabled = true;
            Image.Width = displayedImage.Width;
            Image.Height = displayedImage.Height;
            size = 100;
        }

        private void ScreenFit()
        {
            originalBtn.IsEnabled = true;
            fitBtn.IsEnabled = false;

            if (displayedImage.Width > ImageGrid.ActualWidth - 60)
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
                if (displayedImage.Height * (ImageGrid.ActualWidth - 60) / displayedImage.Width > ImageGrid.ActualHeight)
                {
                    Image.Height = ImageGrid.ActualHeight;
                    Image.Width = displayedImage.Width * ImageGrid.ActualHeight / displayedImage.Height;
                    size = (int)(ImageGrid.ActualHeight / displayedImage.Height * 100);
                }
            }
        }
    }
}