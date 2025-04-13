using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;
using System;
using System.Windows.Controls;

namespace IMDB_Tests.ViewModels
{
    [TestClass]
    public class HomeViewModelTests
    {
        private HomeViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new HomeViewModel(_navigationService);
        }

        [TestMethod]
        public void InitialState_PropertiesAreInitialized()
        {
            Assert.IsNotNull(_viewModel.SearchResults);
            Assert.AreEqual(0, _viewModel.SearchResults.Count);
            Assert.IsFalse(_viewModel.IsLoading);
            Assert.IsNull(_viewModel.SearchText);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.SearchCommand);
            Assert.IsNotNull(_viewModel.NavigateToMoviesCommand);
            Assert.IsNotNull(_viewModel.NavigateToTVShowsCommand);
            Assert.IsNotNull(_viewModel.NavigateToActorsCommand);
            Assert.IsNotNull(_viewModel.NavigateToGenresCommand);
            Assert.IsNotNull(_viewModel.SelectTitleCommand);
        }

        [TestMethod]
        public void SearchText_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(HomeViewModel.SearchText))
                    propertyChanged = true;
            };

            _viewModel.SearchText = "Test";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Test", _viewModel.SearchText);
        }

        [TestMethod]
        public void IsLoading_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(HomeViewModel.IsLoading))
                    propertyChanged = true;
            };

            _viewModel.IsLoading = true;

            Assert.IsTrue(propertyChanged);
            Assert.IsTrue(_viewModel.IsLoading);
        }

        [TestMethod]
        public void SearchResults_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(HomeViewModel.SearchResults))
                    propertyChanged = true;
            };

            var newResults = new ObservableCollection<Title>();
            _viewModel.SearchResults = newResults;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newResults, _viewModel.SearchResults);
        }
    }

    // Test double for INavigationService
    public class TestNavigationService : INavigationService
    {
        public event EventHandler<UserControl> CurrentViewChanged;

        public string LastNavigatedView { get; private set; }
        public object LastNavigatedViewModel { get; private set; }
        public object LastParameter { get; private set; }

        public void NavigateTo(UserControl view)
        {
            CurrentViewChanged?.Invoke(this, view);
        }

        public void NavigateToWithViewModel<TView, TViewModel>(object parameter = null)
            where TView : UserControl
            where TViewModel : class
        {
            LastNavigatedView = typeof(TView).Name;
            LastNavigatedViewModel = typeof(TViewModel).Name;
            LastParameter = parameter;

            // Since we're in a test environment, we don't actually create the view
            CurrentViewChanged?.Invoke(this, null);
        }
    }
}