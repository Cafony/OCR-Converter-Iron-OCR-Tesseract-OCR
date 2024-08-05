using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronOcr;
using IronOcr.Languages;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using Tesseract;

namespace OCR_Toze
{
    public partial class Form1 : Form
    {
        private Bitmap _imagem;
        private string _path;
        private Bitmap currentImage;
        private Bitmap originalImage;

        public Form1()
        {
            InitializeComponent();
            panel3.Visible = false;
            panel4.Visible = true;
        }
        private void buttonOpenImage_Click(object sender, EventArgs e)
        {
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the filter for file types
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                // Show the dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _path = openFileDialog.FileName;
                    originalImage = new Bitmap(openFileDialog.FileName);
                    currentImage = originalImage;
                    // Load the selected image into the PictureBox
                    pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox3.Image = originalImage;
                }
            }
        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {

        }

        private void buttonIronOCR_Click(object sender, EventArgs e)
        {
            try
            {
            var ocr = new AutoOcr();           
            var texto = ocr.Read(originalImage);
            richTextBox1.Text = texto.ToString();
            }
            catch 
            {
                return;
            }

        }
    
        private void buttonTesserac_Click(object sender, EventArgs e)
        {

            // Load the OCR engine
            using (var ocr = OcrApi.Create())
            {
                // Specify the language you want to use
                ocr.Init(Languages.Portuguese);

                // Load the image
                using (var image = new Bitmap(_path))
                {
                    // Perform OCR on the image
                    string text = ocr.GetTextFromImage(image);
                    richTextBox1.Text = text;
                }
            }
        }

        private void buttonBlack_Click(object sender, EventArgs e)
        {
            // Convert to Black and White
            currentImage = (Bitmap)originalImage.Clone();
            for (int y = 0; y < currentImage.Height; y++)
            {
                for (int x = 0; x < currentImage.Width; x++)
                {
                    Color pixelColor = currentImage.GetPixel(x, y);
                    Color newColor = pixelColor.GetBrightness() > 0.5 ? Color.White : Color.Black;
                    currentImage.SetPixel(x, y, newColor);
                }
            }

            pictureBox3.Image = currentImage;
        }

        private void buttonGray_Click(object sender, EventArgs e)
        {
            // Convert to Grayscale
            currentImage = (Bitmap)originalImage.Clone();
            for (int y = 0; y < currentImage.Height; y++)
            {
                for (int x = 0; x < currentImage.Width; x++)
                {
                    Color pixelColor = currentImage.GetPixel(x, y);
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    currentImage.SetPixel(x, y, newColor);
                }
            }
            pictureBox3.Image = currentImage;
        }

        private void buttonOriginal_Click(object sender, EventArgs e)
        {
            // Reset to Initial Image
            currentImage = (Bitmap)originalImage.Clone();
            pictureBox3.Image = currentImage;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            // Convert back to Color (Original Image)
            currentImage = (Bitmap)originalImage.Clone();
            pictureBox3.Image = currentImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
          

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private string TesseractOCR(string imagePath)
        {
            string resultText = string.Empty;
            try
            {
                using (var _engine = new TesseractEngine(@"C:\Users\tozec\source\repos\OCR Toze\OCR Toze\tessdata\", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = _engine.Process(img))
                        {
                            resultText = page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during OCR: " + ex.Message);
            }
            return resultText;
        }

        private void buttonTesseract2_Click(object sender, EventArgs e)
        {
            try
            {
                string _text = TesseractOCR(_path);
                richTextBox1.Text = _text;
            }
            catch { return; }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void buttonClearImage_Click(object sender, EventArgs e)
        {
            if(originalImage!=null)
            {
                originalImage.Dispose();
                currentImage.Dispose();
                pictureBox3.Image = null;
            }
            else { return; }

        }
    }

}
