using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace _10PDF.Logic
{
	public class PDF : INotifyPropertyChanged
	{

		private PdfDocument pdfDocument;
		private BitmapImage imgSource = null;
		public BitmapImage ImgSource
		{
			get { return imgSource; }
			set { imgSource = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImgSource))); }
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public PDF()
		{

		}

		public async void LoadFromFile()
		{
			pdfDocument = null;
			ImgSource = null;
			var picker = new Windows.Storage.Pickers.FileOpenPicker();
			picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
			picker.FileTypeFilter.Add(".pdf");
			Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
			if (file != null)
			{
				try
				{
					pdfDocument = await PdfDocument.LoadFromFileAsync(file);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					return;
				}
				if (pdfDocument != null)
				{
					uint page = pdfDocument.PageCount;
					using (PdfPage currentPage = pdfDocument.GetPage(0))
					{
						var stream = new InMemoryRandomAccessStream();
						await currentPage.RenderToStreamAsync(stream);
						ImgSource = new BitmapImage();
						await ImgSource.SetSourceAsync(stream);
					}
				}
			}
			else
				return;
		}
	}
}
