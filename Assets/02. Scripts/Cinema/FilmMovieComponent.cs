using UnityEngine;

namespace Cinema
{
    public class FilmMovieComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField, Range(0, 9999)] private float _playTime;
        private Film _film = Film.EMPTY;
        private Projector _movie;

        private void Awake()
        {
            _movie = new Projector();
            if (string.IsNullOrEmpty(_sceneName))
            {
                _film = Film.EMPTY;
            }
            else
            {
                _film = new Film(_sceneName, _playTime);
            }
            _movie.ChangeFilm(_film);
        }

        public void PlayMovie()
        {
            Movie.DoPlay(_movie);
        }
    }

}

