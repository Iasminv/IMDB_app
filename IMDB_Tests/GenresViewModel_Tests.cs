using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;

namespace IMDB_Tests.ViewModels
{
    [TestClass]
    public class GenresViewModelTests
    {
        private GenresViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new GenresViewModel(_navigationService);
        }

        [TestMethod]
        public void InitialState_PropertiesAreInitialized()
        {
            Assert.IsNotNull(_viewModel.Genres);
            Assert.AreEqual(0, _viewModel.Genres.Count);
            Assert.IsNotNull(_viewModel.BackToHomeCommand);
            Assert.IsNotNull(_viewModel.SelectGenreCommand);
        }

        [TestMethod]
        public void Genres_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GenresViewModel.Genres))
                    propertyChanged = true;
            };

            var newGenres = new ObservableCollection<Genre>();
            _viewModel.Genres = newGenres;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newGenres, _viewModel.Genres);
        }

        [TestMethod]
        public void SelectGenreCommand_WithNullGenre_DoesNotNavigate()
        {
            _viewModel.SelectGenreCommand.Execute(null);
            Assert.IsNull(_navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void SelectGenreCommand_WithValidGenre_NavigatesToMovieList()
        {
            var genre = new Genre { GenreId = 1, Name = "Action" };
            _viewModel.SelectGenreCommand.Execute(genre);

            Assert.AreEqual("MovieListView", _navigationService.LastNavigatedView);
            Assert.AreEqual(1, _navigationService.LastParameter);
        }

        [TestMethod]
        public void BackToHomeCommand_NavigatesToHome()
        {
            _viewModel.BackToHomeCommand.Execute(null);
            Assert.AreEqual("HomeView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void GenresCollection_CanBeModified()
        {
            // Test adding to collection
            var genre = new Genre { GenreId = 1, Name = "Action" };
            _viewModel.Genres.Add(genre);
            Assert.AreEqual(1, _viewModel.Genres.Count);

            // Test removing from collection
            _viewModel.Genres.Remove(genre);
            Assert.AreEqual(0, _viewModel.Genres.Count);
        }

        [TestMethod]
        public void GenresCollection_CanBeCleared()
        {
            // Add some genres
            _viewModel.Genres.Add(new Genre { GenreId = 1, Name = "Action" });
            _viewModel.Genres.Add(new Genre { GenreId = 2, Name = "Comedy" });

            // Clear the collection
            _viewModel.Genres.Clear();
            Assert.AreEqual(0, _viewModel.Genres.Count);
        }
    }
}