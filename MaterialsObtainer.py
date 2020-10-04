import bpy
sceneName = "Test22"

file = open("G:/Other Stuff/Archive/ModelStuff/Escaper v2.0 locations/_MaterialsData " + sceneName +".txt", "w")

checkImagesFilesNames = []

for material in bpy.data.materials:
    imageTexture = material.node_tree.nodes.get("Image Texture")
    if imageTexture != None: 
        print(material.name)
        print(imageTexture.image.name)
        file.write(sceneName + "-" + material.name)
        file.write("\n")
        imageName = imageTexture.image.name
        splits0 = imageName.split(".jpg")
        splits1 = imageName.split(".png")
        splits = None
        if len(splits0) > len(splits1):
            splits = splits0
        else:
            splits = splits1
        file.write(splits[0])
        checkImagesFilesNames.append(splits[0] + "_COLOR")
        file.write("\n")

file.close()
file = open("G:/Other Stuff/Archive/ModelStuff/Escaper v2.0 locations/_MaterialsDataNeeded " + sceneName +".txt", "w")

for imageName in checkImagesFilesNames: 
    checkFile = None
    try:
        checkFile = open("D:/Games/Unity Projects/FPS Tester/Tester/Assets/Resources/Models/Maps/Materials/Textures/" + imageName + ".jpg")
    except:
        try:
            checkFile = open("D:/Games/Unity Projects/FPS Tester/Tester/Assets/Resources/Models/Maps/Materials/Textures/" + imageName + ".png")
        except:
            file.write(imageName + "\n")
    if checkFile != None:
        checkFile.close()
        
file.close()