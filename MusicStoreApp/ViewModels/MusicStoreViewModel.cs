using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStoreApp.Messages;
using MusicStoreApp.Models;

namespace MusicStoreApp.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase
{
    [ObservableProperty] 
    public partial string? SearchText { get; set; }
    
    [ObservableProperty]
    public partial bool IsBusy { get; private set; }
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuyMusicCommand))]
    public partial AlbumViewModel? SelectedAlbum { get; set; }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
    
    private bool CanBuyMusic => SelectedAlbum != null;

    [RelayCommand (CanExecute = nameof(CanBuyMusic))]
    private void BuyMusic()
    {
        if (SelectedAlbum != null)
        {
            var albumExists = WeakReferenceMessenger.Default.Send(new CheckAlbumAlreadyExistsMessage(SelectedAlbum));
            if (albumExists)
            {
                WeakReferenceMessenger.Default.Send(new NotificationMessage("This album already exists"));
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new MusicStoreClosedMessage(SelectedAlbum));
            }
        }
    }

    private async Task DoSearch(string? term)
    {
        IsBusy = true;
        SearchResults.Clear();

        var albums = await Album.SearchAsync(term);

        foreach (var album in albums)
        {
            var vm = new AlbumViewModel(album);
            SearchResults.Add(vm);
        }

        IsBusy = false;
    }

    partial void OnSearchTextChanged(string? value)
    {
        _ = DoSearch(SearchText);
    }
}