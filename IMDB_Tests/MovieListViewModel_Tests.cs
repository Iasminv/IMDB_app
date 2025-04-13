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
    public class MovieListViewModelTests
    {
        private MovieListViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new MovieListViewModel(_navigationService);
        }

        [TestMethod]
        public void InitialState_PropertiesAreInitialized()
        {
            Assert.IsNotNull(_viewModel.Titles);
            Assert.AreEqual(0, _viewModel.Titles.Count);
            Assert.IsNull(_viewModel.GenreName);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.BackToGenresCommand);
            Assert.IsNotNull(_viewModel.SelectTitleCommand);
        }

        [TestMethod]
        public void Titles_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieListViewModel.Titles))
                    propertyChanged = true;
            };

            var newTitles = new ObservableCollection<Title>();
            _viewModel.Titles = newTitles;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newTitles, _viewModel.Titles);
        }

        [TestMethod]
        public void GenreName_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieListViewModel.GenreName))
                    propertyChanged = true;
            };

            _viewModel.GenreName = "Action";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Action", _viewModel.GenreName);
        }

        [TestMethod]
        public void SelectTitleCommand_WithNullTitle_DoesNotNavigate()
        {
            _viewModel.SelectTitleCommand.Execute(null);
            Assert.IsNull(_navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void SelectTitleCommand_WithValidTitle_NavigatesToMovieDetails()
        {
            var title = new Title { TitleId = "tt0111161", PrimaryTitle = "The Shawshank Redemption" };
            _viewModel.SelectTitleCommand.Execute(title);

            Assert.AreEqual("MovieDetailsView", _navigationService.LastNavigatedView);
            Assert.AreEqual("tt0111161", _navigationService.LastParameter);
        }

        [TestMethod]
        public void BackToGenresCommand_NavigatesToGenres()
        {
            _viewModel.BackToGenresCommand.Execute(null);
            Assert.AreEqual("GenresView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void Initialize_WithValidGenreId_DoesNotThrow()
        {
            try
            {
                _viewModel.Initialize(1);
                // Since we can't easily test database operations, we just verify no exception is thrown
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Initialize threw an exception");
            }
        }
    }
}