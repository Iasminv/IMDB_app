using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using IMDB_App.Data;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace IMDB.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private string _searchText;
        private ObservableCollection<Title> _searchResults = new ObservableCollection<Title>();
        private bool _isLoading;
        private bool _isSearched;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    
                    PerformDynamicSearch();
                }
            }
        }

        public ObservableCollection<Title> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsSearched
        {
            get => _isSearched;
            set => SetProperty(ref _isSearched, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand NavigateToMoviesCommand { get; }
        public ICommand NavigateToTVShowsCommand { get; }
        public ICommand NavigateToActorsCommand { get; }
        public ICommand NavigateToGenresCommand { get; }
        public ICommand SelectTitleCommand { get; }

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SearchCommand = new RelayCommand(Search);
            NavigateToMoviesCommand = new RelayCommand(() => NavigateToMoviesList("movie"));
            NavigateToTVShowsCommand = new RelayCommand(() => NavigateToMoviesList("tvSeries"));
            NavigateToActorsCommand = new RelayCommand(NavigateToActors);
            NavigateToGenresCommand = new RelayCommand(NavigateToGenres);
            SelectTitleCommand = new RelayCommand<Title>(SelectTitle);
        }

        private void PerformDynamicSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 2)
            {
                // Clear results if search text is empty 
                SearchResults.Clear();
                IsSearched = false;
                return;
            }

            IsLoading = true;
            IsSearched = true;

            using (var context = new ImdbContext())
            {
                var titles = context.Titles
                    .Where(t => t.PrimaryTitle.Contains(SearchText))
                    .Include(t => t.Rating)
                    .Take(10)  // Limit to 10 for dynamic search to be responsive
                    .ToList();

                SearchResults = new ObservableCollection<Title>(titles);
            }

            IsLoading = false;
        }

        private void Search()
        {
            
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            IsLoading = true;

            using (var context = new ImdbContext())
            {
                var titles = context.Titles
                    .Where(t => t.PrimaryTitle.Contains(SearchText))
                    .Include(t => t.Rating)
                    .Take(20)
                    .ToList();

                SearchResults = new ObservableCollection<Title>(titles);
            }

            IsSearched = true;
            IsLoading = false;
        }

        private void NavigateToMoviesList(string titleType)
        {
            _navigationService.NavigateToWithViewModel<MovieListView, MovieListViewModel>(titleType);
        }

        private void NavigateToActors()
        {
            _navigationService.NavigateToWithViewModel<ActorsView, ActorsViewModel>();
        }

        private void NavigateToGenres()
        {
            _navigationService.NavigateToWithViewModel<GenresView, GenresViewModel>();
        }

        private void SelectTitle(Title title)
        {
            if (title == null) return;
            _navigationService.NavigateToWithViewModel<MovieDetailsView, MovieDetailsViewModel>(title.TitleId);
        }
    }
}