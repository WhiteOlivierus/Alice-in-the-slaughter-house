using System.Diagnostics;
using BB;

public static class BlackBoard
{
    private static BlackBoardType<System.Single> _PigRange = new BlackBoardType<System.Single>(nameof(PigRange), typeof(PigCarry));
    public static System.Single PigRange
    {
        get { return _PigRange.GetValue(); }
        private set { }
    }
    
    private static BlackBoardType<System.Int32> _ButtonOffset = new BlackBoardType<System.Int32>(nameof(ButtonOffset), typeof(PigController));
    public static System.Int32 ButtonOffset
    {
        get { return _ButtonOffset.GetValue(); }
        private set { }
    }
    
    private static BlackBoardType<System.Single> _PigSpeed = new BlackBoardType<System.Single>(nameof(PigSpeed), typeof(PigMovement));
    public static System.Single PigSpeed
    {
        get { return _PigSpeed.GetValue(); }
        private set { }
    }
    
    private static BlackBoardType<System.Int32> _PigLayer = new BlackBoardType<System.Int32>(nameof(PigLayer), typeof(PigMovement));
    public static System.Int32 PigLayer
    {
        get { return _PigLayer.GetValue(); }
        private set { }
    }
    
    private static BlackBoardType<System.Single> _PlayerSpeed = new BlackBoardType<System.Single>(nameof(PlayerSpeed), typeof(PlayerMovement));
    public static System.Single PlayerSpeed
    {
        get { return _PlayerSpeed.GetValue(); }
        private set { }
    }
    
}
