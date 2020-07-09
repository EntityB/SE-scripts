//Merge drive Mananger script v25 by jonathan:

//necessary for MPD-1,MSD-2-21,MSD-2-S,MSD-35
//optional for PDD-1,PDD-3,MID-6-2

//You can add as many of them as want to your ship
//The script finds these drives automatically, even if you build them in survival (no nametagging necessary)

//The Drives are activated automatically by WASD
//Inertial dampening of drives can be toggled with 'z' just like with normal thrusters.

//Run programmable block with argument "toggle" to turn turn everything on or off (you can put this into your toolbar)


//Setup:
//

string mode = "auto";                                  //"auto"=triggered by WASD if toggled on (it is on by default)
                                                                    //"manual"=all forward drives active if toggled on, no dampening  (it is off by default)

double power = 1.2;                                       //Power of drives in percent when active - can be overclocked beyond 100% (kinda risky)

double dampeningstrength = 0.5;             //Percent value, multiplies with power// 0 turns off inertial dampening completely

double reversepower = 0.5;                      //Mass shift drives can work in reverse mode, this is the power multiplier for that

//
//End of setup
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




//Script body - alter at own risk
//Script body - alter at own risk
//Script body - alter at own risk

//declaring variables
//random stuff
double speedlimit = 8000;  //change speedlimit for drives only to debug, leave at 0 for auto detect; set far above real speed limit to disable it
bool idle; MyMovingAverage runtime = new MyMovingAverage(5,5);
Vector3D speedGlobal, speedLocal; double power_tfo, power_tba, power_tri, power_tle, power_tup, power_tdo;
double[] ramp = {0,0,0,0,0,0}; bool autospeedlimit; int inittick; bool toocomplex=false;

//block lists
List<IMyShipConnector> Connectors; List<IMyExtendedPistonBase> Pistons; List<IMyCargoContainer> Cargos;  //all necessary blocks used in drives
List<IMyAssembler> Assemblers; List<IMyMotorBase> Rotors; List<IMyShipMergeBlock> MergeBlocks; List<IMyShipConnector> AllConnectors;
List<IMyDoor> doors;

//cockpit
List<IMyCockpit> Controllers; public IMyCockpit ShipReference;  //needed to figure out orientation

//drives
List<MergeDrive> MergeDrives; List<PistonDrive> PistonDrives;  //classes for the drives itself, MID has no class, just group
List<MassDrive> MassDrives; List<IMyShipMergeBlock> MergeBlocksMID = new List<IMyShipMergeBlock>();     

//controllers
PID_Controller myControllerX, myControllerY, myControllerZ;     //controllers in 3 axis for inertial dampening


//The manager programm, creates a class for each drive
//All following methods are only run once after recompile
public Program() 
{ 
    Echo("Initializing...");
    MergeDrives = new List<MergeDrive>();
    MassDrives = new List<MassDrive>();
    PistonDrives = new List<PistonDrive>();

    if(mode=="auto" | mode=="hover") idle=false;    //turns drive on by default in auto and hover mode
    else idle=true;     //turns drive off by default in manual mode
    
    if(speedlimit==0) {autospeedlimit=true; speedlimit=100; }   //determines if speedlimit should be set on auto or is manual
    else {autospeedlimit=false;}
    
    Connectors = new List<IMyShipConnector>(); //needs to init connectors here so as to not remove them at any new init
    
    inittick=1;
    
    Init();     //Initialize drives and sort blocks into class objects
    Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update100; //updates script every tick and additionally every 100 ticks to detect cockpit
}

void EmergencyInit()
{
    Echo("EmergencyInit");
    //regenerate all lists for drives, since there are faulty ones
    IMyCockpit reftemp = ShipReference;
    MergeDrives = new List<MergeDrive>();
    MassDrives = new List<MassDrive>();
    PistonDrives = new List<PistonDrive>();
    MergeBlocksMID = new List<IMyShipMergeBlock>();
    Init(); //init only adds drives, doesn't reinitialize
    ShipReference = reftemp;    //for some reason script can't find cockpit after emergencyinit (literally no idea, so workaround)
}

void Init()
{
    FindController();   //Searches for a Cockpit etc. to use as orientation reference
    
    speedGlobal = new Vector3D(0,0,0);
    speedLocal = new Vector3D(0,0,0);
    
    //creates 3 new PID_controllers for inertial dampening, values are gain for proportional, integral and differential
    myControllerX = new PID_Controller(0.04,0.2,0.1);   
    myControllerY = new PID_Controller(0.04,0.2,0.1);
    myControllerZ = new PID_Controller(0.04,0.2,0.1);
    
    //pile up all necessary blocks
    MergeBlocks = new List<IMyShipMergeBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyShipMergeBlock>(MergeBlocks);
    
    Rotors = new List<IMyMotorBase>();
    GridTerminalSystem.GetBlocksOfType<IMyMotorBase>(Rotors);
    
    Pistons = new List<IMyExtendedPistonBase>();
    GridTerminalSystem.GetBlocksOfType<IMyExtendedPistonBase>(Pistons);
    
    Cargos = new List<IMyCargoContainer>();
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(Cargos);
    
    for(int i = Cargos.Count-1; i>-1; i--)
    {
        if(Cargos[i].BlockDefinition.SubtypeId.ToString() == "LargeBlockLockerRoomCorner" || Cargos[i].BlockDefinition.SubtypeId.ToString() == "LargeBlockLockerRoom" || Cargos[i].BlockDefinition.SubtypeId.ToString() == "LargeBlockLockers") 
        {
            Cargos.RemoveAt(i); //remove lockers and shit
        }
    }
    
    Assemblers = new List<IMyAssembler>();
    GridTerminalSystem.GetBlocksOfType<IMyAssembler>(Assemblers);
    
    AllConnectors = new List<IMyShipConnector>();    //connectors are later sorted into the proper connector list, but only if fitting
    GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(AllConnectors);
    
    doors = new List<IMyDoor>();
    GridTerminalSystem.GetBlocksOfType<IMyDoor>(doors);
    
    //these methods find the drives and create classes
    switch(inittick)
    {
            case 1:
                InitDampeners();
                InitPistonDrives();
                inittick++;
                break;
            case 2:
                InitMergeDrives();
                inittick++;
                break;
            case 3:
                InitMassDrives();
                inittick=0;
                break;
    }
}

