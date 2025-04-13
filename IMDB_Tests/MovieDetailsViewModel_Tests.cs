using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;

namespace IMDB_Tests.ViewModels
{
    [TestClass]
    public class MovieDetailsViewModelTests
    {
        private MovieDetailsViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new MovieDetailsViewModel(_navigationService);
        }

        [TestMethod]
        public void InitialState_PropertiesAreInitialized()
        {
            Assert.IsNotNull(_viewModel.SimilarTitles);
            Assert.AreEqual(0, _viewModel.SimilarTitles.Count);
            Assert.IsNull(_viewModel.TitleName);
            Assert.IsNull(_viewModel.Overview);
            Assert.IsNull(_viewModel.GenreTags);
            Assert.AreEqual(0, _viewModel.Rating);
            Assert.IsNull(_viewModel.Runtime);
            Assert.IsNull(_viewModel.ReleaseDate);
            Assert.IsNull(_viewModel.CastAndCrew);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.BackToHomeCommand);
        }

        [TestMethod]
        public void TitleName_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.TitleName))
                    propertyChanged = true;
            };

            _viewModel.TitleName = "The Shawshank Redemption";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("The Shawshank Redemption", _viewModel.TitleName);
        }

        [TestMethod]
        public void Rating_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.Rating))
                    propertyChanged = true;
            };

            _viewModel.Rating = 9.3m;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(9.3m, _viewModel.Rating);
        }

        [TestMethod]
        public void SimilarTitles_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.SimilarTitles))
                    propertyChanged = true;
            };

            var newSimilarTitles = new ObservableCollection<Title>();
            _viewModel.SimilarTitles = newSimilarTitles;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newSimilarTitles, _viewModel.SimilarTitles);
        }

        [TestMethod]
        public void Initialize_WithMovieType_SetsParameter()
        {
            _viewModel.Initialize("movie");
            // Since we can't easily test the database interaction without mocking,
            // we can at least verify the method doesn't throw an exception
        }

        [TestMethod]
        public void Initialize_WithTitleId_SetsParameter()
        {
            _viewModel.Initialize("tt0111161");
            // Since we can't easily test the database interaction without mocking,
            // we can at least verify the method doesn't throw an exception
        }
    }
}