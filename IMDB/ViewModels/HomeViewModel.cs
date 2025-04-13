using IMDB.Commands;
using IMDB.Services;
using IMDB.Views;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
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

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
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
            NavigateToMoviesCommand = new RelayCommand(() => NavigateToMovies("movie"));
            NavigateToTVShowsCommand = new RelayCommand(() => NavigateToMovies("tvSeries"));
            NavigateToActorsCommand = new RelayCommand(NavigateToActors);
            NavigateToGenresCommand = new RelayCommand(NavigateToGenres);
            SelectTitleCommand = new RelayCommand<Title>(SelectTitle);

            // Data is pre-loaded by App.LoadData()
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            IsLoading = true;

            using (var context = new IMDB_App.Data.ImdbContext())
            {
                var titles = context.Titles
                    .Where(t => t.PrimaryTitle.Contains(SearchText))
                    .Include(t => t.Rating)
                    .Take(20)
                    .ToList();

                SearchResults = new ObservableCollection<Title>(titles);
            }

            IsLoading = false;
        }

        private void NavigateToMovies(string titleType)
        {
            _navigationService.NavigateToWithViewModel<MovieDetailsView, MovieDetailsViewModel>(titleType);
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