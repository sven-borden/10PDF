using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;

namespace _10PDF.Logic
{
	public class OnePage : INotifyPropertyChanged
	{
		private BitmapImage imagePair = new BitmapImage();
		public BitmapImage ImagePair
		{
			get { return imagePair; }
			set { imagePair = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePair))); }
		}
		private BitmapImage imageUnPair = new BitmapImage();
		public BitmapImage ImageUnPair
		{
			get { return imageUnPair; }
			set { imageUnPair = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageUnPair))); }
		}

		public OnePage()
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
