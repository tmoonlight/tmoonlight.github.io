using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdtitle_trans
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Administrator.-20171206IUEUWS\Desktop\notes\simplenote_dir");
            FileInfo[] fileInfos = di.GetFiles("*.md");

            var templateStr = @"---
title: {{title}}
date: {{date}}
tags:
---

";
            //Stream str;

            Random rand = new Random();
            DateTime dt = DateTime.Now;
            foreach (var fi in fileInfos)
            {
                dt =dt.AddDays(-rand.Next(1, 13)).AddSeconds(rand.Next(28800));
                FileStream fStream = fi.OpenRead();
                StreamReader streamReader = new StreamReader(fStream);
                var head = streamReader.ReadLine().Substring(2);
                var rest = streamReader.ReadToEnd();
                fStream.Close();
                string vhead = head.Replace("/", "").
                   Replace("\\", "").Replace(":", "").Replace("<", "").Replace(">", "").Replace(" ", "").Replace("\"", "").Replace("?", "").Replace("*", "");
                //创建文件并且写入da
                File.WriteAllText(
                    @"C:\Users\Administrator.-20171206IUEUWS\Desktop\notes\export\" +
                   vhead
                    + ".md",
                    templateStr.Replace("{{title}}", vhead).Replace("{{date}}", dt.ToString()) + rest);
            }
        }
    }
}
