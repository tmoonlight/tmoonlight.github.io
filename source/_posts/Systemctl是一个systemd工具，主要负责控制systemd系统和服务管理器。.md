---
title: Systemctl是一个systemd工具，主要负责控制systemd系统和服务管理器。
date: 2016/7/19 19:03:52
tags:
---


Systemctl是一个systemd工具，主要负责控制systemd系统和服务管理器。

Systemd是一个系统管理守护进程、工具和库的集合，用于取代System V初始进程。Systemd的功能是用于集中管理和配置类UNIX系统。

在Linux生态系统中，Systemd被部署到了大多数的标准Linux发行版中，只有为数不多的几个发行版尚未部署。Systemd通常是所有其它守护进程的父进程，但并非总是如此。

 _使用Systemctl管理Linux服务_

本文旨在阐明在运行systemd的系统上“如何控制系统和服务”。

### Systemd初体验和Systemctl基础

#### 1\. 首先检查你的系统中是否安装有systemd并确定当前安装的版本

  1. `# systemctl --version`
  2. `systemd 215`
  3. `+PAM +AUDIT +SELINUX +IMA +SYSVINIT +LIBCRYPTSETUP +GCRYPT +ACL +XZ -SECCOMP -APPARMOR`



上例中很清楚地表明，我们安装了215版本的systemd。

#### 2\. 检查systemd和systemctl的二进制文件和库文件的安装位置

  1. `# whereis systemd`
  2. `systemd: /usr/lib/systemd /etc/systemd /usr/share/systemd /usr/share/man/man1/systemd.1.gz`
  3. `# whereis systemctl`
  4. `systemctl: /usr/bin/systemctl /usr/share/man/man1/systemctl.1.gz`



#### 3\. 检查systemd是否运行

  1. `# ps -eaf | grep [s]ystemd`
  2. `root 1 0 0 16:27 ? 00:00:00 /usr/lib/systemd/systemd --switched-root --system --deserialize 23`
  3. `root 444 1 0 16:27 ? 00:00:00 /usr/lib/systemd/systemd-journald`
  4. `root 469 1 0 16:27 ? 00:00:00 /usr/lib/systemd/systemd-udevd`
  5. `root 555 1 0 16:27 ? 00:00:00 /usr/lib/systemd/systemd-logind`
  6. `dbus 556 1 0 16:27 ? 00:00:00 /bin/dbus-daemon --system --address=systemd: --nofork --nopidfile --systemd-activation`



 **注意** ：systemd是作为父进程（PID=1）运行的。在上面带（-e）参数的ps命令输出中，选择所有进程，（-a）选择除会话前导外的所有进程，并使用（-f）参数输出完整格式列表（即 -eaf）。

也请注意上例中后随的方括号和例子中剩余部分。方括号表达式是grep的字符类表达式的一部分。

#### 4\. 分析systemd启动进程

  1. `# systemd-analyze`
  2. `Startup finished in 487ms (kernel) + 2.776s (initrd) + 20.229s (userspace) = 23.493s`



#### 5\. 分析启动时各个进程花费的时间

  1. `# systemd-analyze blame`
  2. `8.565s mariadb.service`
  3. `7.991s webmin.service`
  4. `6.095s postfix.service`
  5. `4.311s httpd.service`
  6. `3.926s firewalld.service`
  7. `3.780s kdump.service`
  8. `3.238s tuned.service`
  9. `1.712s network.service`
  10. `1.394s lvm2-monitor.service`
  11. `1.126s systemd-logind.service`
  12. `....`



#### 6\. 分析启动时的关键链

  1. `# systemd-analyze critical-chain`
  2. `The time after the unit is active or started is printed after the "@" character.`
  3. `The time the unit takes to start is printed after the "+" character.`
  4. `multi-user.target @20.222s`
  5. `└─mariadb.service @11.657s +8.565s`
  6. `└─network.target @11.168s`
  7. `└─network.service @9.456s +1.712s`
  8. `└─NetworkManager.service @8.858s +596ms`
  9. `└─firewalld.service @4.931s +3.926s`
  10. `└─basic.target @4.916s`
  11. `└─sockets.target @4.916s`
  12. `└─dbus.socket @4.916s`
  13. `└─sysinit.target @4.905s`
  14. `└─systemd-update-utmp.service @4.864s +39ms`
  15. `└─auditd.service @4.563s +301ms`
  16. `└─systemd-tmpfiles-setup.service @4.485s +69ms`
  17. `└─rhel-import-state.service @4.342s +142ms`
  18. `└─local-fs.target @4.324s`
  19. `└─boot.mount @4.286s +31ms`
  20. `└─systemd-fsck@dev-disk-by\x2duuid-79f594ad\x2da332\x2d4730\x2dbb5f\x2d85d19608096`
  21. `└─dev-disk-by\x2duuid-79f594ad\x2da332\x2d4730\x2dbb5f\x2d85d196080964.device @4`



 **重要** ：Systemctl接受服务（.service），挂载点（.mount），套接口（.socket）和设备（.device）作为单元。

