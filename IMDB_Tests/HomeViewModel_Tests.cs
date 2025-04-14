using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;

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
            Assert.IsFalse(_viewModel.IsSearched);
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

        [TestMethod]
        public void IsSearched_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(HomeViewModel.IsSearched))
                    propertyChanged = true;
            };

            _viewModel.IsSearched = true;

            Assert.IsTrue(propertyChanged);
            Assert.IsTrue(_viewModel.IsSearched);
        }

        [TestMethod]
        public void SearchText_ShortInput_ClearsResults()
        {
            // Arrange
            _viewModel.SearchResults.Add(new Title { PrimaryTitle = "Test" });

            // Act
            _viewModel.SearchText = "a";  // Too short to trigger search

            // Assert
            Assert.AreEqual(0, _viewModel.SearchResults.Count);
            Assert.IsFalse(_viewModel.IsSearched);
        }

        [TestMethod]
        public void SearchText_EmptyInput_ClearsResults()
        {
            // Arrange
            _viewModel.SearchResults.Add(new Title { PrimaryTitle = "Test" });

            // Act
            _viewModel.SearchText = "";

            // Assert
            Assert.AreEqual(0, _viewModel.SearchResults.Count);
            Assert.IsFalse(_viewModel.IsSearched);
        }

        [TestMethod]
        public void NavigateToMovies_UsesCorrectParameter()
        {
            // Act
            _viewModel.NavigateToMoviesCommand.Execute(null);

            // Assert
            Assert.AreEqual("MovieListView", _navigationService.LastNavigatedView);
            Assert.AreEqual("movie", _navigationService.LastParameter);
        }

        [TestMethod]
        public void NavigateToTVShows_UsesCorrectParameter()
        {
            // Act
            _viewModel.NavigateToTVShowsCommand.Execute(null);

            // Assert
            Assert.AreEqual("MovieListView", _navigationService.LastNavigatedView);
            Assert.AreEqual("tvSeries", _navigationService.LastParameter);
        }

        [TestMethod]
        public void SelectTitle_NavigatesToMovieDetails()
        {
            // Arrange
            var title = new Title { TitleId = "tt0111161", PrimaryTitle = "The Shawshank Redemption" };

            // Act
            _viewModel.SelectTitleCommand.Execute(title);

            // Assert
            Assert.AreEqual("MovieDetailsView", _navigationService.LastNavigatedView);
            Assert.AreEqual("tt0111161", _navigationService.LastParameter);
        }

        [TestMethod]
        public void NavigateToActors_NavigatesToActorsView()
        {
            // Act
            _viewModel.NavigateToActorsCommand.Execute(null);

            // Assert
            Assert.AreEqual("ActorsView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void NavigateToGenres_NavigatesToGenresView()
        {
            // Act
            _viewModel.NavigateToGenresCommand.Execute(null);

            // Assert
            Assert.AreEqual("GenresView", _navigationService.LastNavigatedView);
        }
    }
}