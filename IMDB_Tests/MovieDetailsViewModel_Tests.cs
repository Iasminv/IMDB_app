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
        public void Overview_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.Overview))
                    propertyChanged = true;
            };

            _viewModel.Overview = "Test Overview";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Test Overview", _viewModel.Overview);
        }

        [TestMethod]
        public void GenreTags_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.GenreTags))
                    propertyChanged = true;
            };

            _viewModel.GenreTags = "Drama, Crime";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Drama, Crime", _viewModel.GenreTags);
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
        public void Runtime_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.Runtime))
                    propertyChanged = true;
            };

            _viewModel.Runtime = "142 minutes";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("142 minutes", _viewModel.Runtime);
        }

        [TestMethod]
        public void ReleaseDate_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.ReleaseDate))
                    propertyChanged = true;
            };

            _viewModel.ReleaseDate = "1994";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("1994", _viewModel.ReleaseDate);
        }

        [TestMethod]
        public void CastAndCrew_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieDetailsViewModel.CastAndCrew))
                    propertyChanged = true;
            };

            _viewModel.CastAndCrew = "Tim Robbins, Morgan Freeman";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Tim Robbins, Morgan Freeman", _viewModel.CastAndCrew);
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
        public void Initialize_WithMovieType_DoesNotThrow()
        {
            try
            {
                _viewModel.Initialize("movie");
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Initialize with movie type threw an exception");
            }
        }

        [TestMethod]
        public void Initialize_WithTVSeriesType_DoesNotThrow()
        {
            try
            {
                _viewModel.Initialize("tvSeries");
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Initialize with TV series type threw an exception");
            }
        }

        [TestMethod]
        public void Initialize_WithTitleId_DoesNotThrow()
        {
            try
            {
                _viewModel.Initialize("tt0111161");
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Initialize with title ID threw an exception");
            }
        }

        [TestMethod]
        public void BackToHomeCommand_NavigatesToHome()
        {
            _viewModel.BackToHomeCommand.Execute(null);
            Assert.AreEqual("HomeView", _navigationService.LastNavigatedView);
        }
    }
}