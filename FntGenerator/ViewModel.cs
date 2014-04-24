using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FntGenerator
{
    class ViewModel : INotifyPropertyChanged
    {
        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        private int _characterWidth;
        public int CharacterWidth
        {
            get { return _characterWidth; }
            set
            {
                _characterWidth = value;
                OnPropertyChanged("CharacterWidth");
            }
        }

        private int _characterHeight;
        public int CharacterHeight
        {
            get { return _characterHeight; }
            set
            {
                _characterHeight = value;
                OnPropertyChanged("CharacterHeight");
            }
        }

        private string _characterString;
        public string CharacterString
        {
            get { return _characterString; }
            set
            {
                _characterString = value;
                OnPropertyChanged("CharacterString");
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GenerateFnt()
        {
            // Load file
            Bitmap img = new Bitmap(ImagePath);
            FileInfo file = new FileInfo(ImagePath);

            // Determine cols
            int cols = img.Width / CharacterWidth;
            int rows = img.Height / CharacterHeight;

            // Create character array
            char[] characters = CharacterString.ToCharArray();

            List<string> result = new List<string>();

            // Dump Common line
            result.Add(String.Format("common lineHeight={0} base={1} scaleW={2} scaleH={3} pages=1 packed=0 alphaChnl=1 redChnl=1 greenChnl=1 blueChnl=1",
                Math.Ceiling(CharacterHeight * 1.5),
                CharacterWidth,
                img.Width,
                img.Height));

            // Dump Page line
            result.Add(String.Format("page id=0 file=\"{0}\"", file.Name));

            // Dump Chars line
            result.Add(String.Format("chars count={0}", characters.Length));

            // Dump each Char
            for (int i = 0; i < characters.Length; i++)
            {
                result.Add(String.Format("char id={0} x={1} y={2} width={3} height={4} xoffset=0 yoffset=0 xadvance={5} page=0 chnl=15",
                    (int)characters[i],
                    (i % cols) * CharacterWidth,
                    (i / cols) * CharacterHeight,
                    CharacterWidth,
                    CharacterHeight,
                    CharacterWidth));
            }

            return String.Join("\n", result);
        }
    }
}