void InitMergeDrives()
{
    for(int i=0; i < MergeBlocks.Count(); i++)  //iterate the merge blocks
    {
        if(Runtime.CurrentInstructionCount > 40000) toocomplex=true;    //fail safe to prevent crashing, will then maybe fail to detect all drives
        else toocomplex=false;
        if(toocomplex==true) break;
        
        List<IMyMotorBase> RotorsTemp = new List<IMyMotorBase>();
        List<IMyShipMergeBlock> MergeTemp = new List<IMyShipMergeBlock>();
        IMyShipConnector ConnectorTemp = null;
        
        MergeTemp.Add(MergeBlocks[i]);
        for(int y = 0; y < MergeBlocks.Count(); y++)  // check for more than one merge block
        {
            if(MergeBlocks[y].CubeGrid==MergeBlocks[i].CubeGrid && y!=i)  //check if merge blocks are on top of top rotor
            {                           
                MergeTemp.Add(MergeBlocks[y]);
            }
        }
        
        if(MergeBlocks[i].CubeGrid==ShipReference.CubeGrid) continue;
        
        bool alreadyexists=false;
        for(int z=0; z<MergeDrives.Count;z++)
        {
            if(MergeDrives[z].MergeBlocks[0].EntityId==MergeTemp[0].EntityId) alreadyexists=true;   //check if already exists
        }
        if(alreadyexists==true) continue;
        
        for(int j = 0; j < Rotors.Count(); j++)
        {
            if(Rotors[j].TopGrid==MergeBlocks[i].CubeGrid)  //check if merge block is on top of any rotor
            {
                int tempindex = j;
                bool done = false;
                RotorsTemp.Add(Rotors[j]);
                
                for(int m = 0; m < AllConnectors.Count; m++)   //checks if rotor is on same grid as connector
                {
                    if(Rotors[j].CubeGrid==AllConnectors[m].CubeGrid && AllConnectors[m].CubeGrid!=ShipReference.CubeGrid)
                    {
                        if(AllConnectors[m].Status==MyShipConnectorStatus.Connected)  ConnectorTemp=AllConnectors[m].OtherConnector;
                        done=true;
                        break;
                    }
                }
                
                while(done==false)  //do this until no more rotors are in line, usually just once (until one more is found)
                {
                    int tempcount=RotorsTemp.Count();
                    for(int k=0; k<Rotors.Count(); k++)     //search for rotors until one on the main grid is found
                    {
                        if(Rotors[k].TopGrid==Rotors[tempindex].CubeGrid)   //if rotor is below previous found rotor
                        {
                            RotorsTemp.Add(Rotors[k]);
                            tempindex=k;
                            
                            for(int m = 0; m < AllConnectors.Count; m++)   //checks if rotor is on same grid as connector
                            {
                                if(Rotors[k].CubeGrid==AllConnectors[m].CubeGrid && AllConnectors[m].CubeGrid!=ShipReference.CubeGrid)
                                {
                                    if(AllConnectors[m].Status==MyShipConnectorStatus.Connected)  ConnectorTemp=AllConnectors[m].OtherConnector;
                                    done=true;
                                    break;
                                }
                            }
                        }
                        if (done==true) break;
                    }
                    if(RotorsTemp.Count()==tempcount) break;
                }
                
                if(ConnectorTemp!=null && RotorsTemp.Any() && MergeTemp.Any())
                {
                    MergeDrives.Add(new MergeDrive(RotorsTemp,MergeTemp,ConnectorTemp));     //give drive its corresponding blocks
                    
                    for(int z=0; z<Connectors.Count;z++)
                    {
                        if(Connectors[z].EntityId!=ConnectorTemp.EntityId) Connectors.Add(ConnectorTemp);    //adds the base grid connector to the proper connector list
                    }
                    
                    for(int x = 0; x < MergeDrives.Count; x++) MergeDrives[x].FigureOrientation(ShipReference);   //Makes the drives figure out their orientation and save it

                    break;  //found fitting drive, no need to iterate through the rest
                }
            }
        }   
    }
}

void InitPistonDrives()
{
    for (int i = 0; i < Pistons.Count(); i++)
    {
        if(Runtime.CurrentInstructionCount > 40000) toocomplex=true;    //fail safe to prevent crashing, will then maybe fail to detect all drives
        else toocomplex=false;
        if(toocomplex==true) break;
        
        Vector3I Distance = new Vector3I(0,0,0);
        switch(Pistons[i].Orientation.Up)
        {
            case Base6Directions.Direction.Forward: //pointing backward
                Distance.Z=-1;
                break;
            case Base6Directions.Direction.Backward:    //pointing forward
                Distance.Z=1;
                break;
            case Base6Directions.Direction.Up:  //pointing down
                Distance.Y=1;
                break;
            case Base6Directions.Direction.Down:    //pointing up
                Distance.Y=-1;
                break;
            case Base6Directions.Direction.Left:    //pointing right
                Distance.X=-1;
                break;
            case Base6Directions.Direction.Right:   //pointing left
                Distance.X=1;
                break;
        }
        
        for (int k = 0; k< doors.Count(); k++)
        {
            bool alreadyexists = false;
            if(doors[k].Position==Pistons[i].Position+(Distance*4)) //pdd with normal door and pillar
            {
                for(int z=0; z<PistonDrives.Count;z++)
                {
                    if(PistonDrives[z].Piston.EntityId==Pistons[i].EntityId) alreadyexists = true;  //checks if drive already exists
                }
                if(alreadyexists==false) PistonDrives.Add(new PistonDrive(Pistons[i]));
            }
            
            if(doors[k].Position==Pistons[i].Position+(Distance*5)) //pdd with hangar door
            {
                for(int z=0; z<PistonDrives.Count;z++)
                {
                    if(PistonDrives[z].Piston.EntityId==Pistons[i].EntityId) alreadyexists = true;  //checks if drive already exists
                }
                if(alreadyexists==false) PistonDrives.Add(new PistonDrive(Pistons[i], doors[k]));
            }
        }
    }
    for(int i = 0; i < PistonDrives.Count; i++) PistonDrives[i].FigureOrientation(ShipReference);     //Makes the drives figure out their orientation and save it
}

