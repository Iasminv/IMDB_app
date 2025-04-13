using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB.ViewModels;
using IMDB.Services;
using System.Collections.ObjectModel;

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

        // This test is commented out because the initial state of Filmography is not set in the constructor.
        // It currently pulls a random actor & filmography on construction.
        //[TestMethod]
        //public void InitialState_PropertiesAreInitialized()
        //{
        //    Assert.IsNotNull(_viewModel.Filmography);
        //    Assert.AreEqual(0, _viewModel.Filmography.Count);
        //    Assert.IsNull(_viewModel.SearchText);
        //    Assert.IsNull(_viewModel.ActorName);
        //    Assert.IsNull(_viewModel.BirthYear);
        //    Assert.IsNull(_viewModel.PrimaryProfession);
        //}

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
    }
}