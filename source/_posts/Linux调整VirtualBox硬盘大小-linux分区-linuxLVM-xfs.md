---
title: Linux调整VirtualBox硬盘大小-linux分区-linuxLVM-xfs
date: 2017/10/2 2:11:16
tags:
---


VirtualBox虚拟机在使用的过程中，有时会遇到磁盘大小不够用，如果设置了”动态分配存储“，可以通过下面的方法对磁盘的空间进行手动扩展。

1.启动CMD命令行，进入VirtualBox的安装目录。如

cd E:\Program Files\[Oracle](http://www.linuxidc.com/topicnews.aspx?tid=12 "Oracle")\VirtualBox

2.查看需要修改的虚拟硬盘：

E:\Program Files\Oracle\VirtualBox>VBoxManage.exe list hdds

UUID:          e8e2c341-b3b1-49db-ad2d-ab4e6b08bc5a

Parent UUID:    base

State:          locked write

Type:          normal (base)

Location:      F:\VM\[CentOS](http://www.linuxidc.com/topicnews.aspx?tid=14 "CentOS")-64.vdi

Storage format: VDI

Capacity:      8000 MBytes

UUID:          707d45b6-380d-4e51-96bd-8c9508bfd313

Parent UUID:    base

State:          created

Type:          normal (base)

Location:      F:\VM\CentOS-64-ext.vdi

Storage format: VDI

Capacity:      21273 MBytes

UUID:          aca81637-fbc0-4826-be66-847ecc96d83b

Parent UUID:    base

State:          created

Type:          normal (base)

Location:      C:\Users\Edward.Wu\VirtualBox VMs\WinXP\WinXP.vdi

Storage format: VDI

Capacity:      10240 MBytes

\----

我们看到共有三个虚拟磁盘，我们要修改图中第一个，它的空间大小为8G，UUID：e8e2c341-b3b1-49db-ad2d-ab4e6b08bc5a 

2\. 调整磁盘空间为15G：

E:\Program Files\Oracle\VirtualBox>VBoxManage.exe modifyhd e8e2c341-b3b1-49db-ad2d-ab4e6b08bc5a \--resize 150000%...10%...20%...30%...40%...50%...60%...70%...80%...90%...100%

3.重新查看：

E:\Program Files\Oracle\VirtualBox>VBoxManage.exe list hdds UUID:          e8e2c341-b3b1-49db-ad2d-ab4e6b08bc5a Parent UUID:    base State:          locked write Type:          normal (base) Location:      F:\VM\CentOS-64.vdi Storage format: VDI Capacity:      15000 MBytes

UUID:          707d45b6-380d-4e51-96bd-8c9508bfd313 Parent UUID:    base State:          created Type:          normal (base) Location:      F:\VM\CentOS-64-ext.vdi Storage format: VDI Capacity:      21273 MBytes

UUID:          aca81637-fbc0-4826-be66-847ecc96d83b Parent UUID:    base State:          created Type:          normal (base) Location:      C:\Users\Edward.Wu\VirtualBox VMs\WinXP\WinXP.vdi Storage format: VDI Capacity:      10240 MBytes

4.查看新的磁盘空间

重新启动虚拟机，查看磁盘情况。

[root@aimin ~]# fdisk -l /dev/sda

Disk /dev/sda: 15.7 GB, 15728640000 bytes 255 heads, 63 sectors/track, 1912 cylinders

可以看到磁盘空间已经扩展到15G，但这时还不可以使用。

5.Enable新增加的空间

使用 fdisk 将虚拟磁盘的空闲空间创建为一个新的分区。注意要使用代表 Linux LVM 的分区号 8e 来作为 ID。

# fdisk /dev/sda

n {new partition}

p {primary partition}

3 {partition number}

[这时会提示修改大小，选择默认直接回车即可]

t {change partition id}

3 {partition number}

8e {Linux LVM partition}

w

\------

如果中间有设置大小之类的提示，就直接回车。

完成后，如果提示：

WARNING: Re-reading the partition table failed with error 16: 设备或资源忙. The kernel still uses the old table. The new table will be used at the next reboot or after you run partprobe(8) or kpartx(8)

就重启一下系统。

6.查看新增加的sda3是否标记为LVM，如果没有需要reboot

#fdisk -l /dev/sda

7.调整LVM大小

先看一下Volume Group名称

[root@aimin ~]# vgdisplay   \--- Volume group ---   VG Name              vg_aimin

....

vg_aimin是我的VolumeGroup的名称，实际操作时，需要使用实际显示的名称。

8.把新分配的空间创建一个新的物理卷

#pvcreate /dev/sda3

9.然后使用新的物理卷来扩展 LVM 的 VolGroup，

# vgextend vg_aimin /dev/sda3

No physical volume label read from /dev/sda3

Writing physical volume data to disk "/dev/sda3"

Physical volume "/dev/sda3" successfully created

Volume group "vg_aimin" successfully extended

10.然后扩展 LVM 的逻辑卷 vg_aimin-lv_root，

# lvextend /dev/vg_aimin/lv_root /dev/sda3

11.调整逻辑卷的大小

#resize2fs /dev/vg_aimin/lv_root

到这里就完成了空间的扩展。

12.查看效果

[root@aimin ~]# df -h文件系统              容量  已用  可用 已用%% 挂载点/dev/mapper/vg_aimin-lv_root                      12G  5.2G  6.2G  46% /tmpfs                499M  80K  499M  1% /dev/shm/dev/sda1            485M  33M  427M  8% /boot

成功扩展！！！

(12步时仍未解决问题)

因为文件系统是xps的

[root@localhost ~]# df -h -T

文件系统                类型      容量  已用  可用 已用% 挂载点

/dev/mapper/centos-root xfs       9.4G  6.3G  3.2G   67% /

devtmpfs                devtmpfs  904M     0  904M    0% /dev

tmpfs                   tmpfs     920M     0  920M    0% /dev/shm

tmpfs                   tmpfs     920M  8.8M  911M    1% /run

tmpfs                   tmpfs     920M     0  920M    0% /sys/fs/cgroup

/dev/sda1               xfs       497M  223M  274M   45% /boot

tmpfs                   tmpfs     184M   12K  184M    1% /run/user/42

tmpfs                   tmpfs     184M     0  184M    0% /run/user/0

使用如下命令解决问题:

[root@localhost ~]# xfs_growfs /dev/mapper/centos-root

  


  


以上是centos调整分区的方法 debian又是另外的方法

  


  


  

