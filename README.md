# MarvellousMouse
this project can show framworkelement around mouse

这个小Demo很简单：能够在鼠标周围随机生成格桑花，然后放大。
![image](https://user-images.githubusercontent.com/35446904/160274180-abca7c71-7a98-4749-aa3a-6ac6e2b235b2.png)


如果生成自定义样式，只需继承FrameworElement就可以了。Image本身就继承了这个类，所以常规的样式，只需要保存到Image里就可以了。  
如果要绘制自己的样式，只需要实现Onrender方法也可以（但目前没必要）
