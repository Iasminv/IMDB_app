using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using IMDB.Commands;
using IMDB_App.Data;
using IMDB_App.Models;
using System.Runtime.CompilerServices;

namespace IMDB.ViewModels
{
    public class IMDBViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Title> _filteredTitles = new ObservableCollection<Title>();
        private string _searchText;
        private bool _isLoading;

        public ObservableCollection<Title> FilteredTitles
        {
            get => _filteredTitles;
            set
            {
                _filteredTitles = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand LoadTopMoviesCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand CloseCommand { get; }

        private int _currentPage = 0;
        private string _currentSearchTerm = "";

        public IMDBViewModel()
        {
            CloseCommand = new RelayCommand(CloseApplication);

            SearchCommand = new RelayCommand(async (param) =>
            {
                _currentSearchTerm = param?.ToString() ?? "";
                _currentPage = 0;
                await SearchTitlesAsync(_currentSearchTerm);
            });

            LoadTopMoviesCommand = new RelayCommand(async () =>
            {
                _currentPage = 0;
                await LoadTopMoviesAsync();
            });

            LoadMoreCommand = new RelayCommand(async () =>
            {
                _currentPage++;
                if (string.IsNullOrEmpty(_currentSearchTerm))
                    await LoadTopMoviesAsync();
                else
                    await SearchTitlesAsync(_currentSearchTerm);
            });
        }

        private async Task SearchTitlesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                FilteredTitles.Clear();
                return;
            }

            try
            {
                IsLoading = true;

                using (var context = new ImdbContext())
                {
                    var query = context.Titles
                        .Where(t => t.PrimaryTitle.Contains(searchTerm))
                        .Include(t => t.Rating)
                        .AsNoTracking();

                    var results = await query
                        .Skip(_currentPage * 50)
                        .Take(50)
                        .ToListAsync();

                    if (_currentPage == 0)
                        FilteredTitles = new ObservableCollection<Title>(results);
                    else
                        foreach (var title in results)
                            FilteredTitles.Add(title);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTopMoviesAsync()
        {
            try
            {
                IsLoading = true;

                using (var context = new ImdbContext())
                {
                    var query = context.Titles
                        .Where(t => t.TitleType == "movie")
                        .Include(t => t.Rating)
                        .OrderByDescending(t => t.Rating.AverageRating)
                        .AsNoTracking();

                    var results = await query
                        .Skip(_currentPage * 50)
                        .Take(50)
                        .ToListAsync();

                    if (_currentPage == 0)
                        FilteredTitles = new ObservableCollection<Title>(results);
                    else
                        foreach (var title in results)
                            FilteredTitles.Add(title);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load movies: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
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
    }
}