
public Program()
{
    Init();
    // Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Save()
{
}

IMyCockpit GCockpit;
AdvancedProgrammambleBlock me2;

public void Main(string argument, UpdateType updateSource)
{
    // me2.echo(GCockpit.MoveIndicator.ToString());
    
    GetBlocksOfType<IMyCubeBlock>(List<IMyCubeBlock> blocks, Func<IMyCubeBlock, System.Boolean> collect)
}

public void Init() {
    me2 = new AdvancedProgrammambleBlock(Me);

    GCockpit = GridTerminalSystem.GetBlockWithName("Cockpit") as IMyCockpit;
}

public enum StreamType
{
    CustomData,
    Echo
}

public class AdvancedProgrammambleBlock {

    private IMyProgrammableBlock me;
    private string stream;
    public StreamType streamType;
    public int streamLinesLenght;

    public AdvancedProgrammambleBlock(IMyProgrammableBlock programmableBlock) {
        this.me = programmableBlock;
        this.streamType = StreamType.CustomData;
        this.streamLinesLenght = 100; // TODO
    }

    public void echo(string str) {
        this.stream += "\n" + str;
        print();
    }

    public void print() {
        if(this.streamType == StreamType.CustomData) {
            this.me.CustomData = this.stream;
        } else {
            // Echo(this.stream);
        }
    }
}