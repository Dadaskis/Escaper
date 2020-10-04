import bpy

sceneName = bpy.path.basename(bpy.context.blend_data.filepath)
sceneName = sceneName.replace(".blend", "")

locationsArchivePath = "G:/Other Stuff/Archive/ModelStuff/Escaper v2.0 locations/"
projectMapMaterialsPath = "D:/Games/Unity Projects/FPS Tester/Tester/Assets/Resources/Models/Maps/Materials/Textures/" 

file = open(locationsArchivePath + "_MaterialsData " + sceneName +".txt", "w")

checkImagesFilesNames = []

for material in bpy.data.materials:
    imageTexture = material.node_tree.nodes.get("Image Texture")
    if imageTexture != None: 
        print(material.name)
        print(imageTexture.image.name)
        file.write(sceneName + "-" + material.name)
        file.write("\n")
        imageName = imageTexture.image.name
        # close your eyes
        splits0 = imageName.split(".jpg")
        splits1 = imageName.split(".png")
        splits = None
        if len(splits0) > len(splits1):
            splits = splits0
        else:
            splits = splits1
        # open your eyes
        file.write(splits[0])
        checkImagesFilesNames.append(splits[0])
        file.write("\n")

file.close()
file = open(locationsArchivePath + "_MaterialsDataNeeded " + sceneName +".txt", "w")

blacklist = {}

for imageName in checkImagesFilesNames: 
    checkFile = None
    try:
        checkFile = open(projectMapMaterialsPath + imageName + "_COLOR.jpg")
    except:
        try:
            checkFile = open(projectMapMaterialsPath + imageName + "_COLOR.png")
        except:
            if blacklist.get(imageName) == None:
                file.write(imageName + "\n")
                blacklist.update({ imageName : True })
    if checkFile != None:
        checkFile.close()
        
file.close()