#### 7\. 列出所有可用单元

  1. `# systemctl list-unit-files`
  2. `UNIT FILE STATE`
  3. `proc-sys-fs-binfmt_misc.automount static`
  4. `dev-hugepages.mount static`
  5. `dev-mqueue.mount static`
  6. `proc-sys-fs-binfmt_misc.mount static`
  7. `sys-fs-fuse-connections.mount static`
  8. `sys-kernel-config.mount static`
  9. `sys-kernel-debug.mount static`
  10. `tmp.mount disabled`
  11. `brandbot.path disabled`
  12. `.....`



#### 8\. 列出所有运行中单元

  1. `# systemctl list-units`
  2. `UNIT LOAD ACTIVE SUB DESCRIPTION`
  3. `proc-sys-fs-binfmt_misc.automount loaded active waiting Arbitrary Executable File Formats File Syste`
  4. `sys-devices-pc...0-1:0:0:0-block-sr0.device loaded active plugged VBOX_CD-ROM`
  5. `sys-devices-pc...:00:03.0-net-enp0s3.device loaded active plugged PRO/1000 MT Desktop Adapter`
  6. `sys-devices-pc...00:05.0-sound-card0.device loaded active plugged 82801AA AC'97 Audio Controller`
  7. `sys-devices-pc...:0:0-block-sda-sda1.device loaded active plugged VBOX_HARDDISK`
  8. `sys-devices-pc...:0:0-block-sda-sda2.device loaded active plugged LVM PV Qzyo3l-qYaL-uRUa-Cjuk-pljo-qKtX-VgBQ8`
  9. `sys-devices-pc...0-2:0:0:0-block-sda.device loaded active plugged VBOX_HARDDISK`
  10. `sys-devices-pl...erial8250-tty-ttyS0.device loaded active plugged /sys/devices/platform/serial8250/tty/ttyS0`
  11. `sys-devices-pl...erial8250-tty-ttyS1.device loaded active plugged /sys/devices/platform/serial8250/tty/ttyS1`
  12. `sys-devices-pl...erial8250-tty-ttyS2.device loaded active plugged /sys/devices/platform/serial8250/tty/ttyS2`
  13. `sys-devices-pl...erial8250-tty-ttyS3.device loaded active plugged /sys/devices/platform/serial8250/tty/ttyS3`
  14. `sys-devices-virtual-block-dm\x2d0.device loaded active plugged /sys/devices/virtual/block/dm-0`
  15. `sys-devices-virtual-block-dm\x2d1.device loaded active plugged /sys/devices/virtual/block/dm-1`
  16. `sys-module-configfs.device loaded active plugged /sys/module/configfs`
  17. `...`



#### 9\. 列出所有失败单元

  1. `# systemctl --failed`
  2. `UNIT LOAD ACTIVE SUB DESCRIPTION`
  3. `kdump.service loaded failed failed Crash recovery kernel arming`
  4. `LOAD = Reflects whether the unit definition was properly loaded.`
  5. `ACTIVE = The high-level unit activation state, i.e. generalization of SUB.`
  6. `SUB = The low-level unit activation state, values depend on unit type.`
  7. `1 loaded units listed. Pass --all to see loaded but inactive units, too.`
  8. `To show all installed unit files use 'systemctl list-unit-files'.`



#### 10\. 检查某个单元（如 cron.service）是否启用

  1. `# systemctl is-enabled crond.service`
  2. `enabled`



#### 11\. 检查某个单元或服务是否运行

  1. `# systemctl status firewalld.service`
  2. `firewalld.service - firewalld - dynamic firewall daemon`
  3. `Loaded: loaded (/usr/lib/systemd/system/firewalld.service; enabled)`
  4. `Active: active (running) since Tue 2015-04-28 16:27:55 IST; 34min ago`
  5. `Main PID: 549 (firewalld)`
  6. `CGroup: /system.slice/firewalld.service`
  7. `└─549 /usr/bin/python -Es /usr/sbin/firewalld --nofork --nopid`
  8. `Apr 28 16:27:51 tecmint systemd[1]: Starting firewalld - dynamic firewall daemon...`
  9. `Apr 28 16:27:55 tecmint systemd[1]: Started firewalld - dynamic firewall daemon.`



