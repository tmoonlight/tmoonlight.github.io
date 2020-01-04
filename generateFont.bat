
rem delfont
echo y | del .\public\css\fonts\*.*
rem hexo g
rem hexo g
rem hexo g
rem copgo
echo d | xcopy .\public\* d:\* /e /r /k /y /d
rem 生成字体
font-spider public/**/*.html --debug
pause