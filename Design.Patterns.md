Design.Patterns 设计模式系列

https://www.cnblogs.com/zhili/p/StatePattern.html  高质量参考博客

# 简单工厂模式 SimpleFactory

为什么要学习这个模式？

- **面试**： 面试需要问
- **提高编程水平**： 提高代码编写的水平，还有代码的简洁性

解决什么问题？

1.**方便扩展**：创建类的时候方便扩展多个类，一个方法统一获取所有类型，获取扩展方法的类型，直接更改工厂方法就可以了

2.**封装/隐藏类的创建**：我看SqlSugar的源码里面有单独使用GetFactory的方法的，就是把类的创建隐藏到一个方法中，如果类的创建逻辑有变动的话方便改动

参考学习：https://www.cnblogs.com/zhili/p/SimpleFactory.html  #设计模式(2)——简单工厂模式 