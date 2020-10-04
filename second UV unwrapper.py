import bpy

print("Second UV unwrapper started")
for object in bpy.data.objects:
    print(object)
    try:
        print(object.data.uv_textures)
        object.select = True
        bpy.context.scene.objects.active = object
        try:
            object.data.uv_textures[1].active = True
        except Exception:
            bpy.ops.mesh.uv_texture_add()
            try:
                object.data.uv_textures[1].active = True
            except Exception:
                bpy.ops.mesh.uv_texture_add()
                object.data.uv_textures[1].active = True
        bpy.ops.uv.smart_project()
        object.data.uv_textures[0].active = True
        object.select = False
    except Exception as ex:
        print(ex)