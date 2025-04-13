using IMDB_App.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace IMDB.Views
{
    public partial class MovieListView : UserControl
    {
        public MovieListView()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is Title title)
            {
                var viewModel = DataContext as ViewModels.MovieListViewModel;
                viewModel?.SelectTitleCommand.Execute(title);
            }
        }
    }
}