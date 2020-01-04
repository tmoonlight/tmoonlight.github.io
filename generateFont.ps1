
#删除字体重新生成
Remove-Item ./public/css/fonts/*.* -recurse

hexo generate

echo d | xcopy .\public\* d:\* /e /r /k /y /d

font-spider public/**/*.html

echo d | xcopy d:\css .\public\css /e /r /k /y /d
pause