### 使用Systemctl控制并管理服务

#### 12\. 列出所有服务（包括启用的和禁用的）

  1. `# systemctl list-unit-files --type=service`
  2. `UNIT FILE STATE`
  3. `arp-ethers.service disabled`
  4. `auditd.service enabled`
  5. `autovt@.service disabled`
  6. `blk-availability.service disabled`
  7. `brandbot.service static`
  8. `collectd.service disabled`
  9. `console-getty.service disabled`
  10. `console-shell.service disabled`
  11. `cpupower.service disabled`
  12. `crond.service enabled`
  13. `dbus-org.fedoraproject.FirewallD1.service enabled`
  14. `....`



#### 13\. Linux中如何启动、重启、停止、重载服务以及检查服务（如 httpd.service）状态

  1. `# systemctl start httpd.service`
  2. `# systemctl restart httpd.service`
  3. `# systemctl stop httpd.service`
  4. `# systemctl reload httpd.service`
  5. `# systemctl status httpd.service`
  6. `httpd.service - The Apache HTTP Server`
  7. `Loaded: loaded (/usr/lib/systemd/system/httpd.service; enabled)`
  8. `Active: active (running) since Tue 2015-04-28 17:21:30 IST; 6s ago`
  9. `Process: 2876 ExecStop=/bin/kill -WINCH ${MAINPID} (code=exited, status=0/SUCCESS)`
  10. `Main PID: 2881 (httpd)`
  11. `Status: "Processing requests..."`
  12. `CGroup: /system.slice/httpd.service`
  13. `├─2881 /usr/sbin/httpd -DFOREGROUND`
  14. `├─2884 /usr/sbin/httpd -DFOREGROUND`
  15. `├─2885 /usr/sbin/httpd -DFOREGROUND`
  16. `├─2886 /usr/sbin/httpd -DFOREGROUND`
  17. `├─2887 /usr/sbin/httpd -DFOREGROUND`
  18. `└─2888 /usr/sbin/httpd -DFOREGROUND`
  19. `Apr 28 17:21:30 tecmint systemd[1]: Starting The Apache HTTP Server...`
  20. `Apr 28 17:21:30 tecmint httpd[2881]: AH00558: httpd: Could not reliably determine the server's fully q...ssage`
  21. `Apr 28 17:21:30 tecmint systemd[1]: Started The Apache HTTP Server.`
  22. `Hint: Some lines were ellipsized, use -l to show in full.`



 **注意** ：当我们使用systemctl的start，restart，stop和reload命令时，我们不会从终端获取到任何输出内容，只有status命令可以打印输出。

#### 14\. 如何激活服务并在启动时启用或禁用服务（即系统启动时自动启动服务）

  1. `# systemctl is-active httpd.service`
  2. `# systemctl enable httpd.service`
  3. `# systemctl disable httpd.service`



#### 15\. 如何屏蔽（让它不能启动）或显示服务（如 httpd.service）

  1. `# systemctl mask httpd.service`
  2. `ln -s '/dev/null' '/etc/systemd/system/httpd.service'`
  3. `# systemctl unmask httpd.service`
  4. `rm '/etc/systemd/system/httpd.service'`



#### 16\. 使用systemctl命令杀死服务

  1. `# systemctl kill httpd`
  2. `# systemctl status httpd`
  3. `httpd.service - The Apache HTTP Server`
  4. `Loaded: loaded (/usr/lib/systemd/system/httpd.service; enabled)`
  5. `Active: failed (Result: exit-code) since Tue 2015-04-28 18:01:42 IST; 28min ago`
  6. `Main PID: 2881 (code=exited, status=0/SUCCESS)`
  7. `Status: "Total requests: 0; Current requests/sec: 0; Current traffic: 0 B/sec"`
  8. `Apr 28 17:37:29 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  9. `Apr 28 17:37:29 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  10. `Apr 28 17:37:39 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  11. `Apr 28 17:37:39 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  12. `Apr 28 17:37:49 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  13. `Apr 28 17:37:49 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  14. `Apr 28 17:37:59 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  15. `Apr 28 17:37:59 tecmint systemd[1]: httpd.service: Got notification message from PID 2881, but recepti...bled.`
  16. `Apr 28 18:01:42 tecmint systemd[1]: httpd.service: control process exited, code=exited status=226`
  17. `Apr 28 18:01:42 tecmint systemd[1]: Unit httpd.service entered failed state.`
  18. `Hint: Some lines were ellipsized, use -l to show in full.`



### 使用Systemctl控制并管理挂载点

#### 17\. 列出所有系统挂载点

  1. `# systemctl list-unit-files --type=mount`
  2. `UNIT FILE STATE`
  3. `dev-hugepages.mount static`
  4. `dev-mqueue.mount static`
  5. `proc-sys-fs-binfmt_misc.mount static`
  6. `sys-fs-fuse-connections.mount static`
  7. `sys-kernel-config.mount static`
  8. `sys-kernel-debug.mount static`
  9. `tmp.mount disabled`



