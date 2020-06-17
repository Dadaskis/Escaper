local MaterialData = {}
MaterialData.MaterialName = "WhiteDevTile"
MaterialData.DiffuseName = "DevTile_COLOR.png"
MaterialData.NormalName = "DevTile_NRM.png"
MaterialData.SpecularName = "DevTile_SPEC.png"
MaterialData.ParallaxName = "DevTile_DISP.png"
MaterialData.DetailName = ""
MaterialData.AOName = ""
MaterialData.NormalPower = 1.0
MaterialData.SpecularPower = 0.1

local Color = {}
Color.R = 1
Color.G = 1
Color.B = 1
Color.A = 1

MaterialData.Color = Color

MaterialLoader:RegisterStandardMaterial(MaterialData)