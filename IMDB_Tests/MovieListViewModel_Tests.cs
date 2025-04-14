using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;

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
            Assert.IsNull(_viewModel.ListTitle);
            Assert.IsNull(_viewModel.SearchText);
            Assert.IsFalse(_viewModel.IsLoading);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.BackCommand);
            Assert.IsNotNull(_viewModel.SelectTitleCommand);
            Assert.IsNotNull(_viewModel.SearchCommand);
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
        public void ListTitle_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieListViewModel.ListTitle))
                    propertyChanged = true;
            };

            _viewModel.ListTitle = "Action Movies";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Action Movies", _viewModel.ListTitle);
        }

        [TestMethod]
        public void SearchText_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MovieListViewModel.SearchText))
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
                if (e.PropertyName == nameof(MovieListViewModel.IsLoading))
                    propertyChanged = true;
            };

            _viewModel.IsLoading = true;

            Assert.IsTrue(propertyChanged);
            Assert.IsTrue(_viewModel.IsLoading);
        }

        [TestMethod]
        public void Initialize_WithMovieType_SetsCorrectListTitle()
        {
            _viewModel.Initialize("movie");
            Assert.AreEqual("Movies", _viewModel.ListTitle);
        }

        [TestMethod]
        public void Initialize_WithTVSeriesType_SetsCorrectListTitle()
        {
            _viewModel.Initialize("tvSeries");
            Assert.AreEqual("TV Shows", _viewModel.ListTitle);
        }

        [TestMethod]
        public void Initialize_WithInvalidType_SetsDefaultListTitle()
        {
            _viewModel.Initialize("invalid");
            Assert.AreEqual("Titles", _viewModel.ListTitle);
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
        public void SearchText_ShortInput_ReloadsOriginalData()
        {
            _viewModel.Initialize("movie");
            _viewModel.SearchText = "Test";  // Set initial search
            _viewModel.IsLoading = false;

            _viewModel.SearchText = "a";  // Too short to trigger search

            Assert.IsFalse(_viewModel.IsLoading);
        }

        [TestMethod]
        public void BackCommand_FromGenreView_NavigatesToGenres()
        {
            _viewModel.Initialize(1);  // Initialize with genre ID
            _viewModel.BackCommand.Execute(null);

            Assert.AreEqual("GenresView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void BackCommand_FromTitleTypeView_NavigatesToHome()
        {
            _viewModel.Initialize("movie");  // Initialize with title type
            _viewModel.BackCommand.Execute(null);

            Assert.AreEqual("HomeView", _navigationService.LastNavigatedView);
        }

        [TestMethod]
        public void SearchCommand_DoesNotThrow()
        {
            try
            {
                _viewModel.SearchCommand.Execute(null);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Search command threw an exception");
            }
        }

        [TestMethod]
        public void SearchText_NullInput_DoesNotThrow()
        {
            try
            {
                _viewModel.SearchText = null;
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("Setting SearchText to null threw an exception");
            }
        }
    }
}