void InitMassDrives()
{
    //makes list of assemblers and cargo containers, both could be used as a base block
    List<IMyTerminalBlock> AssAndCargo = new List<IMyTerminalBlock>(Assemblers.Count + Cargos.Count);
    AssAndCargo.AddRange(Assemblers);
    AssAndCargo.AddRange(Cargos);
    
    for(int i =0; i<AssAndCargo.Count(); i++)
    {
        if(Runtime.CurrentInstructionCount > 40000) toocomplex=true;    //fail safe to prevent crashing, will then maybe fail to detect all drives
        else toocomplex=false;
        if(toocomplex==true) break;
        
        if(AssAndCargo[i].CubeGrid!=ShipReference.CubeGrid) continue;   //not on main grid, try next
        
        bool alreadyexists=false;
        for(int z=0; z<MassDrives.Count;z++)
        {
            if(MassDrives[z].BaseBlock.EntityId==AssAndCargo[i].EntityId) alreadyexists=true;  //checks if drive already exists
        }
        if(alreadyexists==true) continue;
        
        for(int m=0; m<Cargos.Count(); m++)
        {
            if(Cargos[m].EntityId == AssAndCargo[i].EntityId) continue; //same block, go on with next block
            if(Cargos[m].GetInventory().IsConnectedTo(AssAndCargo[i].GetInventory()))    //found fitting inventories, now searching corresponding rotors
            {
                for(int k=0; k<Rotors.Count(); k++) //first rotor
                {
                    if(Rotors[k].TopGrid==Cargos[m].CubeGrid)   //found top rotor
                    {
                        int tempindex = k;
                        bool done = false;
                        List<IMyMotorBase> RotorsTemp = new List<IMyMotorBase>();
                        RotorsTemp.Add(Rotors[k]);
                        
                        while(done==false)  //do this until no more rotors are in line, usually just once (until one more is found)
                        {
                            int tempcount = RotorsTemp.Count();
                            for(int j=0; j<Rotors.Count(); j++)     //search for more rotors until one on the main grid is found
                            {
                                if(Rotors[j].TopGrid==Rotors[tempindex].CubeGrid)
                                {
                                    RotorsTemp.Add(Rotors[j]);
                                    tempindex=j;
                                }
                                if(Rotors[j].CubeGrid==AssAndCargo[i].CubeGrid) done=true;  //break if rotor is on main grid
                            }
                            if(RotorsTemp.Count()==tempcount) break;
                        }
                        RotorsTemp.Reverse(); //so that the bottom rotor is first in the list, useful for finding orientation
                        
                        MassDrives.Add(new MassDrive(RotorsTemp, AssAndCargo[i], Cargos[m], reversepower));
                        break;
                    }
                }
                break;
            }
        }
        
    }
    for(int i = 0; i < MassDrives.Count; i++) MassDrives[i].FigureOrientation(ShipReference);     //Makes the drives figure out their orientation and save it
}

void InitDampeners()
{   
    MergeBlocksMID = new List<IMyShipMergeBlock>();
    List<IMyShipMergeBlock> MergeTemp = new List<IMyShipMergeBlock>();
    
    for(int i = 0; i < AllConnectors.Count(); i++) //iterates through all connectors that could potentially be part of a drive
    {
        if(Runtime.CurrentInstructionCount > 40000) toocomplex=true;    //fail safe to prevent crashing, will then maybe fail to detect all drives
        else toocomplex=false;
        if(toocomplex==true) break;
        
        if(AllConnectors[i].CubeGrid != ShipReference.CubeGrid) //checks if connector is NOT on same grid as controller to avoid false positive
        {
            for(int j = 0; j < MergeBlocks.Count(); j++) //iterates through all merge blocks
            {
                if(AllConnectors[i].CubeGrid==MergeBlocks[j].CubeGrid)   //checks, if they are on the same grid
                {
                    bool alreadyexists=false;
                    for(int y=0; y<MergeTemp.Count;y++)
                        if(MergeTemp[y].EntityId==MergeBlocks[j].EntityId) alreadyexists=true;     //checks if merge already found in temp list
                    for(int z=0; z<MergeBlocksMID.Count;z++)
                        if(MergeBlocksMID[z].EntityId==MergeBlocks[j].EntityId) alreadyexists=true;   //checks if merge already found in perma list
                    
                    if(alreadyexists==false)
                    {
                        MergeTemp.Add(MergeBlocks[j]); 
                    }
                }
            }
            
            if(MergeTemp.Any())
            {
                MergeBlocksMID.AddRange(MergeTemp);
                Connectors.Add(AllConnectors[i].OtherConnector);
            }
        }
    }
    List<IMyTimerBlock> Timers = new List<IMyTimerBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(Timers);
    for(int i = 0; i < Timers.Count; i++) if(Timers[i].IsFunctional && Timers[i].CustomName.Contains("_MID")) Timers[i].GetActionWithName("OnOff_Off").Apply(Timers[i]); //disables all timers, connectors now handled by script
    
}


//All methods from here on are run every tick
void Main(string args, UpdateType updateSource)
{
    if ((updateSource & UpdateType.Update100) != 0) 
    {
        inittick=1;
    }
    if(inittick==1 || inittick==2 || inittick==3)
    {
        Init();     //Additional Initialization every 100 ticks to add new drives, doesn't reinitialize the drives. Also checks for controller again
    }
    
    ErrorHandler();     //Simply Echoes a bunch of stuff to show you what you fucked up this time
    
    LockConnectors();   //auto locks connectors, so you can't accidentally unlock them with 'P', needs batteries on subgrid or else the subgrid connector is off and can't be reconnected
    
    RemoveDamagedDrives();  //checks if any drives are damaged and remove them from list
    
    for(int i=0; i<PistonDrives.Count; i++)
    {
        if(PistonDrives[i].door!=null) 
            if(PistonDrives[i].door.OpenRatio<=0.5) PistonDrives[i].door.Enabled=false;
    }
    
    //calc speed of ship, used for inertial dampening and power controlling
    speedGlobal = ShipReference.GetShipVelocities().LinearVelocity;
    speedLocal = Vector3D.TransformNormal(speedGlobal, MatrixD.Transpose(ShipReference.WorldMatrix));

    if(autospeedlimit==true) 
        if(ShipReference.GetShipSpeed()>speedlimit) speedlimit=ShipReference.GetShipSpeed();    //calculates speedlimit automatically, increases until ceiling

    SmartPower(); //calculates power automatically in all directions
    
    Argumenthandler(args);  //this looks at the current idle state, toggles it if args is toggle and fires the drives with correct logic
    
    EchoFunction();
}

