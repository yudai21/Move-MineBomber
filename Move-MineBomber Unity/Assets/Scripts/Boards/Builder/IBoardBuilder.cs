namespace Bomb.Boards.Builders
{
    public interface IBoardBuilder
    {
        int Create(out BoardManager board);
    }
}