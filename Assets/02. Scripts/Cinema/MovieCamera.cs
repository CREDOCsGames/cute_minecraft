using Flow;
using System;
using CoroutineRunner = Unity.VisualScripting.CoroutineRunner;

namespace Cinema
{
    public class MovieCamera
    {
        public static readonly MovieCamera DEFAULT = new MovieCamera();
        public event Action OnPlay;
        public event Action OnSkip;
        public event Action OnEnd;
        public readonly byte ClosingDuration;
        public bool IsPlaying => _camera.IsStart && !_camera.IsPause;
        public bool IsStart => IsStart && !_camera.IsPause;
        private Film _film = Film.EMPTY;
        private readonly Timer _camera = new();
        private readonly CameraUtil _cameraUtil = new();
        private readonly CoroutineRunner _runner = CoroutineRunner.instance;

        public MovieCamera(byte closingDuration = 2)
        {
            AddMoviePlayEvent();
            AddMovieEndEvent();
            AddMovieSkipEvent();
            ClosingDuration = closingDuration;

        }
        public MovieCamera(Film film, byte closingDuration = 2) : this(closingDuration)
        {
            ChangeFilm(film);
        }
        public void ChangeFilm(Film film)
        {
            if (_camera.IsStart)
            {
                _camera.DoStop();
            }

            if (film.IsBad)
            {
                _film = Film.EMPTY;
                return;
            }

            _film = film;
            _camera.SetTimeout(film.Time + ClosingDuration);
        }
        public void DoPlay()
        {
            _camera.DoStart();

        }
        public void DoSkip()
        {
            _camera.DoStop();
        }
        private void AddMoviePlayEvent()
        {
            _camera.OnStart += (t) => _cameraUtil.DisableAllCamerasInScene();
            _camera.OnStart += (t) => _cameraUtil.LoadFilm(_film);
            _camera.OnStart += (t) => OnPlay?.Invoke();
            _camera.OnStart += (t) => _runner.StartCoroutine(_camera.Update());
        }
        private void AddMovieEndEvent()
        {
            _camera.OnTimeout += (t) => _cameraUtil.UnloadFilm(_film);
            _camera.OnTimeout += (t) => _cameraUtil.EnableAllCamerasInScene();
            _camera.OnTimeout += (t) => _runner.StopCoroutine(_camera.Update());
            _camera.OnTimeout += (t) => OnEnd?.Invoke();
        }
        private void AddMovieSkipEvent()
        {
            _camera.OnStop += (t) => OnSkip?.Invoke();
            _camera.OnStop += (t) => _cameraUtil.UnloadFilm(_film);
            _camera.OnStop += (t) => _cameraUtil.EnableAllCamerasInScene();
            _camera.OnStop += (t) => _runner.StopCoroutine(_camera.Update());
            _camera.OnStop += (t) => OnEnd?.Invoke();
        }
    }
}