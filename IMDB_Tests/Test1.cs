using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IMDB.ViewModels;
using IMDB_App.Models;
using System.ComponentModel;
using System.Collections.Generic;

namespace IMDB_Tests
{
    [TestClass]
    public class IMDBViewModelTests
    {
        private IMDBViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new IMDBViewModel();
        }

        [TestMethod]
        public void SearchText_PropertyChanged_Test()
        {
            // Arrange
            var propertiesChanged = new List<string>();
            _viewModel.PropertyChanged += (s, e) => { propertiesChanged.Add(e.PropertyName); };

            // Act
            _viewModel.SearchText = "Test Search";

            // Assert
            Assert.AreEqual(1, propertiesChanged.Count);
            Assert.AreEqual("SearchText", propertiesChanged[0]);
            Assert.AreEqual("Test Search", _viewModel.SearchText);
        }

        [TestMethod]
        public void IsLoading_PropertyChanged_Test()
        {
            // Arrange
            var propertiesChanged = new List<string>();
            _viewModel.PropertyChanged += (s, e) => { propertiesChanged.Add(e.PropertyName); };

            // Act
            _viewModel.IsLoading = true;

            // Assert
            Assert.AreEqual(1, propertiesChanged.Count);
            Assert.AreEqual("IsLoading", propertiesChanged[0]);
            Assert.IsTrue(_viewModel.IsLoading);
        }

        [TestMethod]
        public void FilteredTitles_PropertyChanged_Test()
        {
            // Arrange
            var propertiesChanged = new List<string>();
            _viewModel.PropertyChanged += (s, e) => { propertiesChanged.Add(e.PropertyName); };
            var newCollection = new ObservableCollection<Title>();

            // Act
            _viewModel.FilteredTitles = newCollection;

            // Assert
            Assert.AreEqual(1, propertiesChanged.Count);
            Assert.AreEqual("FilteredTitles", propertiesChanged[0]);
            Assert.AreEqual(newCollection, _viewModel.FilteredTitles);
        }

        [TestMethod]
        public void FilteredTitles_InitiallyEmpty()
        {
            // Assert
            Assert.AreEqual(0, _viewModel.FilteredTitles.Count);
        }

        [TestMethod]
        public void Commands_NotNull()
        {
            // Assert
            Assert.IsNotNull(_viewModel.SearchCommand);
            Assert.IsNotNull(_viewModel.LoadTopMoviesCommand);
            Assert.IsNotNull(_viewModel.LoadMoreCommand);
            Assert.IsNotNull(_viewModel.CloseCommand);
        }

        [TestMethod]
        public void SearchCommand_EmptySearch_ClearsResults()
        {
            // Arrange
            _viewModel.FilteredTitles.Add(new Title { PrimaryTitle = "Test Movie" });

            // Act
            _viewModel.SearchCommand.Execute("");

            // Wait for async operation to complete
            Task.Delay(100).Wait();

            // Assert
            Assert.AreEqual(0, _viewModel.FilteredTitles.Count);
        }

        [TestMethod]
        public void IsLoading_InitiallyFalse()
        {
            // Assert
            Assert.IsFalse(_viewModel.IsLoading);
        }

        [TestMethod]
        public void SearchText_InitiallyNull()
        {
            // Assert
            Assert.IsNull(_viewModel.SearchText);
        }

        [TestMethod]
        public void SearchText_CanBeSetToNull()
        {
            // Act
            _viewModel.SearchText = null;

            // Assert
            Assert.IsNull(_viewModel.SearchText);
        }

        [TestMethod]
        public void FilteredTitles_CanAddAndRemoveItems()
        {
            // Arrange
            var title = new Title { PrimaryTitle = "Test Movie" };

            // Act - Add
            _viewModel.FilteredTitles.Add(title);

            // Assert
            Assert.AreEqual(1, _viewModel.FilteredTitles.Count);
            Assert.AreEqual("Test Movie", _viewModel.FilteredTitles[0].PrimaryTitle);

            // Act - Remove
            _viewModel.FilteredTitles.Remove(title);

            // Assert
            Assert.AreEqual(0, _viewModel.FilteredTitles.Count);
        }

        [TestMethod]
        public void FilteredTitles_CanBeClearedDirectly()
        {
            // Arrange
            _viewModel.FilteredTitles.Add(new Title { PrimaryTitle = "Test Movie 1" });
            _viewModel.FilteredTitles.Add(new Title { PrimaryTitle = "Test Movie 2" });

            // Act
            _viewModel.FilteredTitles.Clear();

            // Assert
            Assert.AreEqual(0, _viewModel.FilteredTitles.Count);
        }
    }
}