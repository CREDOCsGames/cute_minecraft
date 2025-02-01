using Flow;
using System;
using UnityEngine.SceneManagement;
using CoroutineRunner = Unity.VisualScripting.CoroutineRunner;

namespace Cinema
{
    public class Projector
    {
        public static readonly Projector DEFAULT = new Projector();
        public event Action OnPlay;
        public event Action OnSkip;
        public event Action OnEnd;
        public bool IsStart => IsStart && !_player.IsPause;
        public bool IsPlaying => _player.IsStart && !_player.IsPause;
        public readonly byte ClosingDuration;
        private Film _film = Film.EMPTY;
        private readonly Timer _player = new();
        private readonly CameraUtil _cameraUtil = new();
        private readonly CoroutineRunner _runner = CoroutineRunner.instance;

        public Projector(byte closingDuration = 2)
        {
            AddEventMoviePlay();
            AddEventMovieEnd();
            AddEventMovieSkip();
            ClosingDuration = closingDuration;

        }
        public Projector(Film film, byte closingDuration = 2) : this(closingDuration)
        {
            ChangeFilm(film);
        }
        public void ChangeFilm(Film film)
        {
            if (_player.IsStart)
            {
                _player.DoStop();
            }

            if (film.IsBad)
            {
                _film = Film.EMPTY;
                return;
            }

            _film = film;
            _player.SetTimeout(film.Time + ClosingDuration);
        }
        public void DoPlay() => _player.DoStart();
        public void DoSkip() => _player.DoStop();
        private void LoadFilm(Film film)
        {
            SceneManager.LoadSceneAsync(film.Name, LoadSceneMode.Additive);
        }
        private void UnloadFilm(Film film)
        {
            SceneManager.UnloadSceneAsync(film.Name);
        }
        private void AddEventMoviePlay()
        {
            _player.OnStart += (t) => _cameraUtil.DisableAllCamerasInScene();
            _player.OnStart += (t) => LoadFilm(_film);
            _player.OnStart += (t) => OnPlay?.Invoke();
            _player.OnStart += (t) => _runner.StartCoroutine(_player.Update());
        }
        private void AddEventMovieEnd()
        {
            _player.OnTimeout += (t) => UnloadFilm(_film);
            _player.OnTimeout += (t) => _cameraUtil.EnableAllCamerasInScene();
            _player.OnTimeout += (t) => _runner.StopCoroutine(_player.Update());
            _player.OnTimeout += (t) => OnEnd?.Invoke();
        }
        private void AddEventMovieSkip()
        {
            _player.OnStop += (t) => OnSkip?.Invoke();
            _player.OnStop += (t) => UnloadFilm(_film);
            _player.OnStop += (t) => _cameraUtil.EnableAllCamerasInScene();
            _player.OnStop += (t) => _runner.StopCoroutine(_player.Update());
            _player.OnStop += (t) => OnEnd?.Invoke();
        }
    }
}