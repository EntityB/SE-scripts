// ---------------------------------------------------------------------------//
// --- Information Section ---------------------------------------------------//
// ---------------------------------------------------------------------------//
// Originally from http://steamcommunity.com/sharedfiles/filedetails/?id=379969347
// Author(s) : Heisenboy http://steamcommunity.com/profiles/76561198010919614/myworkshopfiles/?appid=244850
//    Phil Allen phil@hilands.com
// Last Edited By: phil@hilands.com
// Version : 2019040301
//========================================================================// 
// Variables to Edit
//========================================================================// 
string strProjectorNameContains = "[BP]"; // name of the projector
const string strDisplayNameContains = "[BPLCD]"; // name of the text panel/lcd
int intRescanTicks = 10; // ticks till cargo/projector components are rescanned.
//The projector doesn't tell how many Interior wall is in
//  the blueprint. You'll have to enter it manually
int numberOfInteriorWall = 0;
bool boolDebug = false; // debug data sent to echo. sends lots of data....
//========================================================================// 
// Dictionaries of component info.
//   Add new blocks, change component totals and volumes etc here.
//   Would appreciate if you submitted changes to the discussions thread
//     in the steam workshop so I can update it too!
//========================================================================// 
// Component Volumes
Dictionary<string, int> dVolumes = new Dictionary<string, int>()
{
    {"BulletproofGlass", 8},
    {"Computer", 1},
    {"Construction", 2},
    {"Detector", 6},
    {"Display", 6},
    {"Explosives", 2},
    {"GravityGenerator", 200},
    {"Girder", 2},
    {"InteriorPlate", 5},
    {"LargeTube", 38},
    {"Medical", 160},
    {"MetalGrid", 15},
    {"Motor", 8},
    {"PowerCell", 45},
    {"RadioCommunication", 140},
    {"Reactor", 8},
    {"SmallTube", 2},
    {"SolarCell", 50},
    {"SteelPlate", 3},
    {"Superconductor",8},
    {"Thrust", 10}
};
//========================================================================// 
//Small ship blocks from projector
//========================================================================// 
Dictionary<string, Dictionary<string, int>> dSmallShipComponentPieces = new Dictionary<string, Dictionary<string, int>>()
{
    {"Advanced Rotor", new Dictionary<string, int>{
        {"SteelPlate",35},
        {"Construction",5},
        {"SmallTube",1},
        {"Motor",1},
        {"Computer",1},
        {"LargeTube",10}
    }}, //Includes Adv. rotor part
    {"Air Vent", new Dictionary<string, int>{{"SteelPlate",8},{"Construction",10},{"Motor",2},{"Computer",5}}},
    {"Antenna", new Dictionary<string, int>{
        {"RadioCommunication",4},
        {"Computer",1},
        {"Construction",2},
        {"SmallTube",1},
        {"SteelPlate",1}
    }},
    {"Armor blocks", new Dictionary<string, int>{{"SteelPlate",1}}},
    {"Artificial Mass", new Dictionary<string, int>{
        {"GravityGenerator",1},
        {"Computer",2},
        {"Construction",2},
        {"Superconductor",2},
        {"SteelPlate",3}
    }},
    {"Atmospheric Thrusters", new Dictionary<string, int>{
        {"SteelPlate",3},
        {"Construction",22},
        {"LargeTube",1},
        {"MetalGrid",1},
        {"Motor", 18}
    }},
    {"Battery", new Dictionary<string, int>{
        {"SteelPlate",25},
        {"Construction",5},
        {"PowerCell",20},
        {"Computer", 2}
    }},
    {"Beacon", new Dictionary<string, int> {
        {"RadioCommunication",4},
        {"Computer",1},
        {"SmallTube",1},
        {"Construction",1},
        {"SteelPlate",2}
    }},
    {"Blast doors", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door corner", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door corner inverted", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Blast door edge", new Dictionary<string, int>{{"SteelPlate",5}}},
    {"Button Panel", new Dictionary<string, int> {
        {"Computer", 1},
        {"Construction",2},
        {"InteriorPlate",2}
    }},
    {"Camera", new Dictionary<string, int>{{"Computer",3},{"SteelPlate",2}}},
    {"Cockpit", new Dictionary<string, int>{
        {"SteelPlate",10},
        {"Construction",10},
        {"Motor",1},
        {"Display",5},
        {"Computer",15},
        {"BulletproofGlass", 30}
    }},
    {"Collector", new Dictionary<string, int>{{"SteelPlate",35},{"Construction",35},{"SmallTube",12},{"Motor",8},{"Display",2},{"Computer",8}}},
    {"Connector", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",12},{"SmallTube",6},{"Motor",6},{"Computer",6}}},
    {"Control Panel", new Dictionary<string, int>{
        {"Display",1},
        {"Computer",1},
        {"Construction",1},
        {"SteelPlate",1}
    }},
    {"Conveyor", new Dictionary<string, int> {{"InteriorPlate", 25}, {"Construction", 70}, {"SmallTube", 25}, {"Motor", 2}}},
    {"Conveyor Junction", new Dictionary<string, int> {{"InteriorPlate", 25}, {"Construction", 70}, {"SmallTube", 25}, {"Motor", 2}}},
    {"Conveyor Frame", new Dictionary<string, int> {{"SteelPlate", 4}, {"Construction", 12}, {"SmallTube", 5}, {"Motor", 2}}},
    {"Conveyor Sorter", new Dictionary<string, int> {{"Motor", 2}, {"Computer", 5}, {"SmallTube", 5}, {"Construction", 12}, {"InteriorPlate", 5}}},
    {"Conveyor Tube", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 30}, {"SmallTube", 10}, {"Motor", 6}}},
    {"Corner LCD Bottom", new Dictionary<string, int> {{"Display", 1}, {"Computer", 2}, {"Construction", 3}}},
    {"Corner LCD Top", new Dictionary<string, int> {{"Display", 1}, {"Computer", 2}, {"Construction", 3}}},
    {"Curved Conveyor Tube", new Dictionary<string, int> {{"SteelPlate", 7}, {"Construction", 30}, {"SmallTube", 10}, {"Motor", 6}}},
    {"Decoy", new Dictionary<string, int> {{"Construction", 1}, {"Computer", 1}, {"RadioCommunication", 1}, {"SmallTube", 2}, {"Girder", 1}}},
    {"Drill", new Dictionary<string, int> {{"SteelPlate", 32}, {"Construction", 8}, {"SmallTube", 8}, {"LargeTube", 4}, {"Motor", 1}, {"Computer", 1}}},
    {"Ejector", new Dictionary<string, int> {{"SteelPlate", 7}, {"Construction", 4}, {"SmallTube", 2}, {"Motor", 1}, {"Computer", 4}}},
    {"Fighter Cockpit", new Dictionary<string, int> {{"Construction", 20}, {"Motor", 1}, {"SteelPlate", 18}, {"InteriorPlate", 15}, {"Display", 4}, {"Computer", 20}, {"BulletproofGlass", 40}}},
    {"Gatling Gun", new Dictionary<string, int> {{"Construction", 1}, {"SmallTube", 3}, {"Motor", 1}, {"Computer", 1}, {"SteelPlate", 4}}},
    {"Gatling Turret", new Dictionary<string, int> {{"SteelPlate", 10}, {"Construction", 30}, {"MetalGrid", 5}, {"LargeTube", 1}, {"Motor", 4}, {"Computer", 10}}},
    {"Grinder", new Dictionary<string, int> {{"SteelPlate", 12}, {"Construction", 17}, {"SmallTube", 4}, {"LargeTube", 1}, {"Motor", 4}, {"Computer", 2}}},
    {"Gyroscope", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 5}, {"LargeTube", 1}, {"Motor", 2}, {"Computer", 3}}},
    {"Heavy Armor Block", new Dictionary<string, int> {{"SteelPlate", 3}, {"MetalGrid",2}}},
    {"Heavy Armor Corner", new Dictionary<string, int> {{"SteelPlate", 1}, {"MetalGrid",1}}},
    {"Heavy Armor Inv. Corner", new Dictionary<string, int> {{"SteelPlate", 2}, {"MetalGrid",1}}},
    {"Heavy Armor Slope", new Dictionary<string, int> {{"SteelPlate", 2}, {"MetalGrid",1}}},
    {"Hydrogen Tank Small", new Dictionary<string, int> {{"SteelPlate", 80}, {"LargeTube",40}, {"SmallTube",60}, {"Computer", 8}, {"Construction", 40}}},
    {"Hydrogen Thrusters", new Dictionary<string, int> {{"SteelPlate", 30}, {"Construction",30}, {"MetalGrid",22}, {"LargeTube",10}}},

    {"Ion Thrusters", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 5}, {"Thrust", 12}, {"SteelPlate", 5}}},
    {"Landing Gear", new Dictionary<string, int> {{"Construction", 1}, {"LargeTube", 1}, {"Motor", 1}, {"SteelPlate", 1}}},
    {"Large Atmospheric Thruster", new Dictionary<string, int> {{"SteelPlate", 20}, {"Construction", 30}, {"LargeTube",4}, {"MetalGrid", 8}, {"Motor", 144}}},
    {"Large Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 75}, {"Construction", 25}, {"Computer", 6}, {"Motor", 8}, {"Display", 1}}},
    {"Large Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate", 30}, {"Construction",30}, {"MetalGrid",22}, {"LargeTube",10}}},
    {"Large Ion Thruster", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 5}, {"Thrust", 12}, {"SteelPlate", 5}}},
    {"Large Reactor", new Dictionary<string, int> {{"SteelPlate", 60}, {"Construction", 9}, {"MetalGrid", 9}, {"LargeTube", 3}, {"Reactor", 95}, {"Motor", 5}, {"Computer", 25}}},
    {"Laser Antenna", new Dictionary<string, int> {{"BulletproofGlass", 2}, {"Computer", 30}, {"RadioCommunication", 5}, {"Motor", 5}, {"Construction", 10}, {"SmallTube", 10}, {"SteelPlate", 10},{"Superconductor",10}}},
    {"LCD Panel", new Dictionary<string, int> {{"Construction", 4}, {"Computer", 4}, {"Display", 3}}},
    {"Light Armor Block", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Corner 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Light Armor Slope 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 1}}},
    {"Medium Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 30}, {"Construction", 10}, {"Computer", 4}, {"Motor", 6}, {"Display", 1}}},
    {"Merge Block", new Dictionary<string, int> {{"Construction", 5}, {"Motor", 1}, {"SmallTube", 2}, {"Computer", 1}, {"SteelPlate", 4}}},
    {"Ore Detector", new Dictionary<string, int> {{"Construction", 2}, {"Motor", 1}, {"Computer", 1}, {"Detector", 1}, {"SteelPlate", 2}}},
    //{"Oxygen Generator", new Dictionary<string, int> {{"Computer", 3}, {"Motor", 1}, {"LargeTube", 2}, {"Construction", 8}, {"SteelPlate", 8}}},
    {"O2/H2 Generator", new Dictionary<string, int> {{"Computer", 3}, {"Motor", 1}, {"LargeTube", 2}, {"Construction", 8}, {"SteelPlate", 8}}},
    {"Oxygen Tank", new Dictionary<string, int> {{"Construction", 10}, {"Computer", 3}, {"SmallTube", 10}, {"LargeTube", 2}, {"SteelPlate", 14}}},
    {"Passenger Seat", new Dictionary<string, int> {{"InteriorPlate", 20}, {"Construction", 20}}},
    {"Piston", new Dictionary<string, int> {{"LargeTube", 2}, {"SteelPlate", 8}, {"Construction", 4}, {"SmallTube", 4}, {"Motor", 2}, {"Computer", 1}}}, //Includes piston head since it's not available as a single block
    {"Programmable block", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 2}, {"Motor", 1}, {"Display", 1}, {"Computer", 2}, {"SteelPlate", 2}}},
    {"Projector", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 2}, {"Motor", 1}, {"Computer", 2}, {"SteelPlate", 2}}},
    {"Remote Control", new Dictionary<string, int> {{"Construction", 1}, {"Motor", 1}, {"Computer", 1}, {"InteriorPlate", 2}}},
    {"Rocket Launcher", new Dictionary<string, int> {{"Construction", 2}, {"LargeTube", 4}, {"Motor", 1}, {"Computer", 1}, {"SteelPlate", 4}, {"MetalGrid", 1}}},
    {"Reloadable Rocket Launcher", new Dictionary<string, int> {{"SteelPlate", 8}, {"SmallTube", 50}, {"InteriorPlate", 50}, {"Construction", 24}, {"LargeTube", 8}, {"Motor", 4}, {"Computer", 2}}},
    {"Rotor", new Dictionary<string, int> {{"SteelPlate", 5}, {"Construction", 5}, {"SmallTube", 1}, {"Motor", 1}, {"Computer", 1}}},
    {"Rotor Part", new Dictionary<string, int> {{"SteelPlate", 12}, {"SmallTube", 6}}},
    {"Sensor", new Dictionary<string, int> {{"InteriorPlate", 5}, {"Construction", 5}, {"Computer", 6}, {"RadioCommunication", 6}, {"Detector", 6}, {"SteelPlate", 2}}},
    {"Small Atmospheric Thruster", new Dictionary<string, int> {{"SteelPlate", 3}, {"LargeTube", 1}, {"MetalGrid", 1}, {"Motor", 18}, {"Construction", 2}}},

    {"Small Battery", new Dictionary<string, int>{
        {"Computer",2},
        {"PowerCell",2},
        {"Construction",2},
        {"SteelPlate",4}
    }},
    {"Small Cargo Container", new Dictionary<string, int> {{"InteriorPlate", 3}, {"Construction", 1}, {"Computer", 2}, {"Motor", 2}, {"Display", 1}}},
    {"Small Conveyor", new Dictionary<string, int> {{"InteriorPlate", 4}, {"Construction", 4}, {"Motor", 1}}},
    {"Small Conveyor Sorter", new Dictionary<string, int> {{"Motor", 2}, {"Computer", 5}, {"SmallTube", 5}, {"Construction", 12}, {"InteriorPlate", 5}}},
    {"Small Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Construction", 1}, {"Motor", 1}}},
    {"Small Curved Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Motor", 1}, {"Construction", 1}}},
    {"Small Curved Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate", 1}, {"Motor", 1}, {"Construction", 1}}},
    {"Small Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate",7},{"Construction",15}, {"MetalGrid",4}, {"LargeTube",2}}},
    {"Small Ion Thruster", new Dictionary<string, int> {{"LargeTube", 1}, {"Thrust", 1}, {"Construction", 1}, {"SteelPlate", 1}}},
    {"Small Reactor", new Dictionary<string, int> {{"Construction", 1}, {"MetalGrid", 1}, {"LargeTube", 1}, {"Reactor", 1}, {"Motor", 1}, {"Computer", 10}, {"SteelPlate", 2}}},
    {"Solar Panel", new Dictionary<string, int> {{"MetalGrid", 2}, {"SmallTube", 1}, {"SteelPlate", 1}, {"Computer", 1}, {"SolarCell", 16}, {"Construction", 2}}},
    {"Sound Block", new Dictionary<string, int> {{"InteriorPlate", 4}, {"Construction", 6}, {"Computer", 3}}},
    {"Space Ball", new Dictionary<string, int> {{"GravityGenerator", 1}, {"Computer", 7}, {"Construction", 10}, {"SteelPlate", 70}}},
    {"Spotlight", new Dictionary<string, int> {{"SteelPlate", 1}, {"Construction", 1}, {"InteriorPlate", 1}}},
    {"Survival kit", new Dictionary<string, int>{
        {"Computer",5},
        {"Display",1},
        {"Motor",4},
        {"Medical",3},
        {"Construction",2},
        {"SteelPlate",6}
    }},
    {"Text panel", new Dictionary<string, int> {{"Construction", 4}, {"Computer", 4}, {"Display", 3}}},
    {"Timer Block", new Dictionary<string, int> {{"InteriorPlate", 2}, {"Construction", 3}, {"Computer", 1}}},
    {"Warhead", new Dictionary<string, int> {{"Girder", 1}, {"Construction", 1}, {"SmallTube", 2}, {"Computer", 1}, {"Explosives", 2}}},
    {"Welder", new Dictionary<string, int> {{"SteelPlate", 12}, {"Construction", 17}, {"SmallTube", 6}, {"LargeTube", 1}, {"Motor", 2}, {"Computer", 2}}},
    {"Wheel 1x1", new Dictionary<string, int> {{"MetalGrid", 2}, {"Construction", 5}, {"LargeTube", 1}, {"SteelPlate", 2}}},
    {"Wheel 3x3", new Dictionary<string, int> {{"SteelPlate", 4}, {"MetalGrid", 3}, {"Construction", 15}, {"LargeTube", 2}}},
    {"Wheel 5x5", new Dictionary<string, int> {{"SteelPlate", 6}, {"MetalGrid", 6}, {"Construction", 30}, {"LargeTube", 3}}},
    {"Wheel Suspension 1x1", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 7}, {"SmallTube", 2}, {"Motor", 1}}},
    {"Wheel Suspension 3x3", new Dictionary<string, int> {{"SteelPlate", 8}, {"Construction", 7}, {"SmallTube", 2}, {"Motor", 1}}},
    {"Wheel Suspension 5x5", new Dictionary<string, int> {{"SteelPlate", 16}, {"Construction", 12}, {"SmallTube", 4}, {"Motor", 2}}},
    {"Wide LCD panel", new Dictionary<string, int> {{"Construction", 8}, {"Computer", 8}, {"Display", 6}}},
};
//========================================================================// 
//Large ship blocks from projector
//Projector doesn't differentiate between armor block, heavy armor and interior wall...
//========================================================================// 
Dictionary<string, Dictionary<string, int>> dLargeShipComponentPieces = new Dictionary<string, Dictionary<string, int>>()
{
    {"Basic Assembler", new Dictionary<string, int>{
        {"Computer",80},
        {"Display",4},
        {"Motor",10},
        {"Construction",40},
        {"SteelPlate",80}
    }},
    {"Basic Refinery", new Dictionary<string, int>{
        {"Computer",10},
        {"Motor",10},
        {"Construction",20},
        {"SteelPlate",120}
    }},
    {"Ladder", new Dictionary<string, int>{
        {"SmallTube",10},
        {"Construction",20},
        {"InteriorPlate",10}
    }},
    {"Survival kit", new Dictionary<string, int>{
        {"Computer",5},
        {"Display",1},
        {"Motor",4},
        {"Medical",3},
        {"Construction",2},
        {"SteelPlate",30}
    }},
    {"Wind Turbine", new Dictionary<string, int>{
        {"Computer",2},
        {"Girder", 24},
        {"Construction",20},
        {"Motor",8},
        {"InteriorPlate",40}
    }},
    {"Advanced Rotor", new Dictionary<string, int>{{"Computer",2},{"Motor",4},{"LargeTube",14},{"Construction",10},{"SteelPlate",45}}},//Includes Adv. rotor part
    {"Air Vent", new Dictionary<string, int>{{"Computer",5},{"Motor",10},{"Construction",20},{"SteelPlate",45}}},
    {"Airtight Hangar Door", new Dictionary<string, int>{{"Computer",2},{"Motor",16},{"SmallTube",40},{"Construction",40},{"SteelPlate",350}}},
    {"Antenna", new Dictionary<string, int>{{"RadioCommunication",40},{"Computer",8},{"Construction",30},{"SmallTube",60},{"LargeTube",40},{"SteelPlate",80}}},
    //{"Arc furnace", new Dictionary<string, int> {{"SteelPlate", 120}, {"Construction", 5}, {"LargeTube", 2}, {"Motor", 4}, {"Computer", 5}}},
    {"Armor blocks", new Dictionary<string, int>{{"SteelPlate",25}}},
    {"Artificial Mass", new Dictionary<string, int>{{"GravityGenerator",9},{"Computer",20},{"Construction",30},{"Superconductor",20},{"SteelPlate",90}}},
    {"Assembler", new Dictionary<string, int>{{"Computer",160},{"MetalGrid",10},{"Display",10},{"Motor",20},{"Construction",80},{"SteelPlate",140}}},
    {"Atmospheric Thrusters", new Dictionary<string, int>{{"Motor",110},{"MetalGrid",10},{"LargeTube",8},{"Construction",50},{"SteelPlate",35}}},
    {"Battery", new Dictionary<string, int>{{"Computer",25},{"PowerCell",80},{"Construction",30},{"SteelPlate",80}}},
    {"Beacon", new Dictionary<string, int>{{"RadioCommunication",40},{"Computer",10},{"LargeTube",20},{"Construction",30},{"SteelPlate",80}}},
    {"Blast doors", new Dictionary<string, int>{{"SteelPlate",140}}},
    {"Blast door corner", new Dictionary<string, int>{{"SteelPlate",120}}},
    {"Blast door corner inverted", new Dictionary<string, int>{{"SteelPlate",135}}},
    {"Blast door edge", new Dictionary<string, int>{{"SteelPlate",130}}},
    {"Button Panel", new Dictionary<string, int> {{"Computer",20},{"Construction",20},{"InteriorPlate",10}}},
    {"Camera", new Dictionary<string, int>{{"Computer",3},{"SteelPlate",2}}},
    {"Cockpit", new Dictionary<string, int>{{"BulletproofGlass",60},{"Computer",100},{"Display",8},{"Motor",1},{"Construction",20},{"InteriorPlate",30}}},
    {"Collector", new Dictionary<string, int>{{"Computer",10},{"Display",4},{"Motor",8},{"SmallTube",12},{"Construction",50},{"SteelPlate",45}}},
    {"Connector", new Dictionary<string, int>{{"Computer",20},{"Motor",8},{"SmallTube",12},{"Construction",40},{"SteelPlate",150}}},
    {"Control Panel", new Dictionary<string, int>{{"Display",1},{"Computer",1},{"Construction",1},{"SteelPlate",1}}},
    {"Control Station", new Dictionary<string, int>{{"Display",10},{"Computer",100},{"Motor",2},{"Construction",20},{"InteriorPlate",20}}},
    //{"Conveyor", new Dictionary<string, int> {{"InteriorPlate", 50}, {"Construction", 120}, {"SmallTube", 50}, {"Motor", 2}}},
    {"Conveyor Junction", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",30},{"SmallTube",20},{"Motor",6}}},
    {"Conveyor Sorter", new Dictionary<string, int>{{"Motor",2},{"Computer",20},{"SmallTube",50},{"Construction",120},{"InteriorPlate",50}}},
    {"Conveyor Tube", new Dictionary<string, int>{{"Motor",6},{"SmallTube",12},{"Construction",20},{"InteriorPlate",14}}},
    {"Corner LCD Bottom", new Dictionary<string, int>{{"Display",1},{"Computer",3},{"Construction",5}}},
    {"Corner LCD Top", new Dictionary<string, int>{{"Display",1},{"Computer",3},{"Construction",5}}},
    {"Corner Light", new Dictionary<string, int>{
        {"Construction",3}
    }},
    {"Corner Light - Double", new Dictionary<string, int>{
        {"Construction",6}
    }},

    {"Cover Walls", new Dictionary<string, int>{{"Construction",10},{"SteelPlate",4}}},
    {"Cryo Chamber", new Dictionary<string, int>{{"BulletproofGlass",10},{"Computer",30},{"Display",8},{"Motor",8},{"Construction",20},{"InteriorPlate",40}}},
    {"Curved Conveyor Tube", new Dictionary<string, int> {{"InteriorPlate",10},{"Construction",20},{"SmallTube",12},{"Motor",6}}},
    {"Door", new Dictionary<string, int>{{"SteelPlate",8},{"Computer",2},{"Display",1},{"Motor",2},{"SmallTube",4},{"Construction",40},{"InteriorPlate",10}}},
    {"Decoy", new Dictionary<string, int>{{"LargeTube",2},{"RadioCommunication",1},{"Computer",10},{"Construction",10},{"SteelPlate",30}}},
    {"Diagonal Window", new Dictionary<string, int>{{"InteriorPlate",16},{"Construction",12},{"SmallTube",6}}},
    {"Drill", new Dictionary<string, int>{{"SteelPlate",300},{"Construction",40},{"LargeTube",12},{"Motor",5},{"Computer",5}}},
    //{"Effectiveness Module", new Dictionary<string, int> {{"Motor", 5}, {"MetalGrid", 10}, {"SmallTube", 15}, {"Construction", 50}, {"SteelPlate", 100}}},
    {"Flight Seat", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",20},{"Motor",2},{"Computer",100},{"Display",4}}},
    //{"Full Cover Wall", new Dictionary<string, int> {{"SteelPlate", 4}, {"Construction", 10}}},
    {"Gatling Turret", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",30},{"MetalGrid",15},{"SmallTube",6},{"Motor",8},{"Computer",10}}},
    {"Gravity Generator", new Dictionary<string, int>{{"SteelPlate",150},{"GravityGenerator",6},{"Construction",60},{"LargeTube",4},{"Motor",6},{"Computer",40}}},
    {"Grinder", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",30},{"LargeTube",1},{"Motor",4},{"Computer",2}}},
    {"Gyroscope", new Dictionary<string, int>{{"SteelPlate",600},{"Construction",40},{"LargeTube",4},{"MetalGrid",50},{"Motor",4},{"Computer",5}}},
    {"Half Cover Wall", new Dictionary<string, int>{{"SteelPlate",2},{"Construction",6}}},
    //{"Heavy Armor Block", new Dictionary<string, int> {{"SteelPlate", 150}, {"MetalGrid",50}}},
    //{"Heavy Armor Corner", new Dictionary<string, int> {{"SteelPlate", 25}, {"MetalGrid",10}}},
    //{"Heavy Armor Inv. Corner", new Dictionary<string, int> {{"SteelPlate", 125}, {"MetalGrid",50}}},
    //{"Heavy Armor Slope", new Dictionary<string, int> {{"SteelPlate", 75}, {"MetalGrid",25}}},
    {"Hydrogen Tank", new Dictionary<string, int>{{"SteelPlate",280},{"LargeTube",80},{"SmallTube",60},{"Computer",8},{"Construction",40}}},
    {"Hydrogen Thrusters", new Dictionary<string,int>{{"SteelPlate",25},{"Construction",60},{"MetalGrid",40},{"LargeTube",8}}},
    {"Ion Thrusters", new Dictionary<string, int>{{"SteelPlate",25},{"Construction",60},{"LargeTube",8},{"Thrust",80}}},
    {"Interior Light", new Dictionary<string, int> {{"Construction", 2}}},
    {"Interior Pillar", new Dictionary<string, int>{{"InteriorPlate",25},{"Construction",10},{"SmallTube", 4}}},
    {"Interior Turret", new Dictionary<string, int>{{"InteriorPlate",6},{"Construction",20},{"SmallTube",1},{"Motor",2},{"Computer",5},{"SteelPlate",4}}},
    {"Jump Drive", new Dictionary<string, int>{{"SteelPlate",60},{"MetalGrid",50},{"GravityGenerator",20},{"Detector",20},{"PowerCell",120},{"Superconductor",1000},{"Computer",300},{"Construction",40}}},
    {"Landing Gear", new Dictionary<string, int>{{"SteelPlate",150},{"Construction",20},{"Motor", 6}}},
    {"Large Atmospheric Thruster", new Dictionary<string, int>{{"SteelPlate",230},{"Construction",60},{"LargeTube",50},{"MetalGrid",40},{"Motor",1100}}},
    {"Large Cargo Container", new Dictionary<string, int>{{"InteriorPlate",360},{"Construction",80},{"MetalGrid",24},{"SmallTube",60},{"Motor",20},{"Display",1},{"Computer",8}}},
    {"Large Hydrogen Thruster", new Dictionary<string,int>{{"SteelPlate",150},{"Construction",180},{"MetalGrid",250},{"LargeTube",40}}},
    {"Large Ion Thruster", new Dictionary<string, int>{{"SteelPlate",150},{"Construction",100},{"LargeTube",40},{"Thrust",960}}},
    {"Large Reactor", new Dictionary<string, int>{{"SteelPlate",1000},{"Construction",70},{"MetalGrid", 40},{"LargeTube",40},{"Reactor",2000},{"Motor",20},{"Computer",75},{"Superconductor",100}}},
    {"Laser Antenna", new Dictionary<string, int>{{"BulletproofGlass",4},{"Computer",50},{"Superconductor",100},{"RadioCommunication",20},{"Detector",30},{"Motor",16},{"Construction",40},{"SteelPlate",50}}},
    {"LCD Panel", new Dictionary<string, int>{{"BulletproofGlass",6},{"Display",10},{"Computer",6},{"Construction",6},{"InteriorPlate",1}}},
    {"Light Armor Block", new Dictionary<string, int> {{"SteelPlate", 25}}},
    {"Light Armor Corner", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Light Armor Corner 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 10}}},
    {"Light Armor Corner 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Light Armor Slope", new Dictionary<string, int> {{"SteelPlate", 13}}},
    {"Light Armor Slope 2x1x1 Base", new Dictionary<string, int> {{"SteelPlate", 19}}},
    {"Light Armor Slope 2x1x1 Tip", new Dictionary<string, int> {{"SteelPlate", 4}}},
    {"Medical Room", new Dictionary<string, int>{{"InteriorPlate",240},{"Construction",80},{"MetalGrid",60},{"SmallTube",20},{"LargeTube",5},{"Display",10},{"Computer",10},{"Medical",15}}},
    {"Merge Block", new Dictionary<string, int>{{"SteelPlate",12},{"Construction",15},{"Motor",2},{"LargeTube",6},{"Computer",2}}},
    {"Missile Turret", new Dictionary<string, int>{{"SteelPlate",20},{"Construction",40},{"MetalGrid",5},{"LargeTube",6},{"Motor",16},{"Computer",12}}},
    {"Ore Detector", new Dictionary<string, int>{{"SteelPlate",50},{"Construction",40},{"Motor",5},{"Computer",25},{"Detector",20}}},
    {"Oxygen Farm", new Dictionary<string, int>{{"Computer",20},{"Construction",20},{"SmallTube",10},{"LargeTube",20},{"BulletproofGlass",100},{"SteelPlate", 40}}},
    //{"Oxygen Generator", new Dictionary<string, int> {{"Computer", 5}, {"Motor", 4}, {"LargeTube", 2}, {"Construction", 5}, {"SteelPlate", 120}}},
    {"O2/H2 Generator", new Dictionary<string, int>{{"Computer",5},{"Motor",4},{"LargeTube",2},{"Construction",5},{"SteelPlate",120}}},
    {"Oxygen Tank", new Dictionary<string, int>{{"Construction",40},{"Computer",8},{"SmallTube",60},{"LargeTube",40},{"SteelPlate",80}}},
    {"Passage", new Dictionary<string, int>{{"InteriorPlate",74},{"Construction",20},{"SmallTube",48}}},
    {"Passenger Seat", new Dictionary<string, int>{{"InteriorPlate",20},{"Construction",20}}},
    {"Piston", new Dictionary<string, int>{{"SteelPlate",25},{"Construction",10},{"LargeTube",14},{"Motor",4},{"Computer",2}}}, //Includes piston head
    {"Power Efficiency Module", new Dictionary<string, int>{{"Motor",4},{"PowerCell",20},{"SmallTube",20},{"Construction",40},{"SteelPlate",100}}},
    //{"Productivity Module", new Dictionary<string, int> {{"Motor", 2}, {"LargeTube", 10}, {"SmallTube", 20}, {"Construction", 40}, {"SteelPlate", 100}}},
    {"Programmable block", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",4},{"LargeTube",2},{"Motor",1},{"Display",1},{"Computer",2}}},
    {"Projector", new Dictionary<string, int>{{"SteelPlate",21},{"Construction",4},{"LargeTube",2},{"Motor",1},{"Computer",2}}},
    {"Ramp", new Dictionary<string, int>{{"InteriorPlate",70},{"Construction",16}}},
    {"Refinery", new Dictionary<string, int>{{"SteelPlate",1200},{"Construction",40},{"LargeTube",20},{"Motor",16},{"MetalGrid",20},{"Computer",20}}},
    {"Remote Control", new Dictionary<string, int>{{"InteriorPlate",10},{"Construction",10},{"Motor",1},{"Computer",15}}},
    {"Rocket Launcher", new Dictionary<string, int>{{"SteelPlate",35},{"Construction",8},{"MetalGrid",30},{"LargeTube",25},{"Motor",6},{"Computer",4}}},
    {"Rotor", new Dictionary<string, int>{{"SteelPlate",15},{"Construction",10},{"LargeTube",4},{"Motor",4},{"Computer",2}}}, // does not include rotor part as it's added via addcomops
    {"Rotor Part", new Dictionary<string, int>{{"SteelPlate",30},{"LargeTube",6}}},
    {"Sensor", new Dictionary<string, int>{{"InteriorPlate",5},{"Construction",5},{"Computer",6},{"RadioCommunication",4},{"Detector",6},{"SteelPlate",2}}},
    {"Sliding Door", new Dictionary<string, int>{
        {"BulletproofGlass",15},
        {"Computer",2},
        {"Display",1},
        {"Motor",4},
        {"SmallTube",4},
        {"Construction",40},
        {"SteelPlate",20}
    }},
    {"Small Cargo Container", new Dictionary<string, int> {
        {"InteriorPlate",40},
        {"Construction", 40},
        {"MetalGrid",4},
        {"SmallTube",20},
        {"Motor",4},
        {"Display",1},
        {"Computer", 2}
    }},
    //{"Small Atmospheric Thruster", new Dictionary<string, int>{{"SteelPlate",35},{"Construction", 50}, {"LargeTube",8}, {"MetalGrid", 10}, {"Motor", 113}}},
    //{"Small Hydrogen Thruster", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 60}, {"LargeTube", 8}, {"MetalGrid",40}}},
    //{"Small Ion Thruster", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 60}, {"LargeTube", 8}, {"Thrust", 80}}},
    {"Small Reactor", new Dictionary<string, int> {
        {"SteelPlate",80},
        {"Construction",40},
        {"MetalGrid",4},
        {"LargeTube",8},
        {"Reactor",100},
        {"Motor",6},
        {"Computer",25}
    }},
    {"Solar Panel", new Dictionary<string, int> {
        {"BulletproofGlass", 4},
        {"SolarCell",32},
        {"Computer",4},
        {"Girder",12},
        {"Construction",14},
        {"SteelPlate",4}
    }},
    {"Sound Block", new Dictionary<string, int>{
        {"Computer",3},
        {"Construction",6},
        {"InteriorPlate",4},
    }},
    {"Space Ball", new Dictionary<string, int>{
        {"GravityGenerator",3},
        {"Computer",20},
        {"Construction",30},
        {"SteelPlate",225}
    }},
    {"Speed Module", new Dictionary<string, int>{
        {"Motor",4},
        {"Computer",60},
        {"SmallTube",20},
        {"Construction",40},
        {"SteelPlate",100}
    }},
    {"Spherical Gravity Generator", new Dictionary<string, int>{
        {"SteelPlate",150},
        {"GravityGenerator",6},
        {"Construction",60},
        {"LargeTube",4},
        {"Motor",6},
        {"Computer", 40}
    }},
    {"Spotlight", new Dictionary<string, int> {
        {"BulletproofGlass",4},
        {"Construction",15},
        {"InteriorPlate",20},
        {"LargeTube",2},
        {"SteelPlate",8}
    }},
    {"Stairs", new Dictionary<string, int>{
        {"Construction",30},
        {"InteriorPlate",50}
    }},
    //{"Steel Catwalk", new Dictionary<string, int>{{"Construction",5},{"MetalGrid",25},{"SmallTube",20},{"InteriorPlate", 2}}},
    {"Steel Catwalk Corner", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",32}
    }},
    {"Steel Catwalk Plate", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",23}
    }},
    {"Steel Catwalk Two Sides", new Dictionary<string, int>{
        {"SmallTube",25},
        {"Construction",7},
        {"InteriorPlate",32}
    }},
    {"Steel Catwalks", new Dictionary<string, int>{
        {"SmallTube",20},
        {"Construction",5},
        {"InteriorPlate",27}
    }},
    {"Timer Block", new Dictionary<string, int>{{"InteriorPlate",6},{"Construction",30},{"Computer",5}}},
    {"Text panel", new Dictionary<string, int>{
        {"BulletproofGlass",2},
        {"Display", 10},
        {"Computer",6},
        {"Construction",6},
        {"InteriorPlate",1}
    }},
    {"Warhead", new Dictionary<string, int>{
        {"SteelPlate",20},
        {"Girder",24},
        {"Construction",12},
        {"SmallTube",12},
        {"Computer",2},
        {"Explosives", 6}
    }},
    {"Welder", new Dictionary<string, int>{
        {"SteelPlate",20},
        {"Construction",30},
        {"LargeTube",1},
        {"Motor",2},
        {"Computer",2}
    }},
    {"Wheel 1x1", new Dictionary<string, int>{
        {"SteelPlate",8},
        {"Construction",20},
        {"LargeTube",4}
    }},
    {"Wheel 3x3", new Dictionary<string, int>{
        {"SteelPlate",12},
        {"Construction",25},
        {"LargeTube", 6}
    }},
    {"Wheel 5x5", new Dictionary<string, int> {
        {"SteelPlate",16},
        {"Construction",30},
        {"LargeTube",8}
    }},
    //{"Wheel Suspension 3x3", new Dictionary<string, int> {{"SteelPlate", 25}, {"Construction", 15}, {"LargeTube", 6}, {"SmallTube", 12}, {"Motor", 6}}},
    {"Wheel Suspension 1x1 Left", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 1x1 Right", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 3x3 Left", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 3x3 Right", new Dictionary<string, int>{
        {"Motor", 6},
        {"SmallTube",12},
        {"LargeTube",6},
        {"Construction",15},
        {"SteelPlate",25}
    }},
    {"Wheel Suspension 5x5 Left", new Dictionary<string, int>{
        {"Motor",20},
        {"SmallTube",30},
        {"LargeTube",20},
        {"Construction",40},
        {"SteelPlate",70}
    }},
    {"Wheel Suspension 5x5 Right", new Dictionary<string, int>{
        {"Motor",20},
        {"SmallTube",30},
        {"LargeTube",20},
        {"Construction",40},
        {"SteelPlate",70}
    }},
    //{"Wheel Suspension 5x5", new Dictionary<string, int> {{"SteelPlate",70},{"Construction",40},{"LargeTube",20},{"SmallTube",30},{"Motor",20}}},
    {"Wide LCD panel", new Dictionary<string, int>{
        {"BulletproofGlass",12},
        {"Display", 20},
        {"Computer",12},
        {"Construction",12},
        {"InteriorPlate",2}
    }},
    {"Window 1x2 Slope", new Dictionary<string, int> {{"Girder", 16}, {"BulletproofGlass", 55}}},
    {"Window 1x2 Inv.", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 40}}},
    {"Window 1x2 Face", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 40}}},
    {"Window 1x2 Side Left", new Dictionary<string, int> {{"Girder", 13}, {"BulletproofGlass", 26}}},
    {"Window 1x2 Side Right", new Dictionary<string, int> {{"Girder", 13}, {"BulletproofGlass", 26}}},
    {"Window 1x1 Slope", new Dictionary<string, int> {{"Girder", 12}, {"BulletproofGlass", 35}}},
    {"Window 1x1 Face", new Dictionary<string, int> {{"Girder", 11}, {"BulletproofGlass", 24}}},
    {"Window 1x1 Side", new Dictionary<string, int> {{"Girder", 9}, {"BulletproofGlass", 17}}},
    {"Window 1x1 Inv.", new Dictionary<string, int> {{"Girder", 11}, {"BulletproofGlass", 24}}},
    {"Window 1x1 Flat", new Dictionary<string, int> {{"Girder", 10}, {"BulletproofGlass", 25}}},
    {"Window 1x1 Flat Inv.", new Dictionary<string, int> {{"Girder", 10}, {"BulletproofGlass", 25}}},
    {"Window 1x2 Flat", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 50}}},
    {"Window 1x2 Flat Inv.", new Dictionary<string, int> {{"Girder", 15}, {"BulletproofGlass", 50}}},
    {"Window 2x3 Flat", new Dictionary<string, int> {{"Girder", 25}, {"BulletproofGlass", 140}}},
    {"Window 2x3 Flat Inv.", new Dictionary<string, int> {{"Girder", 25}, {"BulletproofGlass", 140}}},
    {"Window 3x3 Flat", new Dictionary<string, int> {{"Girder", 40}, {"BulletproofGlass", 196}}},
    {"Window 3x3 Flat Inv.", new Dictionary<string, int> {{"Girder", 40}, {"BulletproofGlass", 196}}},
    {"Vertical Window", new Dictionary<string, int> {{"InteriorPlate", 12}, {"Construction", 8}, {"SmallTube", 4}}},
    {"Yield Module", new Dictionary<string, int>{
        {"Motor",4},
        {"Superconductor",20},
        {"SmallTube",15},
        {"Construction",50},
        {"SteelPlate", 100}
    }},

};
//========================================================================// 
// Variables used by code
//========================================================================// 
bool boolInitComplete = false;
List<string> blocks = new List<string>();
List<IMyTerminalBlock> AllStorageBlocks=new List<IMyTerminalBlock>();
List<IMyTerminalBlock> ProjectorBlocks=new List<IMyTerminalBlock>();

