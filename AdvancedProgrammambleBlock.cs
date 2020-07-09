public class AdvancedProgrammambleBlock : IMyProgrammableBlock {

    private IMyProgrammableBlock me;
    private string stream;
    public StreamType streamType;
    public int streamLinesLenght;

    public AdvancedProgrammambleBlock(IMyProgrammableBlock programmableBlock) {
        this.me = programmableBlock;
        this.streamType = StreamType.CustomData;
        this.streamLinesLenght = 100; // TODO
    }

    public void Debug(string str) {
        this.stream += "\n" + str;
    }

    public void showStream() {
        if(this.streamType == StreamType.CustomData) {
            this.me.CustomData(this.stream);
        } else {
            Echo(this.stream);
        }
    }
}