void SmartPower()
{
    //Power drop off between 85% and 95% of speedlimit from 1 to 0
    //Power_t is clamped to power and 0
    power_tle=(9.5-10*(speedLocal.X/speedlimit))*power; 
    if(power_tle>power) power_tle=power;else if(power_tle<0) power_tle=0;
    
    power_tri=(9.5-10*(-speedLocal.X/speedlimit))*power; 
    if(power_tri>power) power_tri=power; else if(power_tri<0) power_tri=0;
    
    power_tup=(9.5-10*(speedLocal.Y/speedlimit))*power; 
    if(power_tup>power) power_tup=power; else if(power_tup<0) power_tup=0;
    
    power_tdo=(9.5-10*(-speedLocal.Y/speedlimit))*power; 
    if(power_tdo>power) power_tdo=power; else if(power_tdo<0) power_tdo=0;
    
    power_tba=(9.5-10*(speedLocal.Z/speedlimit))*power; 
    if(power_tba>power) power_tba=power; else if(power_tba<0) power_tba=0;
    
    power_tfo=(9.5-10*(-speedLocal.Z/speedlimit))*power; 
    if(power_tfo>power) power_tfo=power;else if(power_tfo<0) power_tfo=0;
    
    //power ramp up after inactive, takes 10 ticks from 20% to 100% power, multiplies with power_t
    //1=le, 2=ri, 3=up, 4=do, 5=ba, 6=fo
    for(int i=0; i<6;i++)
    {
        if(ramp[i]<1)
        {
            ramp[i]=ramp[i]+0.08;
        }
        
        //check if any active drives in this direction
        bool active = false;
        for(int k = 0; k < MergeDrives.Count; k++) 
                if(MergeDrives[k].orientation==NumberToOrientation(i,false))
                    if(MergeDrives[k].active==true) active=true;
        for(int k = 0; k < MassDrives.Count; k++) 
        {
            if(MassDrives[k].orientation==NumberToOrientation(i,false))
            {
                if(MassDrives[k].active==true && MassDrives[k].reverse==false) active=true;
            }
            if(MassDrives[k].orientation==NumberToOrientation(i,true))
            {
                if(MassDrives[k].active==true && MassDrives[k].reverse==true) active=true;
            }
        }
        for(int k = 0; k < PistonDrives.Count; k++) 
            if(PistonDrives[k].orientation==NumberToOrientation(i,false))
                if(PistonDrives[k].active==true) active=true;
        
        if(active==false) ramp[i]=0.2;  //reset ramp if all drives in direction are inactive
    }
    power_tle *= ramp[0]; power_tri *= ramp[1]; power_tup *= ramp[2]; power_tdo *= ramp[3]; power_tba *= ramp[4]; power_tfo *= ramp[5];
}

string NumberToOrientation(int p, bool opposite)
{
    if(opposite==true)
    {
        switch (p)
        {
            case 0:
                return "right";
            case 1:
                return "left";
            case 2:
                return "down";
            case 3:
                return "up";
            case 4:
                return "forward";
            case 5:
                return "backward";
            default:
                return "";
        }
    }
    else
    {
        switch (p)
        {
            case 0:
                return "left";
            case 1:
                return "right";
            case 2:
                return "up";
            case 3:
                return "down";
            case 4:
                return "backward";
            case 5:
                return "forward";
            default:
                return "";
        }
    }
}

void EchoFunction()
{   
    Echo("Instruction Count: " + Runtime.CurrentInstructionCount.ToString());  //debug, shows current instructions
    runtime.Enqueue((float) Runtime.LastRunTimeMs);
    Echo("Runtime: " + string.Format("{0:0.000}", runtime.Avg) );
    Echo("Speedlimit: " + string.Format("{0:0.0}", speedlimit));
    Echo("ShipController: " + ShipReference.CustomName);
    Echo("Dampeners found: " + (MergeBlocksMID.Count()).ToString());
    Echo("Merge Drives found: " + MergeDrives.Count().ToString());
    Echo("Piston Drives found: " + PistonDrives.Count().ToString());
    Echo("Mass Shifting Drives found: " + MassDrives.Count().ToString());
    Echo("");
    for(int i=0; i<MergeDrives.Count; i++) Echo("Merge Drive " + (i+1).ToString() + ": " + MergeDrives[i].orientation);
    for(int i=0; i<PistonDrives.Count; i++) Echo("Piston Drive " + (i+1).ToString() + ": " + PistonDrives[i].orientation);
    for(int i=0; i<MassDrives.Count; i++) Echo("Mass Drive " + (i+1).ToString() + ": " + MassDrives[i].orientation);
}

void ErrorHandler()
{
    if(toocomplex==true) Echo("Too many drives, can't detect all!");
    if(mode!="auto" && mode!="manual") Echo("Incorrect mode!");
    if(power<0) Echo("Incorrect power!");
    if(power>1) Echo("Overclocked power!");
    if(reversepower<0) Echo("Incorrect reversepower!");
    if(reversepower<0) Echo("Overclocked reversepower!");
    if(dampeningstrength<0) Echo("Incorrect dampeningstrength!");
    if(dampeningstrength>1) Echo("Overclocked dampeningstrength!");
    if(speedlimit<0) Echo("Incorrect speedlimit!");
}

void FindController()
{
    Controllers = new List<IMyCockpit>();
    GridTerminalSystem.GetBlocksOfType<IMyCockpit>(Controllers);
    for(int i=0; i<Controllers.Count();i++)
    {
        if (Controllers[i].IsMainCockpit) {ShipReference = Controllers[i]; return;}     //first checks if there is any main cockpit
    }
    for(int i=0; i<Controllers.Count();i++)
    {
        if (Controllers[i].IsUnderControl) {ShipReference = Controllers[i]; return;}    //if not, checks if there is any controlled cockpit
    }
    if(Controllers.Any()) ShipReference = Controllers[0];   //if not, just uses the first one it finds
    else { Echo("No Cockpit found!"); Echo(""); ShipReference = null; }
}

void RemoveDamagedDrives()
{
    for(int i=MergeDrives.Count - 1; i > -1; i--)    //iterates backwards to prevent skipping of index
    {
        if(MergeDrives[i].CheckDestruction()==true) MergeDrives.RemoveAt(i);  //drives respond with true if they are damaged
    }
    
    for(int i=PistonDrives.Count - 1; i > -1; i--)   //iterates backwards to prevent skipping of index
    {
        if(PistonDrives[i].CheckDestruction()==true) PistonDrives.RemoveAt(i);  //drives respond with true if they are damaged
    }
    
    for(int i=MassDrives.Count - 1; i > -1; i--)     //iterates backwards to prevent skipping of index
    {
        if(MassDrives[i].CheckDestruction()==true) MassDrives.RemoveAt(i);  //drives respond with true if they are damaged
    }
    
    for(int i=MergeBlocksMID.Count -1; i>-1;i--)
    {
        bool isdead=false;
        if(MergeBlocks[i]==null || MergeBlocks[i].CubeGrid.GetCubeBlock(MergeBlocks[i].Position) == null) isdead=true;
        else if(MergeBlocks[i].IsFunctional==false) isdead=true;
        
        if(isdead==true) MergeBlocksMID.RemoveAt(i);
    }
}

