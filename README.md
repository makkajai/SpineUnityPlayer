### Spine Unity Player
A simple utility unity project to run skeletal animation imported from [Spine](http://esotericsoftware.com)

### Minimum requirements 
* Unity 2019.4.8f1
* spine-unity 3.8 package (20201130 release)

### How to use
1. Export anim from Spine into Unity
2. Open SpinePlayer scene
3. Copy your Spine skeletal to the scene 
4. Select the UI GameObject in hierarchy
    * drag & drop Spine SkeletalalAnimation model to AnimPlayer script's SkeletalAnim field
    * from the project panel, open the directory where your spine export are saved and drag & drop 
    the model's SkeletonData file to AnimPlayer's SkeletalDataAsset field
5. Play Unity
6. Select the track, anim, loop & speed
7. ????
8. Profit