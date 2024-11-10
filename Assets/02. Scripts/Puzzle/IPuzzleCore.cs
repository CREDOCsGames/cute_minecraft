namespace Puzzle
{
    public interface IPuzzleCore
    {
        public void InstramData(byte[] data);
        public CubeMap<byte> CubeMap { get; set; }
        public Madiator Madiator { get;}
        public void Init();
    }

}