void Argumenthandler(string args)
{
    
    //this allows you to check after a run which drives were activated
    for(int i = 0; i < MergeDrives.Count; i++) MergeDrives[i].active=false;
    for(int i = 0; i < MassDrives.Count; i++) MassDrives[i].active=false;
    for(int i = 0; i < PistonDrives.Count; i++) PistonDrives[i].active=false;
    
    if(idle==true)    //argument handler in default state
    {
        Echo("Idling");
        for(int i=0; i<MergeDrives.Count;i++) 
            for(int k=0; k<MergeDrives[i].MergeBlocks.Count;k++) MergeDrives[i].MergeBlocks[k].Enabled=false;    //turn off merge blocks of drive when drive is off
        for(int i=0; i<MergeBlocksMID.Count;i++) MergeBlocksMID[i].Enabled=false;       //turn off merge blocks of MIDs when drive is off
        for(int i=0; i<PistonDrives.Count;i++) PistonDrives[i].StopDrive();             //stops piston drives, pulls out piston
                                                                                        //no need to stop mass drive, it stops itself
       
        
        if(args=="toggle") idle=false;  //switch state
    }
    else           //argument handler while drive is running
    {
        Echo("Firing");
        if(args=="toggle") idle=true;   //switch state
        
        
        //this passage handles the MID Inertial Dampeners
        double speed = ShipReference.GetShipSpeed();
        int index = (int) ( (speed/(speedlimit*0.75))*MergeBlocksMID.Count() );
        
        if (index>MergeBlocksMID.Count()) index = MergeBlocksMID.Count();
        for(int i = 0; i < index; i++)
        {
            MergeBlocksMID[i].Enabled=true;
        }
        for(int i = index; i <MergeBlocksMID.Count ; i++)
        {
            MergeBlocksMID[i].Enabled=false;
        }
        //from here on only real drives
        
        
        if(mode=="auto")    //drive handled by wasd
        {
            if(ShipReference.MoveIndicator.X==1)    //WASD Logic, fires drives corresponding to keyboard input, might not work with gamepad
            {
                HandlePistonDrives(power_tri, "right");
                HandlePistonDrives(0, "left");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="right") drive.FireDrive(power_tri,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="left") drive.FireDrive(power_tri,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="right") drive.FireDrive(power_tri);
            }
            if(ShipReference.MoveIndicator.X==-1) 
            {
                HandlePistonDrives(power_tle, "left");
                HandlePistonDrives(0, "right");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="left") drive.FireDrive(power_tle,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="right") drive.FireDrive(power_tle,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="left") drive.FireDrive(power_tle);
            }
            if(ShipReference.MoveIndicator.X==0)
            {
                HandlePistonDrives(0, "right");
                HandlePistonDrives(0, "left");
            }
            if(ShipReference.MoveIndicator.Y==1)    
            {
                HandlePistonDrives(power_tup, "up");
                HandlePistonDrives(0, "down");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="up") drive.FireDrive(power_tup,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="down") drive.FireDrive(power_tup,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="up") drive.FireDrive(power_tup);
            }
            if(ShipReference.MoveIndicator.Y==-1) 
            {
                HandlePistonDrives(power_tdo, "down");
                HandlePistonDrives(0, "up");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="down") drive.FireDrive(power_tdo,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="up") drive.FireDrive(power_tdo,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="down") drive.FireDrive(power_tdo);
            }
            if(ShipReference.MoveIndicator.Y==0)
            {
                HandlePistonDrives(0, "up");
                HandlePistonDrives(0, "down");      
            }
            if(ShipReference.MoveIndicator.Z==1)    
            {
                HandlePistonDrives(power_tba, "backward");
                HandlePistonDrives(0, "forward");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="backward") drive.FireDrive(power_tba,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="forward") drive.FireDrive(power_tba,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="backward") drive.FireDrive(power_tba);
            }
            if(ShipReference.MoveIndicator.Z==-1) 
            {
                HandlePistonDrives(power_tfo, "forward");
                HandlePistonDrives(0, "backward");
                foreach(MassDrive drive in MassDrives)
                    if(drive.orientation=="forward") drive.FireDrive(power_tfo,false);
                foreach(MassDrive drive in MassDrives) 
                    if(drive.orientation=="backward") drive.FireDrive(power_tfo,true);
                foreach(MergeDrive drive in MergeDrives) 
                    if(drive.orientation=="forward") drive.FireDrive(power_tfo);
            }
            if(ShipReference.MoveIndicator.Z==0)
            {
                HandlePistonDrives(0, "backward");
                HandlePistonDrives(0, "forward");
            }
            
            if(dampeningstrength>0)     //dampeners can be turned off manually in the script
            {
                if(ShipReference.DampenersOverride) //if inertial dampeners are turned on ('Z' key), dampen speed in all unused axis
                {
                    if(ShipReference.GetShipSpeed()>0.1) InertialDampen();  //only dampen if speed above 0.1 m/s, it's useless below anyways
                }
            }
        }
        else if(mode=="manual")     //drive handled by arguments
        {
            foreach(MergeDrive drive in MergeDrives) if(drive.orientation=="forward") drive.FireDrive(power); //fires only the forwards drives in manual mode if toggled on
            foreach(MassDrive drive in MassDrives) if(drive.orientation=="forward") drive.FireDrive(power,false); //fires only the forwards drives in manual mode if toggled on
            foreach(MassDrive drive in MassDrives) if(drive.orientation=="backward") drive.FireDrive(power,true);
            HandlePistonDrives(power, "forward");
        }
        else if(mode=="hover")
        {
        }
    }
}

void HandlePistonDrives(double ppower, string porientation)
{
    if(ppower==0)   //stops drives when power is 0
    {
        foreach(PistonDrive drive in PistonDrives) 
        {
            if(drive.orientation==porientation) drive.StopDrive();
        }
    }
    else    //else fires drives when power is >0
    {
        foreach(PistonDrive drive in PistonDrives) 
        {
            if(drive.orientation==porientation) drive.FireDrive(ppower);
        }
    }
}

