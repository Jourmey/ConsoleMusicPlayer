using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    public class SongManager
    {
        private List<string> _songs = new List<string>();
        private int _currentIndex = 0;

        public SongManager()
        {
            var s = GetSongs();
            if (s != null && s.Count != 0)
            {
                _songs = s;
            }
        }

        internal string GetNextSong()
        {
            if (_songs.Count == 0)
            {
                return string.Empty;
            }

            _currentIndex = (_currentIndex + 1) % _songs.Count;
            return _songs[_currentIndex];
        }

        internal SongContext GetSongContext()
        {
            if (_songs.Count == 0)
            {
                return new SongContext();
            }

            return new SongContext
            {
                MusicName = Path.GetFileNameWithoutExtension(_songs[_currentIndex]),
                NextName = Path.GetFileNameWithoutExtension(_songs[(_currentIndex + 1) % _songs.Count]),
            };
        }


        private List<string> GetSongs()
        {

            DirectoryInfo directory = new DirectoryInfo(UserConfig.SongDic);

            if (!directory.Exists)
            {
                return null;
            }

            List<FileInfo> files = new List<FileInfo>();

            return directory.GetFiles().Where((a) => Path.GetExtension(a.Name) == ".mp3").Select((a) => a.FullName).ToList();

        }
    }
}
