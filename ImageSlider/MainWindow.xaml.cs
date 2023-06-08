using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace ImageSlider
{
    public partial class MainWindow : Window
    {
        private bool isDragging;
        private Point lastMousePosition;
        private System.Windows.Controls.Image selectedImage;

        public MainWindow()
        {
            InitializeComponent();

            LoadAndDisplayImages();
            ResizeAndPositionImages();

            // Subscribe to mouse events
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
        }

        private void LoadAndDisplayImages()
        {
            // Define the URIs for the image files
            Uri imageUri1 = new Uri("C:\\Users\\user\\source\\repos\\ImageSlider\\ImageSlider\\mouse.jpg", UriKind.Absolute);
            Uri imageUri2 = new Uri("C:\\Users\\user\\source\\repos\\ImageSlider\\ImageSlider\\dog.jpg", UriKind.Absolute);
            Uri imageUri3 = new Uri("C:\\Users\\user\\source\\repos\\ImageSlider\\ImageSlider\\cat.jpg", UriKind.Absolute);

            // Create Image controls and set their sources to the image URIs
            image1 = new System.Windows.Controls.Image();
            image1.Source = new BitmapImage(imageUri1);
            canvas.Children.Add(image1);

            image2 = new System.Windows.Controls.Image();
            image2.Source = new BitmapImage(imageUri2);
            canvas.Children.Add(image2);

            image3 = new System.Windows.Controls.Image();
            image3.Source = new BitmapImage(imageUri3);
            canvas.Children.Add(image3);
        }

        private void ResizeAndPositionImages()
        {
            // Calculate the total width required for the images
            double totalWidth = image1.Source.Width + image2.Source.Width + image3.Source.Width;

            // Adjust the size of the canvas to accommodate the images
            canvas.Width = totalWidth * 0.3;
            canvas.Height = Math.Max(image1.Source.Height, Math.Max(image2.Source.Height, image3.Source.Height)) * 0.3;

            // Calculate the scaling factor based on the canvas width and the total width of the images
            double scaleFactor = canvas.Width / totalWidth;

            // Set the size and position of the first image
            image1.Width = image1.Source.Width * scaleFactor;
            image1.Height = image1.Source.Height * scaleFactor;
            Canvas.SetLeft(image1, 0);
            Canvas.SetTop(image1, (canvas.Height - image1.Height) / 2);

            // Set the size and position of the second image
            image2.Width = image2.Source.Width * scaleFactor;
            image2.Height = image2.Source.Height * scaleFactor;
            Canvas.SetLeft(image2, image1.Width);
            Canvas.SetTop(image2, (canvas.Height - image2.Height) / 2);

            // Set the size and position of the third image
            image3.Width = image3.Source.Width * scaleFactor;
            image3.Height = image3.Source.Height * scaleFactor;
            Canvas.SetLeft(image3, image1.Width + image2.Width);
            Canvas.SetTop(image3, (canvas.Height - image3.Height) / 2);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Store the current mouse position and mark dragging as true
            lastMousePosition = e.GetPosition(canvas);
            isDragging = true;

            // Determine which image was clicked based on the mouse position
            if (lastMousePosition.X < Canvas.GetLeft(image1) + image1.ActualWidth)
                selectedImage = image1;
            else if (lastMousePosition.X < Canvas.GetLeft(image2) + image2.ActualWidth)
                selectedImage = image2;
            else
                selectedImage = image3;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedImage != null)
            {
                // Get the current mouse position and calculate the horizontal distance moved
                Point currentMousePosition = e.GetPosition(canvas);
                double deltaX = currentMousePosition.X - lastMousePosition.X;

                // Update the positions of all images simultaneously by the deltaX
                Canvas.SetLeft(image1, Canvas.GetLeft(image1) + deltaX);
                Canvas.SetLeft(image2, Canvas.GetLeft(image2) + deltaX);
                Canvas.SetLeft(image3, Canvas.GetLeft(image3) + deltaX);

                // Update the last mouse position
                lastMousePosition = currentMousePosition;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Mark dragging as false and clear the selected image
            isDragging = false;
            selectedImage = null;
        }
    }
}