#### 18\. 挂载、卸载、重新挂载、重载系统挂载点并检查系统中挂载点状态

  1. `# systemctl start tmp.mount`
  2. `# systemctl stop tmp.mount`
  3. `# systemctl restart tmp.mount`
  4. `# systemctl reload tmp.mount`
  5. `# systemctl status tmp.mount`
  6. `tmp.mount - Temporary Directory`
  7. `Loaded: loaded (/usr/lib/systemd/system/tmp.mount; disabled)`
  8. `Active: active (mounted) since Tue 2015-04-28 17:46:06 IST; 2min 48s ago`
  9. `Where: /tmp`
  10. `What: tmpfs`
  11. `Docs: man:hier(7)`
  12. `http://www.freedesktop.org/wiki/Software/systemd/APIFileSystems`
  13. `Process: 3908 ExecMount=/bin/mount tmpfs /tmp -t tmpfs -o mode=1777,strictatime (code=exited, status=0/SUCCESS)`
  14. `Apr 28 17:46:06 tecmint systemd[1]: Mounting Temporary Directory...`
  15. `Apr 28 17:46:06 tecmint systemd[1]: tmp.mount: Directory /tmp to mount over is not empty, mounting anyway.`
  16. `Apr 28 17:46:06 tecmint systemd[1]: Mounted Temporary Directory.`



#### 19\. 在启动时激活、启用或禁用挂载点（系统启动时自动挂载）

  1. `# systemctl is-active tmp.mount`
  2. `# systemctl enable tmp.mount`
  3. `# systemctl disable tmp.mount`



#### 20\. 在Linux中屏蔽（让它不能启用）或可见挂载点

  1. `# systemctl mask tmp.mount`
  2. `ln -s '/dev/null' '/etc/systemd/system/tmp.mount'`
  3. `# systemctl unmask tmp.mount`
  4. `rm '/etc/systemd/system/tmp.mount'`



### 使用Systemctl控制并管理套接口

#### 21\. 列出所有可用系统套接口

  1. `# systemctl list-unit-files --type=socket`
  2. `UNIT FILE STATE`
  3. `dbus.socket static`
  4. `dm-event.socket enabled`
  5. `lvm2-lvmetad.socket enabled`
  6. `rsyncd.socket disabled`
  7. `sshd.socket disabled`
  8. `syslog.socket static`
  9. `systemd-initctl.socket static`
  10. `systemd-journald.socket static`
  11. `systemd-shutdownd.socket static`
  12. `systemd-udevd-control.socket static`
  13. `systemd-udevd-kernel.socket static`
  14. `11 unit files listed.`



#### 22\. 在Linux中启动、重启、停止、重载套接口并检查其状态

  1. `# systemctl start cups.socket`
  2. `# systemctl restart cups.socket`
  3. `# systemctl stop cups.socket`
  4. `# systemctl reload cups.socket`
  5. `# systemctl status cups.socket`
  6. `cups.socket - CUPS Printing Service Sockets`
  7. `Loaded: loaded (/usr/lib/systemd/system/cups.socket; enabled)`
  8. `Active: active (listening) since Tue 2015-04-28 18:10:59 IST; 8s ago`
  9. `Listen: /var/run/cups/cups.sock (Stream)`
  10. `Apr 28 18:10:59 tecmint systemd[1]: Starting CUPS Printing Service Sockets.`
  11. `Apr 28 18:10:59 tecmint systemd[1]: Listening on CUPS Printing Service Sockets.`



#### 23\. 在启动时激活套接口，并启用或禁用它（系统启动时自启动）

  1. `# systemctl is-active cups.socket`
  2. `# systemctl enable cups.socket`
  3. `# systemctl disable cups.socket`



