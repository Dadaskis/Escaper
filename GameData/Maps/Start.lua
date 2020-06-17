local MapData = {}
MapData.MainModelName = "Start.obj"
MapData.Name = "Start"
--MapData.LevelTransitions = {
--    {
--        GameObjectName = "Transition_Cube.025",
--        TargetMap = "Map1"
--    }
--}

MapLoader:RegisterMap(MapData)
MapLoader:SetNewGameMap("Start")