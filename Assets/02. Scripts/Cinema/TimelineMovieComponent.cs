using UnityEngine;
using UnityEngine.Timeline;

namespace Cinema
{
    public static class FilmFactory
    {
        public static Film MakeFilmAs(TimelineAsset timeline)
        {
            return new Film(timeline.name, (float)timeline.duration);
        }
    }

    public class TimelineMovieComponent : MonoBehaviour
    {
        [SerializeField] private TimelineAsset _timeLine;
        [SerializeField] private Object _scene;
        private Film _film = Film.EMPTY;
        private Projector _movie;

        private void Awake()
        {
            _movie = new Projector();
            if (_scene && _timeLine)
            {
                _film = FilmFactory.MakeFilmAs(_timeLine);
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