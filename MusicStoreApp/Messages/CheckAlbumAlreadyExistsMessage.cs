using CommunityToolkit.Mvvm.Messaging.Messages;
using MusicStoreApp.ViewModels;

namespace MusicStoreApp.Messages;

public class CheckAlbumAlreadyExistsMessage(AlbumViewModel album) : RequestMessage<bool>
{
    public AlbumViewModel Album { get; } = album;
}