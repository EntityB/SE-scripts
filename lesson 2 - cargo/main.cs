public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

List<PropellerGroup> propellers = new List<PropellerGroup>();
int iteration;

public Program()
{
    iteration = 0;
    Runtime.UpdateFrequency = UpdateFrequency.Update1;
    makePropellers();
}

public void Main(string argument, UpdateType updateSource)
{
    Echo("Iteration:" + iteration);
    iteration++;

    // foreach (var block in blocks)
    // {
    //     Echo($"- {block.CustomName}");
    // }

    Fly("W");
    // IMyCargoContainer containerA = GridTerminalSystem.GetBlockWithName("Cx1") as IMyCargoContainer;
    // IMyCargoContainer containerB = GridTerminalSystem.GetBlockWithName("Cx2") as IMyCargoContainer;

    // IMyInventory inventoryA = (containerA).GetInventory(0);
    // IMyInventory inventoryB = (containerB).GetInventory(0);


    // IMyInventory movableInv = inventoryA;
    // IMyInventory targetInv = inventoryB;

    // if(inventoryB.IsItemAt(0)) {
    //     movableInv = inventoryB;
    //     targetInv = inventoryA;
    // }

    // while(movableInv.IsItemAt(0)) {

    //     if (!targetInv.IsFull) {
    //         movableInv.TransferItemTo(targetInv, 0, null, true, null);
    //     } else {
    //     // targetInv is full, let's just break out of the for loop
    //     break;
    //     }
    // }
}

public void Fly(string direction) {
    if(direction == "W")
    foreach (var propeller in propellers)
    {
        Echo("Angle: " + propeller.Rotor.Angle);
        if(propeller.Rotor.Angle >= 0 && propeller.Rotor.Angle < 3.141592) {
            InventoryFlushToInventory(propeller.CI1, propeller.CI2);
        } else {
            InventoryFlushToInventory(propeller.CI2, propeller.CI1);
        }
        
    }
}

public List<IMyTerminalBlock> GetListedGroup(string groupName)
{
    IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName(groupName);
    if (group == null)
    {
        DebugMsg("Group " + groupName + " not found");
        return null;
    }
    List<IMyTerminalBlock> listedGroup = new List<IMyTerminalBlock>();
    group.GetBlocks(listedGroup);
    return listedGroup;
    // foreach (var block in blocks)
    // {
    //     Echo($"- {block.CustomName}");
    // }
}

public void InventoryFlushToInventory(IMyInventory src, IMyInventory dst) {
    while(src.IsItemAt(0)) {
        Echo("Do");
        if (!dst.IsFull) {
            src.TransferItemTo(dst, 0, null, true, null);
        } else {
            Echo("dst is full");
        // targetInv is full, let's just break out of the for loop
        break;
        }
    }
}

public void makePropellers() {
    List<IMyTerminalBlock> C1ListedGroup = GetListedGroup("C1");
    List<IMyTerminalBlock> C2ListedGroup = GetListedGroup("C2");
    List<IMyTerminalBlock> RotorListedGroup = GetListedGroup("Rotors");
    
    foreach (IMyMotorStator rotor in RotorListedGroup) {
        IMyCubeGrid topGrid = rotor.TopGrid;

        foreach (var C1 in C1ListedGroup)
        {
            if(testSameGrid(C1, topGrid)) {
                foreach (var C2 in C2ListedGroup)
                {
                    if(testSameGrid(C1, C2)) {
                        propellers.Add(new PropellerGroup(C1.GetInventory(0), C2.GetInventory(0), rotor));
                        Echo("PropellerGroup made");       

                        continue;
                    }
                }
                continue;
            }
        }
    }
}

public bool testSameGrid(IMyTerminalBlock a, IMyTerminalBlock b) {
    return a.CubeGrid.ToString() == b.CubeGrid.ToString();
}

public bool testSameGrid(IMyTerminalBlock a, IMyCubeGrid grid) {
    return a.CubeGrid.ToString() == grid.ToString();
}

public struct PropellerGroup
{
    public PropellerGroup(IMyInventory ci1, IMyInventory ci2, IMyMotorStator rotor)
    {
        CI1 = ci1;
        CI2 = ci2;
        Rotor = rotor;
    }

    public IMyInventory CI1 { get; }
    public IMyInventory CI2 { get; }
    public IMyMotorStator Rotor { get; }
}

void DebugMsg(string msg) {
    Echo("DEBUG: " + msg);
}