#### 24\. 屏蔽（使它不能启动）或显示套接口

  1. `# systemctl mask cups.socket`
  2. `ln -s '/dev/null' '/etc/systemd/system/cups.socket'`
  3. `# systemctl unmask cups.socket`
  4. `rm '/etc/systemd/system/cups.socket'`



### 服务的CPU利用率（分配额）

#### 25\. 获取当前某个服务的CPU分配额（如httpd）

  1. `# systemctl show -p CPUShares httpd.service`
  2. `CPUShares=1024`



 **注意** ：各个服务的默认CPU分配份额=1024，你可以增加/减少某个进程的CPU分配份额。

#### 26\. 将某个服务（httpd.service）的CPU分配份额限制为2000 CPUShares/

  1. `# systemctl set-property httpd.service CPUShares=2000`
  2. `# systemctl show -p CPUShares httpd.service`
  3. `CPUShares=2000`



 **注意** ：当你为某个服务设置CPUShares，会自动创建一个以服务名命名的目录（如 httpd.service），里面包含了一个名为90-CPUShares.conf的文件，该文件含有CPUShare限制信息，你可以通过以下方式查看该文件：

  1. `# vi /etc/systemd/system/httpd.service.d/90-CPUShares.conf`
  2. `[Service]`
  3. `CPUShares=2000`



#### 27\. 检查某个服务的所有配置细节

  1. `# systemctl show httpd`
  2. `Id=httpd.service`
  3. `Names=httpd.service`
  4. `Requires=basic.target`
  5. `Wants=system.slice`
  6. `WantedBy=multi-user.target`
  7. `Conflicts=shutdown.target`
  8. `Before=shutdown.target multi-user.target`
  9. `After=network.target remote-fs.target nss-lookup.target systemd-journald.socket basic.target system.slice`
  10. `Description=The Apache HTTP Server`
  11. `LoadState=loaded`
  12. `ActiveState=active`
  13. `SubState=running`
  14. `FragmentPath=/usr/lib/systemd/system/httpd.service`
  15. `....`



#### 28\. 分析某个服务（httpd）的关键链

  1. `# systemd-analyze critical-chain httpd.service`
  2. `The time after the unit is active or started is printed after the "@" character.`
  3. `The time the unit takes to start is printed after the "+" character.`
  4. `httpd.service +142ms`
  5. `└─network.target @11.168s`
  6. `└─network.service @9.456s +1.712s`
  7. `└─NetworkManager.service @8.858s +596ms`
  8. `└─firewalld.service @4.931s +3.926s`
  9. `└─basic.target @4.916s`
  10. `└─sockets.target @4.916s`
  11. `└─dbus.socket @4.916s`
  12. `└─sysinit.target @4.905s`
  13. `└─systemd-update-utmp.service @4.864s +39ms`
  14. `└─auditd.service @4.563s +301ms`
  15. `└─systemd-tmpfiles-setup.service @4.485s +69ms`
  16. `└─rhel-import-state.service @4.342s +142ms`
  17. `└─local-fs.target @4.324s`
  18. `└─boot.mount @4.286s +31ms`
  19. `└─systemd-fsck@dev-disk-by\x2duuid-79f594ad\x2da332\x2d4730\x2dbb5f\x2d85d196080964.service @4.092s +149ms`
  20. `└─dev-disk-by\x2duuid-79f594ad\x2da332\x2d4730\x2dbb5f\x2d85d196080964.device @4.092s`



#### 29\. 获取某个服务（httpd）的依赖性列表

  1. `# systemctl list-dependencies httpd.service`
  2. `httpd.service`
  3. `├─system.slice`
  4. `└─basic.target`
  5. `├─firewalld.service`
  6. `├─microcode.service`
  7. `├─rhel-autorelabel-mark.service`
  8. `├─rhel-autorelabel.service`
  9. `├─rhel-configure.service`
  10. `├─rhel-dmesg.service`
  11. `├─rhel-loadmodules.service`
  12. `├─paths.target`
  13. `├─slices.target`
  14. `│ ├─-.slice`
  15. `│ └─system.slice`
  16. `├─sockets.target`
  17. `│ ├─dbus.socket`
  18. `....`



