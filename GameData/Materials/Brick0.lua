local MaterialData = {}
MaterialData.MaterialName = "Brick0"
MaterialData.DiffuseName = "brick 6_COLOR.png"
MaterialData.NormalName = "brick 6_NRM.png"
MaterialData.SpecularName = "brick 6_SPEC.png"
MaterialData.ParallaxName = "brick 6_DISP.png"
MaterialData.DetailName = ""
MaterialData.AOName = ""
MaterialData.NormalPower = 1.0
MaterialData.SpecularPower = 0.1

local Color = {}
Color.R = 1
Color.G = 0.5
Color.B = 0.1
Color.A = 1

MaterialData.Color = Color

MaterialLoader:RegisterStandardMaterial(MaterialData)