Dictionary<string, int> dReqComponents = new Dictionary<string, int>();
Dictionary<string, int> dComponentsCount = new Dictionary<string, int>();
ClapYoFace CYF = new ClapYoFace(true);

IMyProjector projo = null;
bool boolProjectorLoading = false;
bool boolProjectionComplete = false;
//int intProjectorLoadingCounter = 0;
int intDelayCounter = 0;
string strEchoData = "";
string strDisplayData = "";
string strDisplayDataMangled = "";

bool boolAutoScroll = true;

int intTotalVolume=0;

string strTimData = "";

List<string> strKey = new List<string>(); // type of component
List<int> intValue = new List<int>(); // amount of type of component

string strSubTypeForCompoCount="";

//============================================================================//
// Main
//============================================================================//
void Main(string argument)
{
    //========================================================================//
    // Init
    //========================================================================//
    if(!boolInitComplete)
    {
        CYF.SetExactName(false);
        if(!CYF.SetDisplay(strDisplayNameContains, GridTerminalSystem)){Echo(CYF.strError); return;} // Find display, error checking.
        Init();
        return;
    }
    //========================================================================//
    // Argument Handling (Scroll arguments lower)
    //========================================================================//
    if(argument.ToLower().Trim().StartsWith("rescanticks"))
    {
        argument = argument.Remove(0,11).Trim();
        if(!Int32.TryParse(argument,out intRescanTicks))
        {
            Echo("Error Parsing argument not an int:"+argument);
            return;
        }
    }
    else if(argument.ToLower().Trim()=="getrescanticks")
    {
        Echo("RescanTicks: "+intRescanTicks.ToString());
    }
    else if(argument.ToLower().Trim()=="pscanonloaded")
    {
        ProjectorSearch(true,true);
    }
    else if(argument.ToLower().Trim()=="pscanon")
    {
        ProjectorSearch(true);
    }
    else if(argument.ToLower().Trim()=="pscan")
    {
        ProjectorSearch();
    }
    else if(argument.ToLower().Trim()=="reinit")
    {
        Init();
    }
    else if(argument.ToLower().Trim()=="debugon")
        boolDebug=true;
    else if(argument.ToLower().Trim()=="debugoff")
        boolDebug=false;
    else if(argument.ToLower().Trim()=="help")
    {
        help();
        return;
    }
    else if(argument.ToLower().Trim()=="settings")
    {
        EchoSettings();
        return;
    }
    //========================================================================//
    // Display Config, checks/sets display configuration based on the display found.
    //   if font/font size are change this updates the settings.
    //========================================================================//
    CYF.SetDisplayConfig(); // configure display, font sizes, etc.
    //========================================================================//
    // Debug Messages for me.
    //========================================================================//
    if (boolDebug)
    {
        Echo("boolProjectionComplete:"+boolProjectionComplete.ToString());
        Echo("boolProjectorLoading:"+boolProjectorLoading.ToString());
        Echo("IsProjecting:"+projo.IsProjecting.ToString());
        Echo("DelayCounter :"+intDelayCounter.ToString());
    }
    //========================================================================//
    // Delay Counter, so we don't hose your game by scanning cargo containers
    // every tick/program run iteration.
    //========================================================================//
    if (intDelayCounter<intRescanTicks)
        intDelayCounter++;
    else
    {
        GetComponentCount(AllStorageBlocks);//this before GetProjectorData
        GetProjectorData();
        intDelayCounter=0;
    }
    //========================================================================//
    // Variable Resets
    //========================================================================//
    intTotalVolume=0;
    strEchoData = "";
    strDisplayData = "";
    //========================================================================//
    // Display Projection Complete
    //========================================================================//
    if(boolProjectionComplete)
    {
        strDisplayData+=CYF.AlignCenter("Blueprint Requirements");
        strDisplayData+="\n\n\n\n\n\n";
        strDisplayData+=CYF.AlignCenter("Projection is Built!");
        CYF.WriteDisplay(strDisplayData);
        Echo("Projection is Built!");
        return;
    }
    //========================================================================//
    // Display Projector Loading Screen
    //========================================================================//
    if(boolProjectorLoading)
    {
        strDisplayData+=CYF.AlignCenter("Blueprint Requirements");
        strDisplayData+="\n\n\n\n\n\n";
        strDisplayData+=CYF.AlignCenter("Waiting for Projection to load...");
        CYF.WriteDisplay(strDisplayData);
        Echo("Waiting for Projection to load...");
        Echo("Projector is either off or no BP is loaded");
        return;
    }
    //========================================================================//
    // Iterate through Components list
    //========================================================================//
    // these need to be added to init, run clears here?
    // string builder too.. err.
    //List<string> strKey = new List<string>(dReqComponents.Keys); // type of component
    //List<int> intValue = new List<int>(dReqComponents.Values); // amount of type of component
    strKey = new List<string>(dReqComponents.Keys); // type of component
    intValue = new List<int>(dReqComponents.Values); // amount of type of component
    //Store the list of components in a string
    //strDisplayData += CYF.AlignCenter("Blueprint Requirements")+"\n\n";
    strEchoData += "This blueprint requires (have/need):\n";
    strTimData = "[TIM";
    for(int i=0; i<strKey.Count; i++)
    {
        strEchoData += strKey[i]+": "+dComponentsCount[strKey[i]]+"/"+intValue[i]+"\n";
        strDisplayData += CYF.AlignLeftRight(strKey[i],dComponentsCount[strKey[i]]+"/"+intValue[i])+"\n";
        strDisplayData += CYF.ProgressBarTrailingNumbers((float)dComponentsCount[strKey[i]],(float)intValue[i])+"\n";
        intTotalVolume += intValue[i] * dVolumes[strKey[i]];
        strTimData += " " + strKey[i] + ":" + intValue[i]; // lead in with space
    }
    strTimData += "]";
    Me.CustomData = strTimData;
    strEchoData += "\nTotal volume of required components: "+intTotalVolume.ToString()+" L.\n";
    strDisplayData += "\nTotal volume of required components: "+intTotalVolume.ToString()+" L.\n";
    //========================================================================//
    // Scroll Argument Handling (must be here auto for autoresizing, something about component list iteration, brain derp.)
    //========================================================================//
    if(argument.ToLower()=="scrollup")
    {
        Echo(strEchoData);
        strDisplayDataMangled = CYF.WordWrap(strDisplayData);
        strDisplayDataMangled = CYF.ScrollLoop(strDisplayDataMangled,-1,CYF.AlignCenter("Blueprint Requirements")+"\n",CYF.AlignLeftRight("","Build Progress:"+(projo.TotalBlocks-projo.RemainingBlocks).ToString()+"/"+projo.TotalBlocks));
        CYF.WriteDisplay(strDisplayDataMangled);
        return;
    }
    if(argument.ToLower()=="scrolldown")
    {
        Echo(strEchoData);
        strDisplayDataMangled = CYF.WordWrap(strDisplayData);
        strDisplayDataMangled = CYF.ScrollLoop(strDisplayDataMangled,1,CYF.AlignCenter("Blueprint Requirements")+"\n",CYF.AlignLeftRight("","Build Progress:"+(projo.TotalBlocks-projo.RemainingBlocks).ToString()+"/"+projo.TotalBlocks));
        CYF.WriteDisplay(strDisplayDataMangled);
        return;
    }
    else if(argument.ToLower()=="autoscroll=on")
    {
        boolAutoScroll = true;
        return;
    }
    else if(argument.ToLower()=="autoscroll=off")
    {
        boolAutoScroll = false;
        return;
    }
    else if(argument.ToLower()=="toggleautoscroll")
    {
        if (boolAutoScroll)
            boolAutoScroll = false;
        else
            boolAutoScroll = true;
        return;
    }
    //========================================================================//
    // END Argument Handling
    //========================================================================//
    // Output section
    Echo(strEchoData);
    //strDisplayData = CYF.FormatString(strDisplayData);
    strDisplayDataMangled = CYF.WordWrap(strDisplayData);
    if(boolAutoScroll)
        strDisplayDataMangled = CYF.ScrollLoop(strDisplayDataMangled,1,CYF.AlignCenter("Blueprint Requirements")+"\n",CYF.AlignLeftRight("","Build Progress:"+(projo.TotalBlocks-projo.RemainingBlocks).ToString()+"/"+projo.TotalBlocks));
    else
        strDisplayDataMangled = CYF.ScrollLoop(strDisplayDataMangled,0,CYF.AlignCenter("Blueprint Requirements")+"\n",CYF.AlignLeftRight("","Build Progress:"+(projo.TotalBlocks-projo.RemainingBlocks).ToString()+"/"+projo.TotalBlocks));
        //strDisplayDataMangled = CYF.AlignCenter("Blueprint Requirements")+"\n" + strDisplayDataMangled);

    //strDisplayData = CYF.ScrollLoop(strDisplayData);
    CYF.WriteDisplay(strDisplayDataMangled);
}
//============================================================================//
// Init
//============================================================================//
void Init()
{
    Echo ("Initializing System\n");
    //Get the Projector
    //var ProjectorBlocks = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyProjector>(ProjectorBlocks);
    // sort through all projector blocks found and locate the one with [BP] in the name
    if (ProjectorBlocks.Count < 1) // no projector blocks found
    {
        Echo("No projectors found on this grid.");
        boolProjectorLoading=true;
        return;
    }
    else //projectors found lets see if any of them contain our string [BPCR]
    {
        for(int i=0; i < ProjectorBlocks.Count; i++)
        {
            if(ProjectorBlocks[i].CustomName.Contains(strProjectorNameContains))
            {
                projo = (IMyProjector)ProjectorBlocks[i];
                Echo("Using " + ProjectorBlocks[i].CustomName);
                break;
            }
        }
        if(projo == null)
        {
            Echo("No projector with "+strProjectorNameContains+" in it's name");
            boolProjectorLoading=true;
            return;
        }
    }
    GetProjectorData();
    // get all cargo blocks and mash into allstorageblockslist.
    List<IMyTerminalBlock> CargoBlocks=new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(CargoBlocks);
    if(CargoBlocks.Count > 0)
    {
        for (int i=0; i < CargoBlocks.Count; i++)
        {
            AllStorageBlocks.Add(CargoBlocks[i]);
        }
    }
    CargoBlocks.Clear();

    List<IMyTerminalBlock> ConnectorBlocks=new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(ConnectorBlocks); 
    if(ConnectorBlocks.Count > 0)
    {
        for (int i=0; i < ConnectorBlocks.Count; i++)
        {
            AllStorageBlocks.Add(ConnectorBlocks[i]);
        }
    }
    ConnectorBlocks.Clear();

    List<IMyTerminalBlock> AssemblerBlocks=new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyAssembler>(AssemblerBlocks);
    if(AssemblerBlocks.Count > 0)
    {
        for (int i=0; i < AssemblerBlocks.Count; i++)
        {
            AllStorageBlocks.Add(AssemblerBlocks[i]);
        }
    }
    AssemblerBlocks.Clear();

    List<IMyTerminalBlock> WelderBlocks=new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyShipWelder>(WelderBlocks);
    if(WelderBlocks.Count > 0)
    {
        for (int i=0; i < WelderBlocks.Count; i++)
        {
            AllStorageBlocks.Add(WelderBlocks[i]);
        }
    }
    WelderBlocks.Clear();

    GetComponentCount(AllStorageBlocks);
    // flag that we've run init.
    boolInitComplete=true;
}
//============================================================================//
// help
//============================================================================//
void help()
{
    string strHelp="==== Command arguments ====\n"+
    "ScrollUp - text up 1\n"+
    "ScrollDown - text down 1\n"+
    "Autoscroll=on - enable autoscroll\n"+
    "Autoscroll=off - disable autoscroll\n"+
    "ToggleAutoscroll - toggles autoscroll\n"+
    "\n"+
    "==== Setting arguments ====\n"+
    "Settings - list settings\n"+
    "pscan - rescans projector\n"+
    "pscanon - rescans projector must be on\n"+
    "pscanonloaded - rescans projector must be on and loaded\n"+
    "reinit - rescan projector and containers\n"+
    "DebugOn - Default off\n"+
    "DebugOff";
    Echo(strHelp);
}
//============================================================================//
// EchoSettings
//============================================================================//
void EchoSettings()
{
    Echo("==== Current Settings ====");
    Echo("Using " +projo.CustomName);
    if (boolAutoScroll)
        Echo("Autoscroll: Off");
    else
        Echo("Autoscroll: On");
    if(boolDebug)
        Echo("Debug: On");
    else
        Echo("Debug: Off");
}
//========================================================================// 
// ProjectorSearch
//   Search for a projector outside of init.
//========================================================================// 
void ProjectorSearch(bool boolProjectorOn=false, bool boolProjectorLoaded=false)
{
    GridTerminalSystem.GetBlocksOfType<IMyProjector>(ProjectorBlocks);
    // sort through all projector blocks found and locate the one with [BP] in the name
    if (ProjectorBlocks.Count < 1) // no projector blocks found
    {
        Echo("No projectors found on this grid.");
        boolProjectorLoading=true;
        return;
    }
    else //projectors found lets see if any of them contain our string [BPCR]
    {
        for(int i=0; i < ProjectorBlocks.Count; i++)
        {
            if(ProjectorBlocks[i].CustomName.Contains(strProjectorNameContains))
            {
                if (boolProjectorOn)
                {
                    //if(ProjectorBlocks[i].GetValue<Boolean>("OnOff"))
                    if(ProjectorBlocks[i].IsWorking)
                    {
                        if(boolProjectorLoaded)
                        {
                            if(((IMyProjector)ProjectorBlocks[i]).IsProjecting)
                            {
                                projo = (IMyProjector)ProjectorBlocks[i];
                                Echo("Using " + ProjectorBlocks[i].CustomName);
                                break;
                            }
                        }
                        else
                        {
                            projo = (IMyProjector)ProjectorBlocks[i];
                            Echo("Using " + ProjectorBlocks[i].CustomName);
                            break;
                        }
                    }
                }
                else
                {
                    projo = (IMyProjector)ProjectorBlocks[i];
                    Echo("Using " + ProjectorBlocks[i].CustomName);
                    break;
                }
            }
        }
        if(projo == null)
        {
            Echo("No projector with "+strProjectorNameContains+" in it's name");
            boolProjectorLoading=true;
            return;
        }
    }
    GetProjectorData();
}
//========================================================================// 
// GetProjectorData
//========================================================================// 
bool GetProjectorData()
{
    if(boolDebug)
        Echo ("In Get Projector");
    dReqComponents.Clear();
    //if(!projo.GetValue<Boolean>("OnOff"))
    if(!projo.IsWorking)
    {
        boolProjectorLoading=true;
        boolProjectionComplete=false;
        return false;
    }
    // Get the correct dictionary of part depending on the projector size
    Dictionary<string, Dictionary<string, int>> dParts = new Dictionary<string, Dictionary<string, int>>();
    if((projo as IMyCubeBlock).BlockDefinition.ToString().Split('/')[1].Contains("Large"))
        dParts = dLargeShipComponentPieces; //partsDict
    else
        dParts = dSmallShipComponentPieces;
    //Get the blocks list from the projo detailed infos
    string[] strDetailedInfo = (projo as IMyTerminalBlock).DetailedInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    blocks = new List<string>(strDetailedInfo);
    if(projo.RemainingBlocks==0 && projo.IsProjecting)
    {
        boolProjectionComplete = true;
        return false;
    }
    else
        boolProjectionComplete = false;
    if(blocks.Count < 6)
    {
        //throw new Exception("\n\nThe projector doesn't seem to be projecting anything.");
        //Echo ("The projector doesn't seem to be projecting anything.");
        boolProjectorLoading=true;
        return false;
    }
    blocks.RemoveRange(0,5);
    string blockName = "";
    int amount = 0;
    //The first block of DetailedInfos should be "Armor blocks"
    blockName = blocks[0].Split(':')[0];
    amount = Int32.Parse(blocks[0].Split(':')[1]);
    if(blockName != "Armor blocks")
    {
        //throw new Exception("\n\nProjector info didn't show armor blocks. If you don't have armor blocks on the BP this could be an issue.");
        Echo ("\n\nProjector info didn't show armor blocks. If you don't have armor blocks on the BP this could be an issue.");
        return false;
    }
    // add armor blocks if statement starts @1.
    AddCompos(ref dReqComponents, amount, dParts[blockName]);
    // Iterate through projector detailed info.
    for(int i=1; i<blocks.Count; i++)
    {
        blockName = blocks[i].Split(':')[0];
        amount = Int32.Parse(blocks[i].Split(':')[1]);
        // Add a way to bug out if part is not found. and spit it to console thingie.
        if (!dParts.ContainsKey(blockName))
            Echo(blockName +" is not in the dictionary, this block will not be calculated");
        else
        {
            // if blocks are a two piece set add the second set.
            switch (blockName)
            {
                case "Wheel Suspension 1x1":
                    AddCompos(ref dReqComponents, amount, dParts["Wheel 1x1"]);
                    AddCompos(ref dReqComponents, amount, dParts[blockName]);
                    break;
                case "Wheel Suspension 3x3":
                    AddCompos(ref dReqComponents, amount, dParts["Wheel 3x3"]);
                    AddCompos(ref dReqComponents, amount, dParts[blockName]);
                    break;
                case "Wheel Suspension 5x5":
                    AddCompos(ref dReqComponents, amount, dParts["Wheel 5x5"]);
                    AddCompos(ref dReqComponents, amount, dParts[blockName]);
                    break;
                case "Rotor":
                    AddCompos(ref dReqComponents, amount, dParts["Rotor Part"]);
                    AddCompos(ref dReqComponents, amount, dParts[blockName]);
                    break;
                default:
                    AddCompos(ref dReqComponents, amount, dParts[blockName]);
                    break; 
            }
        }
    }
    //Since the projector doesn't count them, add compos for Interior Wall if there's any
    if(numberOfInteriorWall > 0)
        AddCompos(ref dReqComponents, numberOfInteriorWall, new Dictionary<string, int> {{"InteriorPlate", 25}, {"Construction", 10}});
    boolProjectorLoading = false;
    return true;
}
//========================================================================// 
// TestProjector
//   no longer used.
//========================================================================// 
bool TestProjector()
{
    if(boolDebug)
        Echo ("In Projector Test");
    //if(!projo.GetValue<Boolean>("OnOff"))
    if(!projo.IsWorking)
    {
        //Echo("Projector is Off");
        return false;
    }
    if(projo.RemainingBlocks==0)
    {
        //Echo("Projection Complete");
        return false;
    }
    string[] strDetailedInfo = (projo as IMyTerminalBlock).DetailedInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    //List<string> blocks = new List<string>(strDetailedInfo);
    blocks = new List<string>(strDetailedInfo);
    if(blocks.Count < 6)
    {
        //throw new Exception("\n\nThe projector doesn't seem to be projecting anything.");
        //Echo ("The projector doesn't seem to be projecting anything.");
        return false;
    }
    return true;
}
//========================================================================// 
// AddCompos
//    Add the amount of compos from a Dictionary to another and update the total volume
//========================================================================// 
void AddCompos(ref Dictionary<string, int> compos, int amount, Dictionary<string, int> toAdd)
{
    //For this block, get a list of compos and a list of their respective amount
    List<string> listKey = new List<string>(toAdd.Keys);
    List<int> listValue = new List<int>(toAdd.Values);
    //For each compos needed on a block
    for(int i=0; i<listKey.Count; i++)
    {
        if(compos.ContainsKey(listKey[i]))
            compos[listKey[i]] += amount * listValue[i];
        else
            compos.Add(listKey[i], amount*listValue[i]);
    }
}
//========================================================================// 
// GetComponentCount
//========================================================================// 
void GetComponentCount(List<IMyTerminalBlock> Blocks)
{
    MakedComponentsCount();// sets base for dComponentsCount and resets dict
    if (Blocks.Count > 0)
    {
        for(int i=0; i < Blocks.Count; i++)
        {
            // need to get (1) for blocks like assemblers
            //VRage.ModAPI.Ingame.IMyInventory Inventory;
            IMyInventory Inventory; // 2019031500 may need to comment this section inventory returns list not array see https://steamcommunity.com/app/244850/discussions/0/1841314700707693244/
            if(Blocks[i].BlockDefinition.ToString().Contains("Assembler"))
                Inventory = Blocks[i].GetInventory(1);
            else
                Inventory = Blocks[i].GetInventory(0);
            //var Items = Inventory.GetItems();
            List<MyInventoryItem> Items = new List<MyInventoryItem>();
            Inventory.GetItems(Items);

            if(Items.Count > 0)
            {
                for (int o=0;o <Items.Count;o++)
                {
/*
                    //1.187.089 did something with SubtypeName, private variabe for m_subtypeName, but SubtypeName is public.
                    // I don't understand why it's not accessible
                    //so we'll work around it by doing some substring magic to get the SubtypeName
                    if (boolDebug)
                        Echo("Found "+ Items[o].Amount.ToString() + Items[o].Content.SubtypeName);
                    // add to dict totals. make sure key exists so we don't dict fail.
                    if(dComponentsCount.ContainsKey(Items[o].Content.SubtypeName))
                        dComponentsCount[Items[o].Content.SubtypeName] += (int)Items[o].Amount;
*/
                    //Echo(Items[o].ToString().Substring(Items[o].ToString().IndexOf("/")+1, Items[o].ToString().Length - Items[o].ToString().IndexOf("/")-1));
                    strSubTypeForCompoCount = Items[o].ToString().Substring(Items[o].ToString().IndexOf("/")+1, Items[o].ToString().Length - Items[o].ToString().IndexOf("/")-1);
                    if (boolDebug)
                        Echo("Found "+ Items[o].Amount.ToString() + strSubTypeForCompoCount);
                    // add to dict totals. make sure key exists so we don't dict fail.
                    if(dComponentsCount.ContainsKey(strSubTypeForCompoCount))
                        dComponentsCount[strSubTypeForCompoCount] += (int)Items[o].Amount;
                }
            }
        }
    }
}
//========================================================================//
// MakedComponentsCount
//========================================================================//
void MakedComponentsCount()
{
    dComponentsCount.Clear();
    PopulateDictionary(dComponentsCount, "BulletproofGlass", 0);
    PopulateDictionary(dComponentsCount, "Computer", 0);
    PopulateDictionary(dComponentsCount, "Construction", 0);
    PopulateDictionary(dComponentsCount, "Detector", 0);
    PopulateDictionary(dComponentsCount, "Display", 0);
    PopulateDictionary(dComponentsCount, "Explosives", 0);
    PopulateDictionary(dComponentsCount, "GravityGenerator", 0);
    PopulateDictionary(dComponentsCount, "Girder", 0);
    PopulateDictionary(dComponentsCount, "InteriorPlate", 0);
    PopulateDictionary(dComponentsCount, "LargeTube", 0);
    PopulateDictionary(dComponentsCount, "Medical", 0);
    PopulateDictionary(dComponentsCount, "MetalGrid", 0);
    PopulateDictionary(dComponentsCount, "Motor", 0);
    PopulateDictionary(dComponentsCount, "PowerCell", 0);
    PopulateDictionary(dComponentsCount, "RadioCommunication", 0);
    PopulateDictionary(dComponentsCount, "Reactor", 0);
    PopulateDictionary(dComponentsCount, "SmallTube", 0);
    PopulateDictionary(dComponentsCount, "SolarCell", 0);
    PopulateDictionary(dComponentsCount, "SteelPlate", 0);
    PopulateDictionary(dComponentsCount, "Superconductor", 0);
    PopulateDictionary(dComponentsCount, "Thrust", 0);
}
//========================================================================//
// PopulateDictionary
//========================================================================//
void PopulateDictionary(Dictionary<string, int> dComp, string strComponent, int intAmount)
{
    dComp.Add(strComponent, intAmount);
}
//============================================================================//
// ClapYoFace Class!
//============================================================================//
// General Information (algorithm) :
//   Finds a display and handles all the error checking.
//   Format the Display, create progress bars, scroll the data and wordwrap!
//   Works with Text Panels, LCD, Wide LCD, Display On HUD, Antenna's and Beacons.
// Author(s) : Phil Allen phil@hilands.com https://steamcommunity.com/id/the-phil/myworkshopfiles/?appid=244850
// Last Edited By: phil@hilands.com
// Version : 2019033100
// Copyright (C) 2015,2016,2017,2018,2019  Philip Allen
//   This program is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//   This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.
//   You should have received a copy of the GNU Lesser General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
public class ClapYoFace
{
    //========================================================================//
    // ClapYoFace variables
    //========================================================================//
    // basic display stuff
    public IMyTerminalBlock DisplayBlock;
    public string strDebug = "";
    public string strError = "";
    IMyGridTerminalSystem _GridTerminalSystem = null;
    string strDisplayName ="";
    bool boolExactName=false;
    bool boolDisplayOn=true;
    bool boolMultiDisplay=false; // Not implemented. Stuck on thinking this through..
    public string strDisplayType="lcd"; // LCD - text/lcd, Block - others will change CustomName to be displayed on HUD.
    // formatting stuff
    public Dictionary<char, float> FontCharSize = new Dictionary<char, float>();
    public int intScrollLoopOffset = 0; // counter for scrollloop
    public float fltFontSize = 0;
    int intInitialDisplayWidth = 0;
    int intDisplayWidth = 0; // 93 is the arbitary standard number based on the font size. smallest char on LCD is ' which takes 93 to span 100%. All of the dictionary char sizes are based on a percentage of 93... We should probably do this off of 100 as its a cleaner base 10 number, easier to understand.
    public int intLinesPerScreen = 17;
    bool boolWordWrap = true;
    bool boolScroll = true;
    string strFontType;
    bool boolResetFontSize=false;
    //========================================================================//
    // ClapYoFace Constructor
    //========================================================================//
    public ClapYoFace(bool ExactName=false, string FontType="default")
    {
        boolExactName = ExactName;
        // need to get font type from property of LCD
        if(FontType=="default")
            intDisplayWidth = 93;
            intInitialDisplayWidth=intDisplayWidth;
        CreateFontSizeDictionary(FontType);
    }
    //========================================================================//
    // ClapYoFace Gets and Sets
    //========================================================================//
    // SetExactName - for boolExactName
    public void SetExactName(bool ExactName)
    {
        boolExactName = ExactName;
    }
    // GetExactName - for boolExactName
    public bool GetExactName()
    {
        return boolExactName;
    }
    // MultiDisplay Gets/Sets
    public void SetMultiDisplay(bool MultiDisplay)
    {
        boolMultiDisplay = MultiDisplay;
    }
    public bool GetMultiDisplay()
    {
        return boolMultiDisplay;
    }
    // DisplayType Gets/Sets
    public void SetDisplayType(string strType)
    {
        strDisplayType = strType.ToLower();
    }
    public string GetDisplayType()
    {
        return strDisplayType;
    }
    // Toggle Display OnOff
    public void SetDisplayOn(bool OnOff=true)
    {
        if(OnOff)
            boolDisplayOn=true;
        else
            boolDisplayOn=false;
    }
    public bool GetDisplayOn()
    {
        if(boolDisplayOn)
            return true;
        else
            return false;
    }
    public void SetBoolScroll(bool OnOff)
    {
        if (OnOff)
            boolScroll = true;
        else
            boolScroll = false;
    }
    public void SetBoolWordWrap(bool OnOff)
    {
        if (OnOff)
            boolWordWrap = true;
        else
            boolWordWrap = false;
    }

