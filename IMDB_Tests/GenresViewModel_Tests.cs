using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;
using System.Windows.Controls;
using System;

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
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
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
    }
}