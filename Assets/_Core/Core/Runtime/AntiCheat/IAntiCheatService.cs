namespace Kelsey
{
    public interface IAntiCheatService
    {
        bool IsCheater { get; }
        bool IsBypassCheat { get; set; }
    }
}