void InertialDampen()
{
    //inertial dampening, when no input on axis, fire drives to reduce speed in axis to zero
    
    //creates vector with Controller values based on the local ship speed 
    Vector3D PID_Local = new Vector3D(myControllerX.CalcValue(speedLocal.X),myControllerY.CalcValue(speedLocal.Y),myControllerZ.CalcValue(speedLocal.Z));
    
    if(ShipReference.MoveIndicator.X==0)    //right left axis, only dampen when axis is not used by WASD
    {
        if(PID_Local.X<0)
        {
            //adjusts power/retraction distance by Control value from PID_Controller
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="right") drive.FireDrive(dampeningstrength*power_tri*Math.Abs(PID_Local.X));
            HandlePistonDrives(dampeningstrength*power_tri*Math.Abs(PID_Local.X), "right");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="right") drive.FireDrive(dampeningstrength*power_tri*Math.Abs(PID_Local.X),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="left") drive.FireDrive(dampeningstrength*power_tri*Math.Abs(PID_Local.X),true);
        }
        else if(PID_Local.X>0)
        {
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="left") drive.FireDrive(dampeningstrength*power_tle*Math.Abs(PID_Local.X));
            HandlePistonDrives(dampeningstrength*power_tle*Math.Abs(PID_Local.X), "left");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="left") drive.FireDrive(dampeningstrength*power_tle*Math.Abs(PID_Local.X),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="right") drive.FireDrive(dampeningstrength*power_tle*Math.Abs(PID_Local.X),true);
        }
    }
    if(ShipReference.MoveIndicator.Y==0)    //up down axis
    {
        if(PID_Local.Y>0)
        {
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="down") drive.FireDrive(dampeningstrength*power_tdo*Math.Abs(PID_Local.Y));
            HandlePistonDrives(dampeningstrength*power_tdo*Math.Abs(PID_Local.Y), "down");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="down") drive.FireDrive(dampeningstrength*power_tdo*Math.Abs(PID_Local.Y),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="up") drive.FireDrive(dampeningstrength*power_tdo*Math.Abs(PID_Local.Y),true);
        }
        else if(PID_Local.Y<0)
        {
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="up") drive.FireDrive(dampeningstrength*power_tup*Math.Abs(PID_Local.Y));
            HandlePistonDrives(dampeningstrength*power_tup*Math.Abs(PID_Local.Y), "up");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="up") drive.FireDrive(dampeningstrength*power_tup*Math.Abs(PID_Local.Y),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="down") drive.FireDrive(dampeningstrength*power_tup*Math.Abs(PID_Local.Y),true);
        }
    }
    if(ShipReference.MoveIndicator.Z==0)    //forward backward axis
    {
        if(PID_Local.Z<0)
        {
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="backward") drive.FireDrive(dampeningstrength*power_tba*Math.Abs(PID_Local.Z));
            HandlePistonDrives(dampeningstrength*power_tba*Math.Abs(PID_Local.Z), "backward");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="backward") drive.FireDrive(dampeningstrength*power_tba*Math.Abs(PID_Local.Z),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="forward") drive.FireDrive(dampeningstrength*power_tba*Math.Abs(PID_Local.Z),true);
        }
        else if(PID_Local.Z>0)
        {
            foreach(MergeDrive drive in MergeDrives) 
                if(drive.orientation=="forward") drive.FireDrive(dampeningstrength*power_tfo*Math.Abs(PID_Local.Z));
            HandlePistonDrives(dampeningstrength*power_tfo*Math.Abs(PID_Local.Z), "forward");
            foreach(MassDrive drive in MassDrives)
                if(drive.orientation=="forward") drive.FireDrive(dampeningstrength*power_tfo*Math.Abs(PID_Local.Z),false);
            foreach(MassDrive drive in MassDrives) 
                if(drive.orientation=="backward") drive.FireDrive(dampeningstrength*power_tfo*Math.Abs(PID_Local.Z),true);
        }
    }
}

void LockConnectors()
{
    bool emergencyInit=false;   //used to inialize again incase any connectors have been disconnected
    for(int i = 0; i < Connectors.Count; i++) 
    {
        if(Connectors[i]!=null && Connectors[i].CubeGrid.GetCubeBlock(Connectors[i].Position) != null)
        {
            if(Connectors[i].IsFunctional==false)
            {
                emergencyInit=true; //Emergency: Connectors not functioning anymore: needs new initialization
                Connectors.RemoveAt(i);
            }
            else if(Connectors[i].Status==MyShipConnectorStatus.Unconnected || Connectors[i].Status==MyShipConnectorStatus.Connectable) 
            {
                Connectors[i].Connect();    //Tries to connect connectors again/ connectors could be null because they are on the disconnected subgrid
                //Connectors[i].GetActionWithName("Lock").Apply(Connectors[i]);
                emergencyInit=true; //Emergency: Connectors not locked anymore: needs new initialization
            }
        }
    }
    if(emergencyInit==true) EmergencyInit(); //initialize again, since connectors have been disconnected
}


//class for all merge drives (MPD-1)
public class MergeDrive     
{
    private List<IMyMotorBase> Rotors; public List<IMyShipMergeBlock> MergeBlocks; public IMyShipConnector Connector; int tick, wiggle; public string orientation;
    public bool active;
    
    public MergeDrive(List<IMyMotorBase> protors, List<IMyShipMergeBlock> pmerge, IMyShipConnector pconnector) //contructor to asign rotors and merge blocks and figure out orientation
    {
        wiggle=0;
        orientation="not defined";
        tick=3;     //drive needs 2 ticks to fire, 1 for extending rotor, 1 for retracting
        Rotors = protors;
        MergeBlocks = pmerge;
        Connector = pconnector;
        
        retract(Rotors, (float) 0.3);   //retract half the way for safety
    }
    
    public void StopRotation()
    {
        for(int i = 0; i < Rotors.Count; i++) Rotors[i].Enabled=false;
    }
    private void StartRotatation()
    {
        for(int i = 0; i < Rotors.Count; i++) Rotors[i].Enabled=true;
    }

    public bool CheckDestruction()
    {
        bool isdead = false;
        for(int i=0; i<Rotors.Count;i++) if(Rotors[i]==null || Rotors[i].CubeGrid.GetCubeBlock(Rotors[i].Position) == null) isdead=true;
        for(int i=0; i<MergeBlocks.Count;i++) if(MergeBlocks[i]==null || MergeBlocks[i].CubeGrid.GetCubeBlock(MergeBlocks[i].Position) == null) isdead=true;
        if(Connector==null || Connector.CubeGrid.GetCubeBlock(Connector.Position) == null) isdead=true;    //blocks are missing, drive is dead
        
        if(Connector.IsFunctional==false) isdead=true; //blocks are broken, drive also dead
        for(int i=0; i<Rotors.Count;i++) if(Rotors[i].IsFunctional==false) isdead=true;
        for(int i=0; i<MergeBlocks.Count;i++) if(MergeBlocks[i].IsFunctional==false) isdead=true;
        return isdead;   //default response, drive is working fine
    }

