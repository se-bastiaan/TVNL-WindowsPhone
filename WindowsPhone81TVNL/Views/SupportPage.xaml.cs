using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPhone81TVNL.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhone81TVNL.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SupportPage : Page
    {
        public SupportPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private async void DoPurchase(string productId)
        {
            try
            {
                PurchaseResults purchaseResults = await CurrentApp.RequestProductPurchaseAsync(productId);
                Guid transactionId = purchaseResults.TransactionId;

                switch (purchaseResults.Status)
                {
                    case ProductPurchaseStatus.Succeeded:
                        await CurrentApp.ReportConsumableFulfillmentAsync(productId, transactionId);
                        await MessageBox.Show("Je bijdrage is ontvangen.", "Bedankt!");
                        break;

                    case ProductPurchaseStatus.NotFulfilled:
                        await CurrentApp.ReportConsumableFulfillmentAsync(productId, transactionId);
                        await MessageBox.Show("Je bijdrage is ontvangen.", "Bedankt!");
                        break;
                }
            }
            catch (COMException ex) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LikeButtonClick(object sender, RoutedEventArgs e)
        {
            DoPurchase("small_donate");
        }

        private void FavoriteButtonClick(object sender, RoutedEventArgs e)
        {
            DoPurchase("medium_donate");
        }

        private void LoveButtonClick(object sender, RoutedEventArgs e)
        {
            DoPurchase("big_donate");
        }
    }
}
