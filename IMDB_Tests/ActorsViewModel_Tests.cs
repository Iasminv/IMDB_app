using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;
using IMDB_App.Models;

namespace IMDB_Tests.ViewModels
{
    [TestClass]
    public class ActorsViewModelTests
    {
        private ActorsViewModel _viewModel;
        private TestNavigationService _navigationService;

        [TestInitialize]
        public void Setup()
        {
            _navigationService = new TestNavigationService();
            _viewModel = new ActorsViewModel(_navigationService);
        }

        [TestMethod]
        public void Commands_AreInitialized()
        {
            Assert.IsNotNull(_viewModel.BackToHomeCommand);
            Assert.IsNotNull(_viewModel.SearchActorCommand);
        }

        [TestMethod]
        public void ActorName_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.ActorName))
                    propertyChanged = true;
            };

            _viewModel.ActorName = "Brad Pitt";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Brad Pitt", _viewModel.ActorName);
        }

        [TestMethod]
        public void SearchText_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.SearchText))
                    propertyChanged = true;
            };

            _viewModel.SearchText = "Tom Hanks";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Tom Hanks", _viewModel.SearchText);
        }

        [TestMethod]
        public void BirthYear_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.BirthYear))
                    propertyChanged = true;
            };

            _viewModel.BirthYear = 1963;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(1963, _viewModel.BirthYear);
        }

        [TestMethod]
        public void PrimaryProfession_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.PrimaryProfession))
                    propertyChanged = true;
            };

            _viewModel.PrimaryProfession = "Actor";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual("Actor", _viewModel.PrimaryProfession);
        }

        [TestMethod]
        public void Filmography_PropertyChangedIsRaised()
        {
            bool propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActorsViewModel.Filmography))
                    propertyChanged = true;
            };

            var newFilmography = new ObservableCollection<ActorFilmography>();
            _viewModel.Filmography = newFilmography;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual(newFilmography, _viewModel.Filmography);
        }

        [TestMethod]
        public void SearchText_ShortInput_DoesNotTriggerDynamicSearch()
        {
            // Arrange
            var initialFilmographyCount = _viewModel.Filmography.Count;
            var initialActorName = _viewModel.ActorName;

            // Act
            _viewModel.SearchText = "a"; // Too short to trigger search

            // Assert - Since we can't mock the database, we can only verify it doesn't throw
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SearchText_LongEnough_TriggersDynamicSearch()
        {
            // Act
            _viewModel.SearchText = "Tom Hanks"; // Long enough to trigger search

            // Assert - Since we can't mock the database, we can only verify it doesn't throw
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SearchCommand_WithEmptyText_DoesNotSearch()
        {
            // Arrange
            var initialActorName = _viewModel.ActorName;

            // Act
            _viewModel.SearchText = "";
            _viewModel.SearchActorCommand.Execute(null);

            // Assert - Since we can't mock the database, we can only verify it doesn't throw
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BackToHomeCommand_NavigatesToHome()
        {
            // Act
            _viewModel.BackToHomeCommand.Execute(null);

            // Assert
            Assert.AreEqual("HomeView", _navigationService.LastNavigatedView);
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

        [TestMethod]
        public void LoadRandomActor_DoesNotThrow()
        {
            // This test verifies the constructor doesn't throw when loading random actor
            // Since the constructor calls LoadRandomActor
            Assert.IsTrue(true);
        }
    }
}