using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private bool isLoading = false;
		public bool IsLoading
		{
			get { return isLoading; }
			set { isLoading = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading))); }
		}

		private PdfDocument pdfDocument;
		private ObservableCollection<OnePage> imgSource = null;
		public ObservableCollection<OnePage> ImgSource
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
			ImgSource = new ObservableCollection<OnePage>();
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
					for (uint i = 0; i < page;)
					{
						using (PdfPage unpairPage = pdfDocument.GetPage(i++))
						{
							if(i < page)
								using (PdfPage pairPage = pdfDocument.GetPage(i++))
								{
									var stream = new InMemoryRandomAccessStream();
									await unpairPage.RenderToStreamAsync(stream);
									var p = new OnePage();
									await p.ImageUnPair.SetSourceAsync(stream);
									await pairPage.RenderToStreamAsync(stream);
									await p.ImagePair.SetSourceAsync(stream);
									ImgSource.Add(p);
								}
							else
							{
								var stream = new InMemoryRandomAccessStream();
								await unpairPage.RenderToStreamAsync(stream);
								var p = new OnePage();
								await p.ImageUnPair.SetSourceAsync(stream);
								ImgSource.Add(p);
							}
						}
					}
				}
			}
			else
				return;
		}
	}
}
