---
title: 树莓派安装nsp客户端配置
date: 2014/5/15 19:06:58
tags:
---


#### Step1. 安装运行时

首先去微软官方下载镜像
    
    
    sudo wget https://download.visualstudio.microsoft.com/download/pr/87521bd8-1522-4141-9532-91d580292c42/59116d9f6ebced4fdc8b76b9e3bbabbf/dotnet-runtime-2.2.5-linux-arm.tar.gz
    sudo mkdir -p /usr/share/dotnet && sudo tar zxf dotnet-runtime-2.2.5-linux-arm.tar.gz -C /usr/share/dotnet
    sudo ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
    

安装过程无报错，运行以下指令查看dotne版本号，大于2.2即可
    
    
    sudo dotnet -version
    

#### Step2. 下载和安装程序（客户端）
    
    
    mkdir nspclient
    cd nspclient
    sudo wget https://github.com/tmoonlight/NSmartProxy/files/3342627/nspclient_v0.9.zip
    sudo unzip nspclient_v0.9.zip
    

#### Step3. 配置和运行程序（客户端）

按照下图来配置客户端的映射规则  

    
    
    sudo dotnet NSmartProxyClient.dll
    

  * P.S：新的版本对配置文件进行了简化，已经去除反向连接端口和配置端口的配置，只需要配置web端口即可。



%0A%23%23%23%23%20Step1.%20%E5%AE%89%E8%A3%85%E8%BF%90%E8%A1%8C%E6%97%B6%0A%E9%A6%96%E5%85%88%E5%8E%BB%E5%BE%AE%E8%BD%AF%E5%AE%98%E6%96%B9%E4%B8%8B%E8%BD%BD%E9%95%9C%E5%83%8F%0A%60%60%60%0Asudo%20wget%20https%3A%2F%2Fdownload.visualstudio.microsoft.com%2Fdownload%2Fpr%2F87521bd8-1522-4141-9532-91d580292c42%2F59116d9f6ebced4fdc8b76b9e3bbabbf%2Fdotnet-runtime-2.2.5-linux-arm.tar.gz%0Asudo%20mkdir%20-p%20%2Fusr%2Fshare%2Fdotnet%20%26%26%20sudo%20tar%20zxf%20dotnet-runtime-2.2.5-linux-arm.tar.gz%20-C%20%2Fusr%2Fshare%2Fdotnet%0Asudo%20ln%20-s%20%2Fusr%2Fshare%2Fdotnet%2Fdotnet%20%2Fusr%2Fbin%2Fdotnet%0A%60%60%60%0A%E5%AE%89%E8%A3%85%E8%BF%87%E7%A8%8B%E6%97%A0%E6%8A%A5%E9%94%99%EF%BC%8C%E8%BF%90%E8%A1%8C%E4%BB%A5%E4%B8%8B%E6%8C%87%E4%BB%A4%E6%9F%A5%E7%9C%8Bdotne%E7%89%88%E6%9C%AC%E5%8F%B7%EF%BC%8C%E5%A4%A7%E4%BA%8E2.2%E5%8D%B3%E5%8F%AF%0A%60%60%60%0Asudo%20dotnet%20-version%0A%60%60%60%0A%0A!%5B8ab9dda2cc67d53a21b6f273329333a1.gif%5D(en-resource%3A%2F%2Fdatabase%2F1200%3A1)%0A%23%23%23%23%20Step2.%20%E4%B8%8B%E8%BD%BD%E5%92%8C%E5%AE%89%E8%A3%85%E7%A8%8B%E5%BA%8F%EF%BC%88%E5%AE%A2%E6%88%B7%E7%AB%AF%EF%BC%89%0A%60%60%60%0Amkdir%20nspclient%0Acd%20nspclient%0Asudo%20wget%20https%3A%2F%2Fgithub.com%2Ftmoonlight%2FNSmartProxy%2Ffiles%2F3342627%2Fnspclient_v0.9.zip%0Asudo%20unzip%20nspclient_v0.9.zip%0A%60%60%60%0A!%5B0c3db4bacc40b464b240c5d0f65692a0.gif%5D(en-resource%3A%2F%2Fdatabase%2F1204%3A0)%0A%0A%23%23%23%23%20Step3.%20%E9%85%8D%E7%BD%AE%E5%92%8C%E8%BF%90%E8%A1%8C%E7%A8%8B%E5%BA%8F%EF%BC%88%E5%AE%A2%E6%88%B7%E7%AB%AF%EF%BC%89%0A%E6%8C%89%E7%85%A7%E4%B8%8B%E5%9B%BE%E6%9D%A5%E9%85%8D%E7%BD%AE%E5%AE%A2%E6%88%B7%E7%AB%AF%E7%9A%84%E6%98%A0%E5%B0%84%E8%A7%84%E5%88%99%0A!%5B6ec662c0b59f3fea5d6f506bd666bdd8.gif%5D(en-resource%3A%2F%2Fdatabase%2F1208%3A0)%0A%0A%60%60%60%0Asudo%20dotnet%20NSmartProxyClient.dll%0A%60%60%60%0A%0A!%5B332e846c7ea9d33e3c34c6bb478bb214.gif%5D(en-resource%3A%2F%2Fdatabase%2F1210%3A0)%0A%0A%20*%20P.S%EF%BC%9A%E6%96%B0%E7%9A%84%E7%89%88%E6%9C%AC%E5%AF%B9%E9%85%8D%E7%BD%AE%E6%96%87%E4%BB%B6%E8%BF%9B%E8%A1%8C%E4%BA%86%E7%AE%80%E5%8C%96%EF%BC%8C%E5%B7%B2%E7%BB%8F%E5%8E%BB%E9%99%A4%E5%8F%8D%E5%90%91%E8%BF%9E%E6%8E%A5%E7%AB%AF%E5%8F%A3%E5%92%8C%E9%85%8D%E7%BD%AE%E7%AB%AF%E5%8F%A3%E7%9A%84%E9%85%8D%E7%BD%AE%EF%BC%8C%E5%8F%AA%E9%9C%80%E8%A6%81%E9%85%8D%E7%BD%AEweb%E7%AB%AF%E5%8F%A3%E5%8D%B3%E5%8F%AF%E3%80%82%0A%0A%0A%0A%0A%0A%0A
