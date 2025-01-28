using UnityEngine;
using UnityEngine.Timeline;

namespace Cinema
{
    public class TimelineMovieComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private TimelineAsset _timeLine;
        private Film _film = Film.EMPTY;
        private MovieCamera _movie;

        private void Awake()
        {
            _movie = new MovieCamera();
            if (string.IsNullOrEmpty(_sceneName) || _timeLine)
            {
                _film = new Film(_sceneName, (float)_timeLine.duration);
            }
            else
            {
                _film = Film.EMPTY;
            }
            _movie.ChangeFilm(_film);
        }

        public void PlayMovie()
        {

            Movie.DoPlay(_movie);
        }
    }

}