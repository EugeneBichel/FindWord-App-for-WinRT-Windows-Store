using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AnalystRevisionMap.Data.Domain;
using AnalystRevisionMap.UI.Common;
using AnalystRevisionMap.UI.Shared.Models;
using AnalystRevisionMap.UI.ViewModels;

namespace AnalystRevisionMap.UI.Views
{
    public sealed partial class Companies : LayoutAwarePage
    {
        #region Constants Fields

        private const int MinRectangleSize = 40000;
        private const int MaxRectangleSize = 80000;

        #endregion //Constants Fields

        #region Fields

        private CompaniesViewModel _model;
        private SectorModel _currentSector;
        private TextBlock _subTitleTextBlock;

        private bool _isLoadCompleted;

        #endregion //Fields

        #region Init page

        public Companies()
        {
            InitializeComponent();

            this.Loaded += CompaniesLoaded;
        }

        private void CompaniesLoaded(object sender, RoutedEventArgs e)
        {
            _isLoadCompleted = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _model = (CompaniesViewModel)this.DataContext;

            settingsControl.Source = _model.Settings;
        }

        #endregion //Init page

        #region Handlers for FillView

        private void FillViewLoaded(object sender, RoutedEventArgs e)
        {
            if (_model.Sectors != null)
            {
                FillView.ItemsSource = _model.Sectors;

                for (var i = 0; i < _model.Sectors.Count; i++)
                {
                    if (_model.Sectors[i].Equals(_model.SelectedSector) == true)
                        FillView.SelectedIndex = i;
                }
            }
        }

        private void FillViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sector = FillView.SelectedItem as SectorModel;
            if (sector != null)
                _currentSector = sector;
        }

        private void SubTitleLoaded(object sender, RoutedEventArgs e)
        {
            _subTitleTextBlock = (TextBlock)sender;
        }

        #region Company GridView Handlers

        private void CompanyGridViewLoaded(object sender, RoutedEventArgs e)
        {
            var companyGridView = sender as GridView;
            if (companyGridView == null)
                return;

            _currentSector = ((FrameworkElement)sender).DataContext as SectorModel;

            if (_currentSector == null)
                return;

            _currentSector.Companies = AdjustCompaniesForUI(_currentSector.Companies);
            companyGridView.ItemsSource = _currentSector.Companies;

            if (_model.Settings.DisplayType == DisplayType.Ratings)
                companyGridView.ItemTemplate = App.Current.Resources["RatingCompanyTileGridViewItemTemplate"] as DataTemplate;
            else
                companyGridView.ItemTemplate = App.Current.Resources["EstimatesCompanyTileGridViewItemTemplate"] as DataTemplate;
        }

        private void CompanyGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            ShowCompanyHistory(e.ClickedItem);
        }

        #endregion //Company GridView Handlers

        #endregion //Hanlders for FillView

        #region Handlers for SnappedView

        private void SnappedViewLoaded(object sender, RoutedEventArgs e)
        {
            ListView lstView = (ListView)sender;

            SectorModel currentSector = ((FrameworkElement)sender).DataContext as SectorModel;

            if (_model.DisplayMode.DisplayType == DisplayType.Ratings)
                lstView.Style = (Style)App.Current.Resources["CompanyRatingSnappedListView"];
            else
                lstView.Style = (Style)App.Current.Resources["CompanyEstimateSnappedListView"];

            lstView.ItemsSource = currentSector.Companies;
            lstView.IsItemClickEnabled = true;
            lstView.ItemClick += SnappedItemClick;
        }

        private void SnappedItemClick(object sender, ItemClickEventArgs e)
        {
            ShowCompanyHistory(e.ClickedItem);
        }

        #endregion //Handlers for SnappedView

        #region AppBar's Elements Handlers And Help Methods

        private void SettingsChanged()
        {
            if (!_isLoadCompleted)
                return;

            _model.Settings = settingsControl.Source;

            _model.RefreshView();
        }

        #endregion //AppBar's Elements Handlers And Help Methods

        #region Private Helpers

        private CompanyCollection AdjustCompaniesForUI(CompanyCollection companies)
        {
            return CompanyUIHelper.AdjustCompaniesForUI(companies, LayoutRoot.ActualWidth, LayoutRoot.ActualHeight);
        }

        private void ShowCompanyHistory(object source)
        {
            Company company;
            FrameworkElement element = source as FrameworkElement;
            if (element != null)
                company = element.DataContext as Company;
            else
                company = source as Company;

            if (company != null)
                _model.ShowCompanyChangeHistory(company);
        }

        #endregion //Private Helpers
    }
}