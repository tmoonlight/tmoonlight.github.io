

echo y | del .\public\css\fonts\*.*

hexo generate

echo d | xcopy .\public\* d:\* /e /r /k /y /d

font-spider public/**/*.html --debug