    public void FigureOrientation(IMyCockpit ShipController)
    {
        //if(Connector.Orientation.Forward==Base6Directions.GetOppositeDirection(ShipController.Orientation.Forward)) orientation="forward";
        //if(Connector.Orientation.Forward==Base6Directions.GetOppositeDirection(ShipController.Orientation.Up)) orientation="up";
        //if(Connector.Orientation.Forward==Base6Directions.GetOppositeDirection(ShipController.Orientation.Left)) orientation="left";
        //if(Connector.Orientation.Forward==ShipController.Orientation.Forward) orientation="backward";   //Connector is pointing in direction of thrust, so pointing forward=backward thrust
        //if(Connector.Orientation.Forward==ShipController.Orientation.Up) orientation="down";
        //if(Connector.Orientation.Forward==ShipController.Orientation.Left) orientation="right";
        
        if(Connector.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Forward)) orientation="backward";
        if(Connector.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Up)) orientation="down";
        if(Connector.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Left)) orientation="right";
        if(Connector.Orientation.Up==ShipController.Orientation.Forward) orientation="forward";
        if(Connector.Orientation.Up==ShipController.Orientation.Up) orientation="up";
        if(Connector.Orientation.Up==ShipController.Orientation.Left) orientation="left";
    }
    
    private void Wiggle()
    {
        if(wiggle<6) 
        {
            wiggle++; 
            for(int i=0; i<MergeBlocks.Count;i++) MergeBlocks[i].Enabled=true;
        }
        else 
        {
            wiggle=0; 
            for(int i=0; i<MergeBlocks.Count;i++) if(MergeBlocks[i].IsConnected==false) MergeBlocks[i].Enabled=false;
        }
    }
    
    public void FireDrive(double pMag)      //overload of FireDrive with adjusted retraction distance for less power, is used for inertial dampening, also needs axis to give to pid_controller
    {
        active=true;
        if(tick>3) tick--;
        switch(tick)
        {
            case 3:
                for(int i=0; i<MergeBlocks.Count;i++) MergeBlocks[i].Enabled=true;
                Wiggle();   //helps link merge blocks faster
                bool tempconnected = true;
                for(int i=0; i<MergeBlocks.Count;i++) if(MergeBlocks[i].IsConnected==false) tempconnected=false;
                
                if(Rotors[0].GetValue<float>("Displacement")>-0.35) 
                {
                    tick--;     //skips if rotors are not fully retracted for some reason (and then rully retract them)
                    break;
                }
                
                if(tempconnected == true)        //Drive shoots out if all merge blocks are linked
                {
                   tick--;
                   extend(Rotors, (float) (0.5 * pMag));    //extend rotors with pMag as percentage
                }
                break;
                
            case 2:
                for(int i=0; i<MergeBlocks.Count;i++) MergeBlocks[i].Enabled=false;   //turn off merge block, else backwards acceleration
                retract(Rotors, (float) ( (Rotors[0].GetValue<float>("Displacement")+0.4)/2));  //retract half the remaining way
                tick--; //reset firing cycle
                break;
                
            case 1:
                retract(Rotors, (float) ((Rotors[0].GetValue<float>("Displacement")+0.4)/2));   //retract half the remaining way, split over two ticks for safety
                tick=3; //reset firing cycle
                break;
        }
    }

    private void extend (List<IMyMotorBase> roto, float travel)
    {
        for(int i = 0; i < roto.Count; i++) 
        {
            roto[i].SetValue("Displacement", roto[i].GetValue<float>("Displacement")+ (float) travel);      //extends by travel
        }
    }

    private void retract (List<IMyMotorBase> roto, float travel)
    {
        for(int i = 0; i < roto.Count; i++) 
        {
            roto[i].SetValue("Displacement", roto[i].GetValue<float>("Displacement")- (float) travel);     //retracts by travel
        }   
    }  
}

//class for all piston drives (PDD-3) and (PDD-1)
public class PistonDrive
{
    public bool active; public IMyDoor door; bool hangar;
    public string orientation; public IMyExtendedPistonBase Piston;
    
    public PistonDrive(IMyExtendedPistonBase pPiston) //contructor to asign piston
    {
        Piston = pPiston;
        hangar=false;
        orientation="not defined";
        
        if(Piston.Top==null) Piston.GetActionWithName("Add Top Part").Apply(Piston);
        
        //move drive into start position, maxlimit is adjusted in startdrive method on the fly
        Piston.MaxLimit=(float) 2.1;
        Piston.MinLimit=(float) 2.1;
        if(Piston.CurrentPosition<2.1) Piston.Velocity=5;
        if(Piston.CurrentPosition>2.1) Piston.Velocity=-5;
        
        //Piston.MaxImpulseAxis=(float) 5000000;        //this property is inaccesible for some reason
        //Piston.IncreaseMaxImpulseAxis();              //method is also broken :(
    }
    
    public PistonDrive(IMyExtendedPistonBase pPiston, IMyDoor pdoor) //overload with door, used for hangar door version
    {
        Piston = pPiston;
        door = pdoor;
        hangar=true;
        orientation="not defined";
        
        if(Piston.Top==null) Piston.GetActionWithName("Add Top Part").Apply(Piston);
        
        //move drive into start position, maxlimit is adjusted in startdrive method on the fly
        Piston.MaxLimit=(float) 4.65;
        Piston.MinLimit=(float) 4.65;
        if(Piston.CurrentPosition<4.65) Piston.Velocity=5;
        if(Piston.CurrentPosition>4.65) Piston.Velocity=-5;
        
        door.CloseDoor();
        
        //Piston.MaxImpulseAxis=(float) 5000000;        //this property is inaccesible for some reason
        //Piston.IncreaseMaxImpulseAxis();              //method is also broken :(
    }
    
    public void FigureOrientation(IMyCockpit ShipController)
    {
        if(Piston.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Forward)) orientation="forward";
        if(Piston.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Up)) orientation="up";
        if(Piston.Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Left)) orientation="left";
        if(Piston.Orientation.Up==ShipController.Orientation.Forward) orientation="backward";   //Piston is pointing in direction of thrust, so pointing forward=backward thrust
        if(Piston.Orientation.Up==ShipController.Orientation.Up) orientation="down";
        if(Piston.Orientation.Up==ShipController.Orientation.Left) orientation="right";
    }
    
    public void FireDrive(double ppower)
    {
        active=true;
        if(hangar==false)
        {
            if(Piston.MaxLimit>2.31+0.19*ppower) Piston.Velocity=-5;    //if limit is lowered, move back piston
            else Piston.Velocity=5;     //if limit is the same or higher, move out piston
        
            Piston.MaxLimit=(float) (2.31+0.19*ppower); //calc new maxlimit
        }
        if(hangar==true)
        {
            if(Piston.MaxLimit>4.95+0.15*ppower) Piston.Velocity=-5;    //if limit is lowered, move back piston
            else Piston.Velocity=5;     //if limit is the same or higher, move out piston
        
            Piston.MaxLimit=(float) (4.95+0.15*ppower); //calc new maxlimit
        }
    }
    
    public void StopDrive()
    {
        Piston.Velocity=-5;
    }
    
    public bool CheckDestruction()
    {
        bool isdead = false;
        if(Piston==null || Piston.CubeGrid.GetCubeBlock(Piston.Position) == null) isdead=true;
        else if(Piston.IsFunctional==false) isdead=true; //blocks are broken, drive also dead
        return isdead;   //default response, drive is working fine
    }
}

//class for all mass shift drives (MSD-2-21) 
public class MassDrive
{
    public bool active; double revpower; public bool reverse; double safety; int RotorCount;
    private List<IMyMotorBase> Rotors; int tick; public string orientation; int deadmass;
    public IMyTerminalBlock BaseBlock, TopBlock; double maxdisplacement;
       
