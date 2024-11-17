namespace Puzzle
{
    public interface IPuzzleCore
    {
        public void InstramData(byte[] data);
        public CubeMap<byte> CubeMap { get; set; }
        public Mediator Mediator { get; }
        public void Init();
    }

}