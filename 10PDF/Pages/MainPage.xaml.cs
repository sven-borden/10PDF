using _10PDF.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

		public MainPage()
        {
            this.InitializeComponent();
			TransparentBar();
			applyAcrylicAccent(MainGrid);
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
