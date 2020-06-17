local MapData = {}
MapData.MainModelName = "Map0.obj"
MapData.Name = "Map0"
MapData.LevelTransitions = {
    {
        GameObjectName = "Transition_Cube.025",
        TargetMap = "Map1"
    }
}

MapLoader:RegisterMap(MapData)
--MapLoader:SetNewGameMap("Map0")