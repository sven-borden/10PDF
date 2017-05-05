using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace _10PDF.Logic
{
	public class PDF : INotifyPropertyChanged
	{
		private uint maxPage = 1;
		public uint MaxPage
		{
			get { return maxPage; }
			set { maxPage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxPage))); }
		}

		private uint currentPage = 1;
		public uint CurrentPage
		{
			get { return currentPage; }
			set { currentPage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage))); }
		}

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


		public async void EnterPDF(string enterPath)
		{
			StorageFile file = await StorageFile.GetFileFromPathAsync(enterPath);
			LoadFromFile(file);
		}

		public async void LoadFromFile(StorageFile _file = null)
		{
			pdfDocument = null;
			ImgSource = new ObservableCollection<OnePage>();
			if (_file == null)
			{
				var picker = new Windows.Storage.Pickers.FileOpenPicker();
				picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
				picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
				picker.FileTypeFilter.Add(".pdf");
				_file = await picker.PickSingleFileAsync();
			}
			if (_file != null)
			{
				IsLoading = true;
				try
				{
					pdfDocument = await PdfDocument.LoadFromFileAsync(_file);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					return;
				}
				if (pdfDocument != null)
				{
					MaxPage = pdfDocument.PageCount;
					for (uint i = 0; i < MaxPage;)
					{
						using (PdfPage unpairPage = pdfDocument.GetPage(i++))
						{
							CurrentPage = i;
							if(i < MaxPage)
								using (PdfPage pairPage = pdfDocument.GetPage(i++))
								{
									var stream = new InMemoryRandomAccessStream();
									await unpairPage.RenderToStreamAsync(stream);
									var p = new OnePage();
									await p.ImageUnPair.SetSourceAsync(stream);
									CurrentPage = i;
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
				IsLoading = false;
			}
			else
				return;
		}
	}
}
