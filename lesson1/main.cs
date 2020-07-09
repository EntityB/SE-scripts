public Program()
{
    // The constructor, called only once every session and
    // always before any other method is called. Use it to
    // initialize your script. 
    //     
    // The constructor is optional and can be removed if not
    // needed.
    // 
    // It's recommended to set RuntimeInfo.UpdateFrequency 
    // here, which will allow your script to run itself without a 
    // timer block.
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

    private string assemblerName = "Assembler [BPA]";  
    private string projectorName = "Projector [BPP]";  
    // List<string> blocks = new List<string>();
    // List<Sandbox.ModAPI.Ingame.MyProductionItem> items = new List<Sandbox.ModAPI.Ingame.MyProductionItem>();
    MyDefinitionId definitionId;

public void Main(string argument, UpdateType updateSource)
{

    IMyProjector theProjector = GridTerminalSystem.GetBlockWithName(projectorName) as IMyProjector;
    IMyAssembler assembler = GridTerminalSystem.GetBlockWithName(assemblerName) as IMyAssembler;
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.
    // 
    // The method itself is required, but the arguments above
    // can be removed if not needed.


    Echo("Hi " + theProjector.Components);

    // string[] strDetailedInfo = (theProjector as IMyTerminalBlock).DetailedInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    // blocks = new List<string>(strDetailedInfo);

    // Echo("Hi " + blocks);
    
    // Echo("Hi " + theProjector.GetType().GetProperties(BindingFlags.Public));
    
    assembler.Enabled = false;
    // assembler.AddQueueItem(, 1);
    // assembler.GetQueue(items);
    // MyProductionItem productionItems = items[0];

    // Echo("Amount " + productionItems);
    // DebugBlock(theProjector);
    MyDefinitionId.TryParse("MyObjectBuilder_BlueprintDefinition", "tatar", out definitionId);
    MyDefinitionId steelPlateId = MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/SteelPlate");

    // decimal amount = 3000;
    // Echo("definitionId " + definitionId);
    // Echo("definitionId " + steelPlateId);


    // Echo("TypeId " + steelPlateId.TypeId);
    // Echo("SubtypeId " + steelPlateId.SubtypeId);

    Echo("Components " + assembler.Components);

    foreach (MyComponentBase component in assembler.Components)
    {
        Echo("component: " + component);
    }



//     assembler.AddQueueItem(steelPlateId, amount);

//     var myList = new List<ITerminalAction>();
//     assembler.GetActions(myList);
//     string str = "";
//     for (int x=0; x<myList.Count; x++) {
//         str = str + ", " + myList[x].Name;
//     }

// Echo("Block Actions: "+str);

    // Echo("Amount " + productionItems.Amount);
    // Echo("BlueprintId " + productionItems.BlueprintId);
    // Echo("ItemId " + productionItems.ItemId);
}

public void DebugBlock(IMyTerminalBlock block) {

    if(block == null) {
        Echo($"Block not Found. Check argument.");
    } else {
        List<ITerminalProperty> properties = new List<ITerminalProperty>();
        block.GetProperties(properties);
        Echo($"Properties:");

        foreach (var property in properties) {
            Echo($"{property.Id}");
        }

        Echo($"Actions::");

        List<ITerminalAction> actions = new List<ITerminalAction>();
        
        block.GetActions(actions);
        
        foreach (var action in actions) {
            Echo($"{action.Id} == {action.Name}");
        }

    }

}
