local MaterialData = {}
MaterialData.MaterialName = "Brick0NoiseBlend"

MaterialData.Color1 = {
    R = 1,
    G = 1,
    B = 1,
    A = 1
}
MaterialData.Diffuse1Name = "brick 6_COLOR.png"
MaterialData.Normal1Power = 1.0
MaterialData.Normal1Name = "brick 6_NRM.png"
MaterialData.Specular1Power = 0.01
MaterialData.Specular1Name = "brick 6_SPEC.png"
MaterialData.Parallax1Power = 0.01
MaterialData.Parallax1Name = "brick 6_DISP.png"

MaterialData.Color2 = {
    R = 0.5,
    G = 0.5,
    B = 0.5,
    A = 1.0
}
MaterialData.Diffuse2Name = "brick 6_COLOR.png"
MaterialData.Normal2Power = 1.0
MaterialData.Normal2Name = "brick 6_NRM.png"
MaterialData.Specular2Power = 0.1
MaterialData.Specular2Name = "brick 6_SPEC.png"
MaterialData.Parallax2Power = 0.01
MaterialData.Parallax2Name = "brick 6_DISP.png"

MaterialData.MaskName = "Mask0.png"
MaterialData.MaskPower = 1.0

MaterialData.NoiseOffset = {
    X = 0,
    Y = 0,
    Z = 0
}
MaterialData.NoisePower = 1
MaterialData.NoiseScale = 1

MaterialData.UpCheckValue = 0.1
MaterialData.UpPower = 2.0

MaterialLoader:RegisterNoiseBlendMaterial(MaterialData)