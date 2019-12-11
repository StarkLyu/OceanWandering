# OceanWandering
SJTU VR Project
## 鱼的使用：  
### 首先
将Resources、Animator、Scripts三个文件夹复制到Assets下  
### Fish Generator
Resources中有一个名为FishGenerator的Prefab，可以在一个球内随机位置随机生成鱼  
FishGenerator有一个名为Fish Generator的组件，其Generate Radius属性决定生成的球的大小，Generate Num属性决定生成的鱼的数量  
开始运行后，点击Gen Switch即可按照上述属性生成一次鱼  
具体生成的鱼的种类和各个种类的比例在Resources文件夹中的Configs文件夹下的FishKinds.txt文件设置，每一行格式为：[FishName] [rate]  
注：FishName对应FishPrefabs文件夹内的Prefab名字，rate没有固定总数  
