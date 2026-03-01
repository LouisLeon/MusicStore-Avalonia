using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MusicStoreApp.Models;

namespace MusicStoreApp.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
    // The class had to be made partial and needs to inherit from view model base
    // in order for the viewlocator.cs class to find it and use it as a data template for AlbumView automatically
    
    // An alternative way of doing this would be to just explicitly make a datatemplate in app.axaml where 
    // that data template uses views:AlbumView and datatype AlbumViewModel

    private readonly Album album;
    
    public AlbumViewModel(Album album)
    {
        this.album = album;
    }
    
    public string Artist => album.Artist;
    public string Title => album.Title;
    public Task<Bitmap?> Cover => LoadCoverAsync();

    private async Task<Bitmap?> LoadCoverAsync()
    {
        try
        {
            // We wait a few ms to demonstrate that the images are loaded in the background.
            // Remove this line in production.
            await Task.Delay(200);

            await using (var imageStream = await album.LoadCoverBitmapAsync())
            {
                return await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveToDiskAsync()
    {
        await album.SaveAsync();

        if (await LoadCoverAsync() is Bitmap cover)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using (var fs = album.SaveCoverBitmapStream())
                {
                    cover.Save(fs);
                }
            });
        }
    }
    
    public bool Equals(AlbumViewModel? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return album.Equals(other.album);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AlbumViewModel)obj);
    }

    public override int GetHashCode()
    {
        return album.GetHashCode();
    }
}