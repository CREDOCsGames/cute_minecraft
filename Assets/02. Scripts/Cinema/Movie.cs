namespace Cinema
{
    public static class Movie
    {
        public readonly static MovieCamera INTRO = new(new Film("Intro", 10));
        public readonly static MovieCamera ENTER_GAME = new(new Film("GameStart", 10));
        public readonly static MovieCamera ENTER_BOSS = new(new Film("GameStart", 10));
        private static MovieCamera _movie = MovieCamera.DEFAULT;
        public static void ChangeCamera(MovieCamera movie)
        {
            if (_movie.IsPlaying)
            {
                _movie.DoSkip();
            }
            _movie = movie;
        }
        public static void DoPlay()
        {
            _movie.DoPlay();
        }
        public static void DoPlay(MovieCamera movie)
        {
            ChangeCamera(movie);
            DoPlay();
        }
        public static void DoSkip()
        {
            _movie.DoSkip();
        }

    }
}