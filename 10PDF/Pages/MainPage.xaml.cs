using _10PDF.Logic;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _10PDF
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
    {
		
		public PDF PDF = new PDF();
		public string EnterPath = string.Empty;

		public MainPage()
        {
            this.InitializeComponent();
			TransparentBar();
			applyAcrylicAccent(MainGrid);
			DispatcherTimer t = new DispatcherTimer() { Interval = new System.TimeSpan(0, 0, 1) };
			t.Tick += (o, f) =>
			{
				if (EnterPath != string.Empty)
				{
					PDF.EnterPDF(EnterPath);
					EnterPath = string.Empty;
				}
				t.Stop();
			};
			t.Start();
        }


		#region BackDrop
		private void TransparentBar()
		{
			ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
			formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
			CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
		}

		private void applyAcrylicAccent(Panel e)
		{
			_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
			_hostSprite = _compositor.CreateSpriteVisual();
			_hostSprite.Size = new Vector2((float)MainGrid.ActualWidth, (float)MainGrid.ActualHeight);

			ElementCompositionPreview.SetElementChildVisual(
					MainGrid, _hostSprite);
			_hostSprite.Brush = _compositor.CreateHostBackdropBrush();
		}
		Compositor _compositor;
		SpriteVisual _hostSprite;

		private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (_hostSprite != null)
				_hostSprite.Size = e.NewSize.ToVector2();
		}

		#endregion

		private void AppBarButton_Click(object sender, RoutedEventArgs e)
		{
			PDF.LoadFromFile();
		}
	}
}
