# Aop

原文地址学习：https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0

 **全局**：中间件是作用到整个应用程序的，但是过滤器的粒度更加小，可以做到到指定的方法，指定的控制器上面。

**权限**：还有就是中间件的权限比过滤器的要大，可以更改Http请求的表头，更新响应结果。

**性能**：中间件的性能要比过滤器要好，因为http请求先经过中间件再到指定的过滤器里面

# ActionFilter



# AuthorizationFilter



# ExceptionFilter



# ResourceFilter



# ResultFilter



