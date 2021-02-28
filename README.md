# LinkJump
This is a Unity package for open a url with other app.  
Support iOS and android.  
  
# API
<code>LinkJump.Jump("tmall://tb.cn/xxxx");</code>  

If you want to try with more app, use parameters trySchemes:  
<code>LinkJump.Jump("tmall://tb.cn/xxxx",  new []{"taobao", "weixin", "alipay"});</code>  

If all apps are not installed, open with browser by default. And change the scheme to 'https'.  
If you do not want to open it with browser, set the 'openBrowser' false:  
<code>LinkJump.Jump("tmall://tb.cn/xxxx",  new []{"taobao", "weixin", "alipay"}, openBrowser: false);</code>  

If you want to the scheme is 'http' when open browser, set the 'https' false:  
<code>LinkJump.Jump("tmall://tb.cn/xxxx",  new []{"taobao", "weixin", "alipay"}, https: false);</code>  

If you want to know if this url can be opened:  
<code>LinkJump.CanOpenUrl(url)</code>  

### For iOS
If you want to open url with other app in iOS, you should add scheme to 'LSApplicationQueriesSchemes' in Info.plist.  
I write a script to do this with unity editor.   
Step1. click 'ftxtool' in menu, and click 'Build Queries Scheme List'.  
Step2. choose the 'QueriesSchemeList.asset' and add your scheme to the array 'Schemes'.  
Step3. build as normal, schemes will write to Info.plist automatically.  
