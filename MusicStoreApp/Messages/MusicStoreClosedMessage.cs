using MusicStoreApp.ViewModels;

namespace MusicStoreApp.Messages;

public class MusicStoreClosedMessage(AlbumViewModel album)
{
    public AlbumViewModel SelectedAlbum { get; } = album;
}