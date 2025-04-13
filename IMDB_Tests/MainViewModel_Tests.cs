using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Windows.Controls;
using System;

namespace IMDB_Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        private MainViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new MainViewModel(_navigationService);
        }

        [TestMethod]
        public void InitialState_PropertiesAreInitialized()
        {
            Assert.IsNull(_viewModel.CurrentView);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.CloseCommand);
            Assert.IsNotNull(_viewModel.ShowHomeCommand);
            Assert.IsNotNull(_viewModel.ShowMoviesCommand);
            Assert.IsNotNull(_viewModel.ShowTVShowsCommand);
            Assert.IsNotNull(_viewModel.ShowActorsCommand);
            Assert.IsNotNull(_viewModel.ShowGenresCommand);
        }

        [TestMethod]
        public void CurrentView_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.CurrentView))
                    propertyChanged = true;
            };

            var newView = new UserControl();
            _viewModel.CurrentView = newView;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newView, _viewModel.CurrentView);
        }

        [TestMethod]
        public void ShowHomeCommand_NavigatesToHome()
        {
            _viewModel.ShowHomeCommand.Execute(null);
            Assert.AreEqual("HomeView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void ShowMoviesCommand_NavigatesToMovieDetails()
        {
            _viewModel.ShowMoviesCommand.Execute(null);
            Assert.AreEqual("MovieDetailsView", _navigationService.LastNavigatedView);
            Assert.AreEqual("movie", _navigationService.LastParameter);
        }

        [TestMethod]
        public void ShowTVShowsCommand_NavigatesToMovieDetails()
        {
            _viewModel.ShowTVShowsCommand.Execute(null);
            Assert.AreEqual("MovieDetailsView", _navigationService.LastNavigatedView);
            Assert.AreEqual("tvSeries", _navigationService.LastParameter);
        }

        [TestMethod]
        public void ShowActorsCommand_NavigatesToActors()
        {
            _viewModel.ShowActorsCommand.Execute(null);
            Assert.AreEqual("ActorsView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void ShowGenresCommand_NavigatesToGenres()
        {
            _viewModel.ShowGenresCommand.Execute(null);
            Assert.AreEqual("GenresView", _navigationService.LastNavigatedView);
        }

        //[TestMethod]
        //public void NavigationService_ViewChangeEvent_UpdatesCurrentView()
        //{
        //    var newView = new UserControl();
        //    _navigationService.CurrentViewChanged?.Invoke(this, newView);
        //    Assert.AreEqual(newView, _viewModel.CurrentView);
        //}
    }
}