    public MassDrive(List<IMyMotorBase> pRotors, IMyTerminalBlock pBase, IMyTerminalBlock pTop, double prevpower) //contructor to asign rotors and merge blocks and figure out orientation
    {
        revpower = prevpower;
        orientation="not defined";
        tick=2;     //drive needs 2 ticks to fire, 1 for extending rotor, 1 for retracting
        Rotors = pRotors;
        BaseBlock=pBase;
        TopBlock=pTop;
        RotorCount = Rotors.Count();
        deadmass = 6000;    //6 tons will stay in the moving cargo container to reach resonant frequency at 60 ticks per second, seems to work pretty well for all configs of rotors
        
        maxdisplacement = FigureMaxDisplacement();
        
        if(maxdisplacement > 0.5) //is large rotor
        {
            safety = 0.5;     //some rotors can't handle full power
        }
        else //is small rotor, or large rotor with small head
        {
            safety = 1;    //others can, like small adv rots
        }
        
        extend(Rotors, (float) (maxdisplacement*safety));   //extend half the way, to prevent big boom on activation
    }
    
    public float FigureMaxDisplacement()
    {
        float tempold = Rotors[0].GetValue<float>("Displacement");
        Rotors[0].SetValue("Displacement",(float) 100);
        float temphigh = Rotors[0].GetValue<float>("Displacement");
        Rotors[0].SetValue("Displacement", (float) -100);
        float templow = Rotors[0].GetValue<float>("Displacement");
        Rotors[0].SetValue("Displacement",(float) tempold);
        
        return temphigh-templow;
    }
    
    public void FireDrive(double pMag, bool preverse)        //overload of FireDrive with adjusted retraction distance for less power, is used for inertial dampening, also needs axis to give to pid_controller
    {
        active=true;
        if(preverse!=reverse) { if(tick==2) {tick=1;} else tick=2;}     //flip ticks, if reverse mode changes
        
        reverse=preverse;
        
        int totalmass = (int) TopBlock.GetInventory().CurrentMass;
        
        if(preverse==true)  //drive is working backwards
        {
            switch(tick)
            {
                case 2:
                    transfer(TopBlock.GetInventory(),BaseBlock.GetInventory(), totalmass-deadmass); 
                    retract(Rotors, (float) (maxdisplacement * safety * pMag * revpower));    //retract rotors with pMag as percentage (0.5 because drive cant handle full power)
                    tick--;
                    break;
                case 1: 
                    transfer(BaseBlock.GetInventory(),TopBlock.GetInventory(), 999999999);
                    extend(Rotors, (float) (maxdisplacement));   //extend all the way
                    tick=2;
                    break;
            }
        }
        else    //drive is working normal
        {
            switch(tick)
            {
                case 2:
                    transfer(BaseBlock.GetInventory(),TopBlock.GetInventory(), 999999999);
                    retract(Rotors, (float) (maxdisplacement * safety *pMag));     //retract rotors with pMag as percentage (0.25 because drive cant handle full power)
                    tick--;
                    break;
                case 1: 
                    transfer(TopBlock.GetInventory(),BaseBlock.GetInventory(), totalmass-deadmass);     //tranfer all back
                    extend(Rotors, (float) (maxdisplacement));   //extend all the way
                    tick=2;
                    break;
            }
        }
    }
    
    void transfer(IMyInventory a, IMyInventory b, int pamount) 
    {
        a.TransferItemTo(b,0,0,true,pamount);     
    }

    public bool CheckDestruction()
    {
        bool isdead = false;
        if(Rotors.Count!=RotorCount) isdead=true; 
        else {for(int i=0; i<Rotors.Count; i++){ if(Rotors[i].CubeGrid.GetCubeBlock(Rotors[i].Position)==null) isdead=true;}}
        
        if(isdead==false) 
        {
            if(BaseBlock==null || BaseBlock.CubeGrid.GetCubeBlock(BaseBlock.Position) == null) isdead=true;
            else if(TopBlock==null || TopBlock.CubeGrid.GetCubeBlock(TopBlock.Position) == null) isdead=true;    //blocks are missing, drive is dead
            else if( BaseBlock.IsFunctional==false || TopBlock.IsFunctional==false) isdead=true;            //blocks are broken, drive also dead
            else {for(int i=0; i<Rotors.Count; i++) {if(Rotors[i].IsFunctional==false) isdead=true;}}
        }
        return isdead;   //default response, drive is working fine
    }

    public void FigureOrientation(IMyCockpit ShipController)
    {
        if(Rotors[0].Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Forward)) orientation="forward";
        if(Rotors[0].Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Up)) orientation="up";
        if(Rotors[0].Orientation.Up==Base6Directions.GetOppositeDirection(ShipController.Orientation.Left)) orientation="left";
        if(Rotors[0].Orientation.Up==ShipController.Orientation.Forward) orientation="backward";   //Connector is pointing in direction of thrust, so pointing forward=backward thrust
        if(Rotors[0].Orientation.Up==ShipController.Orientation.Up) orientation="down";
        if(Rotors[0].Orientation.Up==ShipController.Orientation.Left) orientation="right";
    }
    
    private void extend (List<IMyMotorBase> roto, float travel)
    {
        for(int i = 0; i < roto.Count; i++) 
        {
            roto[i].SetValue("Displacement", roto[i].GetValue<float>("Displacement")+ (float) travel);      //extends by travel
        }
    }

    private void retract (List<IMyMotorBase> roto, float travel)
    {
        for(int i = 0; i < roto.Count; i++) 
        {
            roto[i].SetValue("Displacement", roto[i].GetValue<float>("Displacement")- (float) travel);     //retracts by travel
        }   
    }  
}

//class for Controlling velocity for inertial dampening, is a parallel PID controller (I think)
public class PID_Controller
{
    private double error, ki, kp, kd, i;
    
    public PID_Controller(double proportional, double integral, double derivative)
    {
        ki= integral;
        kp = proportional;
        kd = derivative;
        error=0;
        i=0;
    }
    
    public double CalcValue(double perror)
    {
        double p = kp * perror;         //simple error as factor    |   all values always positive, separated by direction anyways
        if(p>2) p=2;    //clamp proportional value, priority over all others
        if(p<-2) p=-2;
        
        i += ki * perror;           //sum of all past errors, should converge on zero
        if(i>0.08) i=0.08;  //clamp integral value, somewhat priorized over differential
        if(i<-0.08) i=-0.08;
        
        double d = kd * (perror-error);     //change in error   | can get negative if error is falling fast
        if(d>1) d=1;    //clamp differential value
        if(d<-1) d=-1;
        
        
        error=perror;       //save error for derivative of next iteration
        
        double temp = p + i + d;
        if(temp > 1) temp = 1;
        if(temp < -1) temp = -1;    //clamp return
        
        
        return (temp);      //return sum of factors
    }
}