
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;




namespace sprint2.Music;


public interface IMusic
{
    public Song Song {get; set;}
    public bool Looping{get; set;}
    public void Update();
    public void Play();
    public void Stop();
    public void NextSong();
}