#### 30\. 按等级列出控制组

  1. `# systemd-cgls`
  2. `├─1 /usr/lib/systemd/systemd --switched-root --system --deserialize 23`
  3. `├─user.slice`
  4. `│ └─user-0.slice`
  5. `│ └─session-1.scope`
  6. `│ ├─2498 sshd: root@pts/0`
  7. `│ ├─2500 -bash`
  8. `│ ├─4521 systemd-cgls`
  9. `│ └─4522 systemd-cgls`
  10. `└─system.slice`
  11. `├─httpd.service`
  12. `│ ├─4440 /usr/sbin/httpd -DFOREGROUND`
  13. `│ ├─4442 /usr/sbin/httpd -DFOREGROUND`
  14. `│ ├─4443 /usr/sbin/httpd -DFOREGROUND`
  15. `│ ├─4444 /usr/sbin/httpd -DFOREGROUND`
  16. `│ ├─4445 /usr/sbin/httpd -DFOREGROUND`
  17. `│ └─4446 /usr/sbin/httpd -DFOREGROUND`
  18. `├─polkit.service`
  19. `│ └─721 /usr/lib/polkit-1/polkitd --no-debug`
  20. `....`



#### 31\. 按CPU、内存、输入和输出列出控制组

  1. `# systemd-cgtop`
  2. `Path Tasks %CPU Memory Input/s Output/s`
  3. `/ 83 1.0 437.8M - -`
  4. `/system.slice - 0.1 - - -`
  5. `/system.slice/mariadb.service 2 0.1 - - -`
  6. `/system.slice/tuned.service 1 0.0 - - -`
  7. `/system.slice/httpd.service 6 0.0 - - -`
  8. `/system.slice/NetworkManager.service 1 - - - -`
  9. `/system.slice/atop.service 1 - - - -`
  10. `/system.slice/atopacct.service 1 - - - -`
  11. `/system.slice/auditd.service 1 - - - -`
  12. `/system.slice/crond.service 1 - - - -`
  13. `/system.slice/dbus.service 1 - - - -`
  14. `/system.slice/firewalld.service 1 - - - -`
  15. `/system.slice/lvm2-lvmetad.service 1 - - - -`
  16. `/system.slice/polkit.service 1 - - - -`
  17. `/system.slice/postfix.service 3 - - - -`
  18. `/system.slice/rsyslog.service 1 - - - -`
  19. `/system.slice/system-getty.slice/getty@tty1.service 1 - - - -`
  20. `/system.slice/systemd-journald.service 1 - - - -`
  21. `/system.slice/systemd-logind.service 1 - - - -`
  22. `/system.slice/systemd-udevd.service 1 - - - -`
  23. `/system.slice/webmin.service 1 - - - -`
  24. `/user.slice/user-0.slice/session-1.scope 3 - - - -`



### 控制系统运行等级

#### 32\. 启动系统救援模式

  1. `# systemctl rescue`
  2. `Broadcast message from root@tecmint on pts/0 (Wed 2015-04-29 11:31:18 IST):`
  3. `The system is going down to rescue mode NOW!`



#### 33\. 进入紧急模式

  1. `# systemctl emergency`
  2. `Welcome to emergency mode! After logging in, type "journalctl -xb" to view`
  3. `system logs, "systemctl reboot" to reboot, "systemctl default" to try again`
  4. `to boot into default mode.`



#### 34\. 列出当前使用的运行等级

  1. `# systemctl get-default`
  2. `multi-user.target`



#### 35\. 启动运行等级5，即图形模式

  1. `# systemctl isolate runlevel5.target`
  2. `或`
  3. `# systemctl isolate graphical.target`



#### 36\. 启动运行等级3，即多用户模式（命令行）

  1. `# systemctl isolate runlevel3.target`
  2. `或`
  3. `# systemctl isolate multiuser.target`



#### 36\. 设置多用户模式或图形模式为默认运行等级

  1. `# systemctl set-default runlevel3.target`
  2. `# systemctl set-default runlevel5.target`



#### 37\. 重启、停止、挂起、休眠系统或使系统进入混合睡眠

  1. `# systemctl reboot`
  2. `# systemctl halt`
  3. `# systemctl suspend`
  4. `# systemctl hibernate`
  5. `# systemctl hybrid-sleep`



对于不知运行等级为何物的人，说明如下。

  * Runlevel 0 : 关闭系统
  * Runlevel 1 : 救援？维护模式
  * Runlevel 3 : 多用户，无图形系统
  * Runlevel 4 : 多用户，无图形系统
  * Runlevel 5 : 多用户，图形化系统
  * Runlevel 6 : 关闭并重启机器



  


到此为止吧。保持连线，进行评论。别忘了在下面的评论中为我们提供一些有价值的反馈哦。喜欢我们、与我们分享，求扩散。
