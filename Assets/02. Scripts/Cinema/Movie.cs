namespace Cinema
{
    public static class Movie
    {
        private static Projector _projector = Projector.DEFAULT;
        public readonly static Projector INTRO = new(FilmContainer.Search("Intro"));
        public readonly static Projector ENTER_GAME = new(FilmContainer.Search("StartGame"));
        public readonly static Projector ENTER_BOSS = new(FilmContainer.Search("EnterBoss"));

        public static void ChangeCamera(Projector movie)
        {
            if (_projector.IsPlaying)
            {
                _projector.DoSkip();
            }
            _projector = movie;
        }
        public static void DoPlay()
        {
            _projector.DoPlay();
        }
        public static void DoPlay(Projector movie)
        {
            ChangeCamera(movie);
            DoPlay();
        }
        public static void DoSkip()
        {
            _projector.DoSkip();
        }

    }
}