    //==============================   Displays   ==============================//
    //========================================================================//
    // ClapYoFace SetDisplayBlock Method - Sets the DisplayBlock and does error checking.
    //   If you just want to use the formatting feature pass your own display block through.
    //========================================================================//
    public bool SetDisplayBlock(IMyTerminalBlock Block)
    {
        // Check for display block if we haven't checked before, first make sure the display block name was not changed.
        // add workaround for cleaner HUD vs LCD.
        bool boolCheckDisplay=false;
        if (strDisplayType == "lcd")
        {
            if(DisplayBlock == null || strDisplayName != DisplayBlock.CustomName)
                boolCheckDisplay=true;
        }
        else
        {
            if(DisplayBlock == null)
                boolCheckDisplay=true;
        }
        if(boolCheckDisplay)
        {
            DisplayBlock = Block;
            if (DisplayBlock == null)
            {
                strError = "Supplied Terminal Block is Empty";
                return false;
            }
            if (!DisplayBlock.BlockDefinition.ToString().Contains("TextPanel"))
                strDisplayType = "block";
            strDisplayName = DisplayBlock.CustomName;
            return true;
        }
        else
        return true;
    }
    //========================================================================//
    // ClapYoFace SetDisplay Method - Sets the display and does error checking.
    //========================================================================//
    public bool SetDisplay(string strName, IMyGridTerminalSystem grid)
    {
        // reset debug and error message.
        strDebug = "";
        //strError = ""; 
        if(strName != strDisplayName) // check if the name matches, if code uses external variable this will force it to update.
            strDisplayName = strName;
        if(_GridTerminalSystem == null) // make sure gridterminalsystem exists, first run
            _GridTerminalSystem = grid;
        // make sure DisplayName is not empty
        if (String.IsNullOrEmpty(strDisplayName))
        {
            strError="Display Name is empty or null. Please provide the name of your display";
            return false;
        }
        // Check for display block if we haven't checked before, first make sure the display block name was not changed.
        // add workaround for cleaner HUD vs LCD.
        bool boolCheckDisplay=false;
        if (strDisplayType == "lcd")
        {
            if(DisplayBlock == null || strDisplayName != DisplayBlock.CustomName)
                boolCheckDisplay=true;
        }
        else
        {
            if(DisplayBlock == null)
                boolCheckDisplay=true;
        }
        //if(DisplayBlock == null || strDisplayName != DisplayBlock.CustomName)
        // remove check on custom name as it messes with the Show On Hud
        //if(DisplayBlock == null)
        if(boolCheckDisplay)
        {
            var DisplayBlocks = new List<IMyTerminalBlock>();
            if (boolExactName)
            {
                if(_GridTerminalSystem.GetBlockWithName(strDisplayName) == null)
                {
                    strError = "No Block found with name '"+strDisplayName+"'\n";
                    return false;
                }
                DisplayBlocks.Add(_GridTerminalSystem.GetBlockWithName(strDisplayName));//getblockwithname returns one block not a list.
            }
            else
                _GridTerminalSystem.SearchBlocksOfName(strDisplayName, DisplayBlocks);
            if (DisplayBlocks.Count == 1)
            {
                DisplayBlock = DisplayBlocks[0];
                // find object type
                /*
                    MyObjectBuilder_TextPanel/LargeLCDPanel // Large Ship Normal LCD
                    MyObjectBuilder_TextPanel/LargeLCDPanelWide // Large Ship Wide LCD
                    MyObjectBuilder_TextPanel/LargeTextPanel // Large Ship TextPanel
                */
                //if (!DisplayBlock.BlockDefinition.ToString().Contains("LCDPanel") || !DisplayBlock.BlockDefinition.ToString().Contains("TextPanel"))
                if (!DisplayBlock.BlockDefinition.ToString().Contains("TextPanel"))
                    strDisplayType = "block";
                return true;
            }
            else if (DisplayBlocks.Count > 1)
            {
                if(boolExactName)
                {
                    strError = "Multiple matches found for a block with the name of '"+strDisplayName+"'.\n\n Change the name of your block so that it does not match the names of other blocks.\n";
                }
                else
                {
                    strError = "Multiple matches found for a block with the name of '"+strDisplayName+"'.\n\n The script is currently set to  use 'SearchBlocksOfName' try adding 'if(!<object>.GetExactName()) <object>.SetExactName(true)' to your code.\n";
                }
            }
            else
                strError = "Could not find a display with the supplied name. Please validate the name of your display is '"+strDisplayName+"'. Are you the current owner of the programmable block and display?";
            return false;
        }
        else // We checked before and nothing changed.
            return true;
    }
    //========================================================================//
    // ClapYoFace WriteDisplay Method - Write input string to display
    //========================================================================//
    public void WriteDisplay(string strData)
    {
        if(strDisplayType.ToLower() == "lcd")
        {
            ((IMyTextPanel)DisplayBlock).WritePublicText(strData);
            if(((IMyTextPanel)DisplayBlock).ShowText == false)
                ((IMyTextPanel)DisplayBlock).ShowPublicTextOnScreen();
        }
        else //assume user set to Block
        {
            DisplayBlock.CustomName = strData;
            DisplayOnHud(true);
        }
    }
    //========================================================================//
    // ClearDisplay Method - Clears Display. Mainly used for DisplayOnHUDtype
    //========================================================================//
    public void ClearDisplay()
    {
        if(strDisplayType.ToLower() == "lcd")
        {
            ((IMyTextPanel)DisplayBlock).WritePublicText("");
            ((IMyTextPanel)DisplayBlock).ShowTextureOnScreen();
            ((IMyTextPanel)DisplayBlock).ShowPublicTextOnScreen();
        }
        else //assume user set to Block
        {
            if(DisplayBlock.CustomName != strDisplayName) // only reset it if it isn't the original.
            {
                //DisplayBlock.SetCustomName(strDisplayName);
                DisplayBlock.CustomName = strDisplayName;
                DisplayOnHud(false);
            }
        }
    }
    //========================================================================//
    // DisplayOnHUD - Sets Display Block to Display On HUD.
    //========================================================================//
    public void DisplayOnHud(bool OnOff)
    {
        if(OnOff)
        {
            DisplayBlock.SetValueBool("ShowOnHUD", true); 
        }
        else
        {
            DisplayBlock.SetValueBool("ShowOnHUD", false); 
        }
    }
    //========================================================================//
    // SetDisplayConfig
    //========================================================================//
    public void SetDisplayConfig()
    {
        strDebug = "";
        // this will reset everything user changes display size
        if (strDisplayType == "lcd")
        {
            if(strFontType != ((IMyTextPanel)DisplayBlock).Font)
            {
                strFontType = ((IMyTextPanel)DisplayBlock).Font;
                if(strFontType == "Monospace")
                {
                    intDisplayWidth=26;
                    intInitialDisplayWidth=intDisplayWidth;
                    intLinesPerScreen=17;
                    CreateFontSizeDictionary(strFontType);
                }
                else // assume default font, covers Debug,Red,Green,Blue,White,DarkBlue,UrlNormal,UrlHighlight,ErrorMessageBoxCaption,ErrorMessageBoxText,InfoMessageBoxCaption,InfoMessageBoxText,ScreenCaption,GameCredits,LoadingScreen,BuildInfo,BuildInfoHighlight
                {
                    intDisplayWidth=93;
                    intInitialDisplayWidth=intDisplayWidth;
                    intLinesPerScreen=17;
                    CreateFontSizeDictionary(strFontType);
                }
                boolResetFontSize=true;
            }
            if(fltFontSize != ((IMyTextPanel)DisplayBlock).FontSize || boolResetFontSize)
            {
                // get font info
                fltFontSize = ((IMyTextPanel)DisplayBlock).FontSize;
                // get Display Width
                float fltDisplayWidth = (float)intInitialDisplayWidth;
                if (DisplayBlock.BlockDefinition.ToString().Contains("LargeLCDPanelWide"))
                    fltDisplayWidth = fltDisplayWidth*2.010752f;
                fltDisplayWidth = fltDisplayWidth/fltFontSize;
                intDisplayWidth = (int)fltDisplayWidth;
                //strDebug += fltDisplayWidth.ToString()+"\n";
                //strDebug += intDisplayWidth.ToString()+"\n";
                //17.6 seems like a good standard. just int it.
                intLinesPerScreen = (int)(17.6/fltFontSize);
                boolResetFontSize=false;
            }
        }
        else // using show on hud, e.g. antenna or beacon.
        {
            if(fltFontSize == 0)
            {
                // display size keep at default.. e.g. 93.
                intDisplayWidth=93;
                // font size to one.
                fltFontSize = 1f;
                // lines are good at 17.
                intLinesPerScreen = 17;
            }
        }
        strDebug += intDisplayWidth.ToString()+"\n";
    }
    //============================   Font Sizes   ============================//
    //========================================================================//
    // CreateFontSizeDictionary
    //========================================================================//
    void CreateFontSizeDictionary(string FontType)
    {
        if (FontType=="Monospace")
        {
            FontCharSize.Clear();
            FontSizeAddCharToDictionary("\n",0f);
            FontSizeAddCharToDictionary("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz",1f);//basic letters
            FontSizeAddCharToDictionary("1234567890",1f);// numbers
            FontSizeAddCharToDictionary("!@#$%^&*()<>?,./[]{}\\|-=_+`~;':\" ",1f); // common symbols
            FontSizeAddCharToDictionary("ƒ«°",1f);// uncommon characters
            // need to set this up so it defaults non loaded
        }
        else // all the other fonts, DEBUG, etc should be this.
        {
            FontCharSize.Clear();
            FontSizeAddCharToDictionary("\n",0f);
            FontSizeAddCharToDictionary("'|",1f);
            FontSizeAddCharToDictionary(" !`Iijl",1.29166f);
            FontSizeAddCharToDictionary("(),.:;[]{}1ft",1.43076f);
            FontSizeAddCharToDictionary("\"-r",1.57627f);
            FontSizeAddCharToDictionary("*",1.72222f);
            FontSizeAddCharToDictionary("\\°",1.86f);
            FontSizeAddCharToDictionary("/",2.16279f);
            FontSizeAddCharToDictionary("«Lvx_ƒ",2.325f);
            FontSizeAddCharToDictionary("?7Jcz",2.44736f);
            FontSizeAddCharToDictionary("3FKTabdeghknopqsuy",2.58333f);
            FontSizeAddCharToDictionary("+<>=^~E",2.73529f);
            FontSizeAddCharToDictionary("#0245689CXZ",2.90625f);
            FontSizeAddCharToDictionary("$&GHPUVY",3f);
            FontSizeAddCharToDictionary("ABDNOQRS",3.20689f);
            FontSizeAddCharToDictionary("%",3.57692f);
            FontSizeAddCharToDictionary("@",3.72f);
            FontSizeAddCharToDictionary("M",3.875f);
            FontSizeAddCharToDictionary("mw",4.04347f);
            FontSizeAddCharToDictionary("W",4.65f);
        }
    }
    //========================================================================//
    // FontSizeAddCharToDictionary
    //========================================================================//
    public void FontSizeAddCharToDictionary(string CharList, float CharSize)
    {
        for (int i=0; i<CharList.Length;i++)
            FontCharSize.Add(CharList[i], CharSize);
    }
    //===================   Percentage and Progress bars   ===================//
    //========================================================================//
    // PercentFirstDecimal - find percentage return formatted "100.0%" "80.9%" etc.
    //========================================================================//
    public string PercentFirstDecimal(float Numerator, float Denominator)
    {
        float fltPercent = Numerator/Denominator;
        if (Numerator > Denominator)// check if numerator is higher and percent is 100.
            fltPercent = 1;
        return Math.Round((fltPercent*100), 1).ToString("F1")+"%";// round to first decimal
    }
    //========================================================================//
    // ProgressBar - Create a Bar [||||''] that spans the entire width
    //========================================================================//
    public string ProgressBar(float Numerator, float Denominator=-1, float fltWidth=1)
    {
        float fltPercent=0;
        if(Denominator==-1f)
            fltPercent = Numerator;
        else
            fltPercent = Numerator/Denominator;
        // Find out how much space will be used before bar.
        float fltBracketSize = StringSize("[]"); // border of bar graph.
        // find how many | or ' we should have
        float fltBarSize = ((float)intDisplayWidth)*fltWidth-fltBracketSize;
        int intBarLength = (int)(fltBarSize/StringSize("|"));
        // Find out how many "filled" bars we will have
        int intFilledBars = (int)(fltPercent*intBarLength);
        // make sure we have one bar filled if greater than 0
        if (intFilledBars == 0 && fltPercent >0)
            intFilledBars = 1;
        // make bar
        string strPercentBar = "[";
        for (int i=0;i<intBarLength;i++)
        {
            if (i<intFilledBars)
                strPercentBar += "|";
            else
                strPercentBar += "'";
        }
        strPercentBar += "]";
        return strPercentBar;
    }
    //========================================================================//
    // ProgressBarTrailingNumbers - Create a Bar "[||||''] 100.0%" Has percent on end and fills the rest.
    //========================================================================//
    public string ProgressBarTrailingNumbers(float Numerator, float Denominator=-1, float fltWidth=1)
    {
        // types: trailing %
        float fltPercent=0;
        if(Denominator==-1f)
            fltPercent = Numerator;
        else if (Numerator > Denominator)// check if numerator is higher and percent is 100.
            fltPercent = 1;
        else
            fltPercent = Numerator/Denominator;
        string strPercent = Math.Round((fltPercent*100), 1).ToString("F1")+"%";
        // Find out how much space will be used before bar.
        float fltExtraSpace = StringSize(" 100.0%"); // largest size of text percent.
        float fltBracketSize = StringSize("[]"); // border of bar graph.
        // find how many | or ' we should have
        float fltBarSize = ((float)intDisplayWidth)*fltWidth-fltExtraSpace-fltBracketSize;
        int intBarLength = (int)(fltBarSize/StringSize("|"));
        // Find out how many "filled" bars we will have
        int intFilledBars = (int)(fltPercent*intBarLength);
        // make sure we have one bar filled if greater than 0
        if (intFilledBars == 0 && fltPercent >0)
            intFilledBars = 1;
        // make bar
        string strPercentBar = "[";
        for (int i=0;i<intBarLength;i++)
        {
            if (i<intFilledBars)
                strPercentBar += "|";
            else
                strPercentBar += "'";
        }
        strPercentBar += "]";
        // find out how many space we need to get the % at the end of the line.
        float fltRightTextSize = StringSize(strPercentBar);
        float fltLeftTextSize = StringSize(strPercent);
        float fltLeftOverSpace = ((float)intDisplayWidth)*fltWidth-fltRightTextSize-fltLeftTextSize;
        int intSpacesToFill = (int)(fltLeftOverSpace/StringSize(" "));
        // add spaces to bar
        for(int i=0;i<intSpacesToFill;i++)
            strPercentBar+=" ";
        // add percent to bar
        strPercentBar+=strPercent;
        return strPercentBar;
    }
    //========================================================================//
    // AlignLeftRight - Takes two strings and aligns one on the left the other on the right.
    //========================================================================//
    public string AlignLeftRight(string Left, string Right,string TruncatePreference="left")
    {
        float fltRightTextSize = StringSize(Right);
        float fltLeftTextSize = StringSize(Left);
        float fltLeftOverSpace = ((float)intDisplayWidth)-fltRightTextSize-fltLeftTextSize;
        if (fltLeftOverSpace > 0)
        {
            int intSpacesToFill = (int)(fltLeftOverSpace/StringSize(" "));
            for(int i=0;i<intSpacesToFill;i++)
                Left+=" ";
        }
        else
        {
            if(TruncatePreference=="left")
            {
                //strDebug +="Here!\n";
                Left=StringTruncate(Left,((float)intDisplayWidth-fltRightTextSize)/intDisplayWidth);
            }
            else
                Right=StringTruncate(Right,((float)intDisplayWidth-fltLeftTextSize)/intDisplayWidth);
        }
        return Left+Right;
    }
    //========================================================================//
    // AlignCenter - Adds spaces to begining of string
    //========================================================================//
    public string AlignCenter(string Center)
    {
        float fltCenterTextSize = StringSize(Center);
        float fltLeftOverSpace = ((float)intDisplayWidth)-fltCenterTextSize;
        if (fltLeftOverSpace > 0)
        {
            int intSpacesToFill = (int)(fltLeftOverSpace/StringSize(" "));
            for(int i=0;i<intSpacesToFill/2;i++)
                Center = " "+Center; // add spaces to the front of text.
        }
        else
        {
            Center=StringTruncate(Center,((float)intDisplayWidth-fltCenterTextSize)/intDisplayWidth);
        }
        return Center;
    }
    //=========================   String Formatting   ========================//
    //========================================================================//
    // FormatString
    //========================================================================//
    public string FormatString(string strData)
    {
        if(boolWordWrap)
            strData = WordWrap(strData);
        if(boolScroll)
            strData = ScrollLoop(strData);
        return strData;
    }
    //========================================================================//
    // StringTruncate
    //========================================================================//
    public string StringTruncate(string Truncate, float Width=1, bool AtWhiteSpace=false, int intDotCount=3)
    {
        //strDebug="";
        float fltDotSize = 0;
        string strDots = "";
        if (intDotCount != 0)
        {
            for(int i=0;i<intDotCount;i++)
                strDots += ".";
            //intDotSize = (int)StringSize(".")*intDotCount;
        }
        //strDebug += StringSize(strDots).ToString()+"\n";
        //strDebug += ((int)StringSize(strDots)).ToString()+"\n";
        fltDotSize = (int)StringSize(strDots);
        int intCutAt = 0;
        if(AtWhiteSpace)
            intCutAt = StringCutLocation(Truncate, (int)(((float)(intDisplayWidth))*Width-fltDotSize));
        else
            intCutAt = StringCutLocation(Truncate, (int)(((float)(intDisplayWidth))*Width-fltDotSize), false);
        Truncate = Truncate.Substring(0,intCutAt);
        return Truncate+strDots;
    }
    //========================================================================//
    // StringSize
    //========================================================================//
    public float StringSize(string strData)
    {
        float fltTotal = 0;
        for (int i=0; i<strData.Length;i++)
        {
            fltTotal += FontCharSize[strData[i]];
        }
        return fltTotal;
    }
    //============================================================================//
    // StringCutLocation
    //============================================================================//
    public int StringCutLocation(string strData, int intDisplayWidth,bool boolCutAtSpace=true)
    {
        int intCurrentLocation = 0;
        float fltTotal = 0;
        for (int i=0; i<strData.Length;i++)
        {
            fltTotal += FontCharSize[strData[i]];
            if (fltTotal < intDisplayWidth)
                intCurrentLocation = i;
    //        else
    //            break; //this should break out of loop?
        }
        // find last space to cut.
        int intCutPosition = strData.Substring(0,intCurrentLocation).LastIndexOf(" ");
        if (intCutPosition == -1 || !boolCutAtSpace) intCutPosition = intCurrentLocation;
        return intCutPosition;
    }
    //============================================================================//
    // StringCutAddNewLine
    //============================================================================//
    public string StringCutAddNewLine(string strData, int intDisplayWidth)
    {
        string strNL = "";// NewLine
        string strT = ""; // Temp
        int intCL = StringCutLocation(strData, intDisplayWidth); // Cut Location
        strNL += strData.Substring(0,intCL)+"\n";
        strT += strData.Substring(intCL).Trim();
        //Echo (strT.Length.ToString());
        //Echo (StringSize(strT).ToString());
        if (StringSize(strT) > intDisplayWidth)
            strNL+=StringCutAddNewLine(strT,intDisplayWidth)+"\n";
        else
            strNL+=strT+"\n";
        return strNL;
    }
    //============================================================================//
    // WordWrap
    //============================================================================//
    //http://stackoverflow.com/questions/3961278/word-wrap-a-string-in-multiple-lines
    public string WordWrap(string strData)
    {
        strDebug = "";
        string strNew = "";
        // split string up by EOL
        // if string has no new line this code is poopoop.
        // add a find char \n if it doesn't exist don't do the loop.
        int intIndexOfNewLine = strData.IndexOf("\n");
        if(intIndexOfNewLine == -1)
        {
            if(StringSize(strData) > intDisplayWidth)
            {
                // needs a line break.
                strNew += StringCutAddNewLine(strData,intDisplayWidth);
            }
            else
                strNew += strData+"\n";
        }
        else
        {
            string[] arrStrData = strData.Split('\n');
            for (int i=0;i<arrStrData.Length-1;i++)
            {
                if(StringSize(arrStrData[i]) > intDisplayWidth)
                {
                    // needs a line break.
                    strNew += StringCutAddNewLine(arrStrData[i],intDisplayWidth);
                }
                else
                    strNew += arrStrData[i]+"\n";
            }
        }
        //strNew=intIndexOfNewLine.ToString();
    return strNew;
    }
    //============================================================================//
    // ScrollLoop
    //   Scrolls in continuous loop
    //============================================================================//
    // hmm would a more simple version of this, simply be taking the top line and adding it to the bottom? lol.
    public string ScrollLoop(string strData, int intScrollLines=1, string strHeader=null, string strFooter=null)
    {
    
        //return strData.IndexOf("\n").ToString();
        int intDisplayLines = intLinesPerScreen;
        string strReturn = "";
        // get lines from data.
        string[] arrData = strData.Split('\n');
        int intDataLength = arrData.Length;
        int x = 0; // recycle data for smooth looping.
        // header
        if (strHeader != null)
        {
            strReturn += strHeader+"\n";
            intDisplayLines = intDisplayLines-1;
        }
        // change counter for footer
        if (strFooter != null)
            intDisplayLines = intDisplayLines-1;
        // process data if longer than limit
        if (intDisplayLines < intDataLength) // need to truncate size.
        {
            for (int i=0; i < intDisplayLines; i++)
            {
                //strReturn += (intScrollLoopOffset+1).ToString()+" - ";
                if (i+intScrollLoopOffset < intDataLength)
                {
                    strReturn += arrData[i+intScrollLoopOffset]+"\n";
                    x=0;
                }
                else // over the line limit, need to recycle data.
                {
                    strReturn += arrData[x]+"\n";
                    x++;
                }
            }
        }
        // no need to scroll return the original data.
        else
        {
            strReturn = strReturn+strData;
        }
        if (strFooter != null)
            strReturn += strFooter+"\n";
        // set variable for
        intScrollLoopOffset+=intScrollLines;
        if ((intDataLength)-1 < intScrollLoopOffset)
        //if ((intDataLength+intScrollLines-intDisplayLines+4) < scrollOffset)
            intScrollLoopOffset=0;
        else if (intScrollLoopOffset < 0)
            // set scroll loop offset to the max length
            intScrollLoopOffset = intDataLength-1;
        // return processed data
        //strReturn=intDataLength.ToString();
        return strReturn;
    }
    //============================================================================//
    // Scroll
    //   Scrolls till reached end, or top.
    //============================================================================//
    public string Scroll(string strData, int intScrollLines=1, string strHeader=null, string strFooter=null)
    {
        int intDisplayLines = intLinesPerScreen;
        string strReturn = "";
        // get lines from data.
        string[] arrData = strData.Split('\n');
        int intDataLength = arrData.Length;
        // header
        if (strHeader != null)
        {
            strReturn += strHeader+"\n";
            intDisplayLines = intDisplayLines-1;
        }
        // change counter for footer
        if (strFooter != null)
            intDisplayLines = intDisplayLines-1;
        // process data if longer than limit
        if (intDisplayLines < intDataLength) // need to truncate size.
        {
            for (int i=0; i < intDisplayLines; i++)
            {
                //strReturn += (intScrollLoopOffset+1).ToString()+" - ";
                if (i+intScrollLoopOffset < intDataLength+intDisplayLines)
                {
                    strReturn += arrData[i+intScrollLoopOffset]+"\n";
                }
                else
                {
                    // Under/Over the line limit do nothing
                }
            }
        }
        // no need to scroll return the original data.
        else
        {
            strReturn = strData;
        }
        if (strFooter != null)
            strReturn += strFooter+"\n";
        // set variable for
        intScrollLoopOffset+=intScrollLines;
        // if we reached the end of the screen revert the offset we just gave.
        if ((intDataLength) < intScrollLoopOffset+intDisplayLines || intScrollLoopOffset < 0)
            intScrollLoopOffset=intScrollLoopOffset-=intScrollLines;
        // return processed data
        return strReturn;
    }
}
//============================================================================//
// END ClapYoFace Class!
//============================================================================//
