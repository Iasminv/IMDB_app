using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using IMDB.Views;
using IMDB.Commands;
using IMDB_App.Data;
using IMDB_App.Models;
using System.Runtime.CompilerServices;

namespace IMDB.ViewModels
{
    public class IMDBViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Title> _titles;
        private ObservableCollection<Name> _names;
        private ObservableCollection<Genre> _genres;
        private ObservableCollection<Principal> _principals;
        private ObservableCollection<Rating> _ratings;
        private ObservableCollection<Episode> _episodes;
        private ObservableCollection<TitleAlias> _titleAliases;
        private bool _isLoading;

        public ObservableCollection<Title> Titles
        {
            get => _titles;
            private set
            {
                _titles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Title> FilteredTitles { get; private set; }

        public ObservableCollection<Name> Names
        {
            get => _names;
            private set
            {
                _names = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Name> FilteredNames { get; private set; }

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            private set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Genre> FilteredGenres { get; private set; }

        public ObservableCollection<Principal> Principals
        {
            get => _principals;
            private set
            {
                _principals = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Principal> FilteredPrincipals { get; private set; }

        public ObservableCollection<Rating> Ratings
        {
            get => _ratings;
            private set
            {
                _ratings = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Rating> FilteredRatings { get; private set; }

        public ObservableCollection<Episode> Episodes
        {
            get => _episodes;
            private set
            {
                _episodes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Episode> FilteredEpisodes { get; private set; }

        public ObservableCollection<TitleAlias> TitleAliases
        {
            get => _titleAliases;
            private set
            {
                _titleAliases = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TitleAlias> FilteredTitleAliases { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private object _selectedView;
        public object SelectedView
        {
            get { return _selectedView; }
            set { _selectedView = value; OnPropertyChanged(nameof(SelectedView)); }
        }

        // Initialize commands
        public ICommand CloseCommand { get; }

        public IMDBViewModel()
        {
            // Initialize empty collections
            Titles = new ObservableCollection<Title>();
            FilteredTitles = new ObservableCollection<Title>();
            Names = new ObservableCollection<Name>();
            FilteredNames = new ObservableCollection<Name>();
            Genres = new ObservableCollection<Genre>();
            FilteredGenres = new ObservableCollection<Genre>();
            Principals = new ObservableCollection<Principal>();
            FilteredPrincipals = new ObservableCollection<Principal>();
            Ratings = new ObservableCollection<Rating>();
            FilteredRatings = new ObservableCollection<Rating>();
            Episodes = new ObservableCollection<Episode>();
            FilteredEpisodes = new ObservableCollection<Episode>();
            TitleAliases = new ObservableCollection<TitleAlias>();
            FilteredTitleAliases = new ObservableCollection<TitleAlias>();

            // Initialize commands
            CloseCommand = new RelayCommand(CloseApplication);

            // HomeView as the default view
            SelectedView = new HomeView();

            // Load data
            LoadData();
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadData()
        {
            try
            {
                IsLoading = true;

                using (var context = new ImdbContext())
                {
                    // Load titles with their related data
                    Titles = new ObservableCollection<Title>(
                        context.Titles
                            .Include(t => t.Genres)
                            .Include(t => t.Rating)
                            .Include(t => t.TitleAliases)
                            .Include(t => t.EpisodeTitle)
                            .Include(t => t.EpisodeParentTitles)
                            .AsNoTracking()
                            .ToList());

                    // Load names with their related data
                    Names = new ObservableCollection<Name>(
                        context.Names
                            .Include(n => n.Principals)
                            .AsNoTracking()
                            .ToList());

                    // Load other entities
                    Genres = new ObservableCollection<Genre>(
                        context.Genres
                            .AsNoTracking()
                            .ToList());

                    Principals = new ObservableCollection<Principal>(
                        context.Principals
                            .Include(p => p.Name)
                            .AsNoTracking()
                            .ToList());

                    Ratings = new ObservableCollection<Rating>(
                        context.Ratings
                            .AsNoTracking()
                            .ToList());

                    Episodes = new ObservableCollection<Episode>(
                        context.Episodes
                            .Include(e => e.ParentTitle)
                            .Include(e => e.Title)
                            .AsNoTracking()
                            .ToList());

                    TitleAliases = new ObservableCollection<TitleAlias>(
                        context.TitleAliases
                            .Include(ta => ta.TitleNavigation)
                            .AsNoTracking()
                            .ToList());
                }

                // Initialize filtered collections
                FilteredTitles = new ObservableCollection<Title>(Titles);
                FilteredNames = new ObservableCollection<Name>(Names);
                FilteredGenres = new ObservableCollection<Genre>(Genres);
                FilteredPrincipals = new ObservableCollection<Principal>(Principals);
                FilteredRatings = new ObservableCollection<Rating>(Ratings);
                FilteredEpisodes = new ObservableCollection<Episode>(Episodes);
                FilteredTitleAliases = new ObservableCollection<TitleAlias>(TitleAliases);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}