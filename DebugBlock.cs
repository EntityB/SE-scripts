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
