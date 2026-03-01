using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStoreApp.Messages;
using MusicStoreApp.Models;

namespace MusicStoreApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();
    
    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        AlbumViewModel? album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
        if (album is not null)
        {
            Albums.Add(album);
            await album.SaveToDiskAsync();
        }
    }
    
    public MainWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<CheckAlbumAlreadyExistsMessage>(this, (v, m) =>
        {
            m.Reply(Albums.Contains(m.Album));
        });
        LoadAlbums();
    }

    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x)).ToList();
        foreach (var album in albums)
        {
            Albums.Add(album);
        }
    }
    
}