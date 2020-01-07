---
title: docker-engine的安装方法
date: 2017/11/4 7:01:37
tags:
---


docker-engine的安装方法

# Get Docker CE for CentOS

Estimated reading time: 10 minutes

To get started with Docker CE on CentOS, make sure you [meet the prerequisites](https://docs.docker.com/install/linux/docker-ce/centos/#prerequisites), then [install Docker](https://docs.docker.com/install/linux/docker-ce/centos/#install-docker-ce).

## Prerequisites

### Docker EE customers

To install Docker Enterprise Edition (Docker EE), go to [Get Docker EE for CentOS](https://docs.docker.com/install/linux/docker-ee/centos/) instead of this topic.

To learn more about Docker EE, see [Docker Enterprise Edition](https://www.docker.com/enterprise-edition/).

### OS requirements

To install Docker CE, you need a maintained version of CentOS 7. Archived versions aren’t supported or tested.

The centos-extras repository must be enabled. This repository is enabled by default, but if you have disabled it, you need to [re-enable it](https://wiki.centos.org/AdditionalResources/Repositories).

The overlay2 storage driver is recommended.

### Uninstall old versions

Older versions of Docker were called docker or docker-engine. If these are installed, uninstall them, along with associated dependencies.

$ sudo yum remove docker \

docker-client \

docker-client-latest \

docker-common \

docker-latest \

docker-latest-logrotate \

docker-logrotate \

docker-selinux \

docker-engine-selinux \

docker-engine

It’s OK if yum reports that none of these packages are installed.

The contents of /var/lib/docker/, including images, containers, volumes, and networks, are preserved. The Docker CE package is now called docker-ce.

## Install Docker CE

You can install Docker CE in different ways, depending on your needs:

  * Most users [set up Docker’s repositories](https://docs.docker.com/install/linux/docker-ce/centos/#install-using-the-repository) and install from them, for ease of installation and upgrade tasks. This is the recommended approach.
  * Some users download the RPM package and [install it manually](https://docs.docker.com/install/linux/docker-ce/centos/#install-from-a-package) and manage upgrades completely manually. This is useful in situations such as installing Docker on air-gapped systems with no access to the internet.
  * In testing and development environments, some users choose to use automated [convenience scripts](https://docs.docker.com/install/linux/docker-ce/centos/#install-using-the-convenience-script) to install Docker.



### Install using the repository

Before you install Docker CE for the first time on a new host machine, you need to set up the Docker repository. Afterward, you can install and update Docker from the repository.

#### SET UP THE REPOSITORY

  1. Install required packages. yum-utils provides the yum-config-manager utility, and device-mapper-persistent-data and lvm2are required by the devicemapper storage driver.  


$ sudo yum install -y yum-utils \

device-mapper-persistent-data \

lvm2

  2. Use the following command to set up the stable repository. You always need the stable repository, even if you want to install builds from the edge or test repositories as well.  


$ sudo yum-config-manager \

\--add-repo \

<https://download.docker.com/linux/centos/docker-ce.repo>

  3. Optional: Enable the edge and test repositories. These repositories are included in the docker.repo file above but are disabled by default. You can enable them alongside the stable repository.  


$ sudo yum-config-manager \--enable docker-ce-edge

  


$ sudo yum-config-manager \--enable docker-ce-test

  
You can disable the edge or test repository by running the yum-config-manager command with the \--disable flag. To re-enable it, use the \--enable flag. The following command disables the edge repository.  


$ sudo yum-config-manager \--disable docker-ce-edge

  


Note: Starting with Docker 17.06, stable releases are also pushed to the edge and test repositories.

  
[Learn about](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[stable](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[and](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[edge](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[builds](https://docs.docker.com/install/).



#### INSTALL DOCKER CE

  1. Install the latest version of Docker CE, or go to the next step to install a specific version.  


$ sudo yum install docker-ce

  


Warning: If you have multiple Docker repositories enabled, installing or updating without specifying a version in the yum install or yum update command always installs the highest possible version, which may not be appropriate for your stability needs.

  
If this is the first time you are installing a package from a recently added repository, you are prompted to accept the GPG key, and the key’s fingerprint is shown. Verify that the fingerprint is correct, and if so, accept the key. The fingerprint should match060A 61C5 1B55 8A7F 742B 77AA C52F EB6B 621E 9F35.  
Docker is installed but not started. The docker group is created, but no users are added to the group.
  2. On production systems, you should install a specific version of Docker CE instead of always using the latest. List the available versions. This example uses the sort -r command to sort the results by version number, highest to lowest, and is truncated.  


$ yum list docker-ce \--showduplicates | sort -r

  


docker-ce.x86_64 17.12.ce-1.el7.centos docker-ce-stable

  
The contents of the list depend upon which repositories are enabled, and are specific to your version of CentOS (indicated by the .el7 suffix on the version, in this example). Choose a specific version to install. The second column is the version string. You can use the entire version string, but you need to include at least to the first hyphen. The third column is the repository name, which indicates which repository the package is from and by extension its stability level. To install a specific version, append the version string to the package name and separate them by a hyphen (-).  


Note: The version string is the package name plus the version up to the first hyphen. In the example above, the fully qualified package name is docker-ce-17.06.1.ce.

  


$ sudo yum install <FULLY-QUALIFIED-PACKAGE-NAME>

  3. Start Docker.  


$ sudo systemctl start docker

  4. Verify that docker is installed correctly by running the hello-world image.  


$ sudo docker run hello-world

  
This command downloads a test image and runs it in a container. When the container runs, it prints an informational message and exits.



Docker CE is installed and running. You need to use sudo to run Docker commands. Continue to [Linux postinstall](https://docs.docker.com/install/linux/linux-postinstall/) to allow non-privileged users to run Docker commands and for other optional configuration steps.

#### UPGRADE DOCKER CE

To upgrade Docker CE, follow the [installation instructions](https://docs.docker.com/install/linux/docker-ce/centos/#install-docker), choosing the new version you want to install.

### Install from a package

If you cannot use Docker’s repository to install Docker, you can download the .rpm file for your release and install it manually. You need to download a new file each time you want to upgrade Docker.

  1. Go to <https://download.docker.com/linux/centos/7/x86_64/stable/Packages/> and download the .rpm file for the Docker version you want to install.  


Note: To install an edge package, change the word stable in the above URL to edge. [Learn about](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[stable](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[and](https://docs.docker.com/install/)[ ](https://docs.docker.com/install/)[edge](https://docs.docker.com/install/)[channels](https://docs.docker.com/install/).

  2. Install Docker CE, changing the path below to the path where you downloaded the Docker package.  


$ sudo yum install /path/to/package.rpm

  
Docker is installed but not started. The docker group is created, but no users are added to the group.
  3. Start Docker.  


$ sudo systemctl start docker

  4. Verify that docker is installed correctly by running the hello-world image.  


$ sudo docker run hello-world

  
This command downloads a test image and runs it in a container. When the container runs, it prints an informational message and exits.



Docker CE is installed and running. You need to use sudo to run Docker commands. Continue to [Post-installation steps for Linux](https://docs.docker.com/install/linux/linux-postinstall/) to allow non-privileged users to run Docker commands and for other optional configuration steps.

#### UPGRADE DOCKER CE

To upgrade Docker CE, download the newer package file and repeat the [installation procedure](https://docs.docker.com/install/linux/docker-ce/centos/#install-from-a-package), using yum -y upgrade instead of yum -y install, and pointing to the new file.

### Install using the convenience script

Docker provides convenience scripts at [get.docker.com](https://get.docker.com/) and [test.docker.com](https://test.docker.com/) for installing edge and testing versions of Docker CE into development environments quickly and non-interactively. The source code for the scripts is in the [docker-install](https://github.com/docker/docker-install)[ ](https://github.com/docker/docker-install)[repository](https://github.com/docker/docker-install). Using these scripts is not recommended for production environments, and you should understand the potential risks before you use them:

  * The scripts require root or sudo privileges to run. Therefore, you should carefully examine and audit the scripts before running them.
  * The scripts attempt to detect your Linux distribution and version and configure your package management system for you. In addition, the scripts do not allow you to customize any installation parameters. This may lead to an unsupported configuration, either from Docker’s point of view or from your own organization’s guidelines and standards.
  * The scripts install all dependencies and recommendations of the package manager without asking for confirmation. This may install a large number of packages, depending on the current configuration of your host machine.
  * The script does not provide options to specify which version of Docker to install, and installs the latest version that is released in the “edge” channel.
  * Do not use the convenience script if Docker has already been installed on the host machine using another mechanism.



This example uses the script at [get.docker.com](https://get.docker.com/) to install the latest release of Docker CE on Linux. To install the latest testing version, use [test.docker.com](https://test.docker.com/) instead. In each of the commands below, replace each occurrence of get with test.

> Warning:
> 
> Always examine scripts downloaded from the internet before running them locally.

$ curl -fsSL[get.docker.com](http://get.docker.com/)-o get-docker.sh

$ sudo sh get-docker.sh

  


<output truncated>

  


If you would like to use Docker as a non-root user, you should now consider

adding your user to the "docker" group with something like:

  


sudo usermod -aG docker your-user

  


Remember to log out and back in for this to take effect!

  


WARNING: Adding a user to the "docker" group grants the ability to run

containers which can be used to obtain root privileges on the

docker host.

Refer to <https://docs.docker.com/engine/security/security/#docker-daemon-attack-surface>

for more information.

Docker CE is installed. It starts automatically on DEB-based distributions. On RPM-based distributions, you need to start it manually using the appropriate systemctl or service command. As the message indicates, non-root users can’t run Docker commands by default.

#### UPGRADE DOCKER AFTER USING THE CONVENIENCE SCRIPT

If you installed Docker using the convenience script, you should upgrade Docker using your package manager directly. There is no advantage to re-running the convenience script, and it can cause issues if it attempts to re-add repositories which have already been added to the host machine.

## Uninstall Docker CE

  1. Uninstall the Docker package:  


$ sudo yum remove docker-ce

  2. Images, containers, volumes, or customized configuration files on your host are not automatically removed. To delete all images, containers, and volumes:  


$ sudo rm -rf /var/lib/docker




You must delete any edited configuration files manually.

## Next steps

  * Continue to [Post-installation steps for Linux](https://docs.docker.com/install/linux/linux-postinstall/)
  * Continue with the [User Guide](https://docs.docker.com/engine/userguide/).



[requirements](https://docs.docker.com/glossary/?term=requirements), [apt](https://docs.docker.com/glossary/?term=apt), [installation](https://docs.docker.com/glossary/?term=installation), [centos](https://docs.docker.com/glossary/?term=centos), [rpm](https://docs.docker.com/glossary/?term=rpm), [install](https://docs.docker.com/glossary/?term=install), [uninstall](https://docs.docker.com/glossary/?term=uninstall), [upgrade](https://docs.docker.com/glossary/?term=upgrade), [update](https://docs.docker.com/glossary/?term=update)

Rate this page:

 

43

 

3

 

  

