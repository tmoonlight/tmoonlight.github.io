---
title: WinForm界面开发之布局控件WeifenLuo.WinFormsUI.Docking的使用
date: 2016/11/21 5:38:13
tags:
---


## [WinForm界面开发之布局控件"WeifenLuo.WinFormsUI.Docking"的使用](http://www.cnblogs.com/wuhuacong/archive/2009/07/09/1520082.html)

本篇介绍Winform程序开发中的布局界面的设计，介绍如何在我的共享软件中使用布局控件"WeifenLuo.WinFormsUI.Docking"。

布局控件"WeifenLuo.WinFormsUI.Docking"是一个非常棒的开源控件，用过的人都深有体会，该控件之强大、美观、不亚于商业控件。而且控件使用也是比较简单的。先看看控件使用的程序界面展示效果。

配电网络可视化管理系统的界面截图：

深田之星送水管理系统网络版的界面截图：

我在几个共享软件都使用了该布局控件，我们先以“深田之星送水管理系统网络版”这款软件为例，介绍如何完成该界面的设计及显示的。

1、首先，我们添加一个主界面窗体，命名为MainForm，该窗体IsMdiContainer设置为True，也就是设置为多文档窗体格式。拖拉布局控件"WeifenLuo.WinFormsUI.Docking.DockPanel"到主窗体MainForm中，并设置下面几个属性：

Dock为Fill、DocumentStyle为DockingMdi、RightToLeftLayout为True。

这几个属性的意思应该不难，Dock就是 覆盖整个MDI窗体的区域，DocumentStyle为多文档类型、RightToLeftLayout是指新打开的窗口都停靠在右边区域。

我们看看设计界面视图如下所示。

2、主界面其实基本上就可以了，另外我们看到“送水管理系统网络版”的界面中有一个左边的工具栏，它其实也是在一个停靠的窗体中的，我们增加一个窗体用来承载相关的工具快捷键按钮展示。命名为MainToolWindow的窗体，继承自WeifenLuo.WinFormsUI.Docking.DockContent.

其中的“HideOnClose”属性很重要，该属性一般设置为True，就是指你关闭窗口时，窗体只是隐藏而不是真的关闭。

左边的窗口MainToolWindow实现停靠的代码是在MainForm的构造函数或者Load函数中加载即可。

mainToolWin.Show(this.dockPanel, DockState.DockLeft);

 3、对于工具窗口我们已经完成了，但是主业务窗口还没有做，也就是下面的部分内容。

为了方便，我们定义一个基类窗体，命名为BaseForm，继承自DockContent，如下所示

public class BaseForm : DockContent

然后每个业务窗口继承BaseForm即可。

4、剩下的内容就是如何在主窗体MainForm中展示相关的业务窗口了，展示的代码如下所示

    public partial class MainForm : Form

    {

        #region 属性字段

  


         private MainToolWindow mainToolWin = new MainToolWindow();

        private FrmProduct frmProduct = new FrmProduct();

        private FrmCustomer frmCustomer = new FrmCustomer();

        private FrmOrder frmOrder = new FrmOrder();

        private FrmStock frmStock = new FrmStock();

        private FrmComingCall frmComingCall = new FrmComingCall();

        private FrmDeliving frmDeliving = new FrmDeliving();

        private FrmTicketHistory frmHistory = new FrmTicketHistory(); 

  


        #endregion

  


        public MainForm()

        {

            InitializeComponent();

  


            SplashScreen.Splasher.Status = "正在展示相关的内容";

            System.Threading.Thread.Sleep(100);

  


            mainToolWin.Show(this.dockPanel, DockState.DockLeft);

            frmComingCall.Show(this.dockPanel);

            frmDeliving.Show(this.dockPanel);

            frmHistory.Show(this.dockPanel);

            frmStock.Show(this.dockPanel);

            frmProduct.Show(this.dockPanel);

            frmCustomer.Show(this.dockPanel);

            frmOrder.Show(this.dockPanel);

  


            SplashScreen.Splasher.Status = "初始化完毕";

            System.Threading.Thread.Sleep(50);

  


            SplashScreen.Splasher.Close();

        }

  


 

5.下面贴出基本窗口的基本操作事件函数

        private void menu_Window_CloseAll_Click(object sender, EventArgs e)

        {

            CloseAllDocuments();

        }

  


        private void menu_Window_CloseOther_Click(object sender, EventArgs e)

        {

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)

            {

                Form activeMdi = ActiveMdiChild;

                foreach (Form form in MdiChildren)

                {

                    if (form != activeMdi)

                    {

                        form.Close();

                    }

                }

            }

            else

            {

                foreach (IDockContent document in dockPanel.DocumentsToArray())

                {

                    if (!document.DockHandler.IsActivated)

                    {

                        document.DockHandler.Close();

                    }

                }

            }

        }

  


        private DockContent FindDocument(string text)

        {

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)

            {

                foreach (Form form in MdiChildren)

                {

                    if (form.Text == text)

                    {

                        return form as DockContent;

                    }

                }

  


                return null;

            }

            else

            {

                foreach (DockContent content in dockPanel.Documents)

                {

                    if (content.DockHandler.TabText == text)

                    {

                        return content;

                    }

                }

  


                return null;

            }

        }

  


        public DockContent ShowContent(string caption, Type formType)

        {

            DockContent frm = FindDocument(caption);

            if (frm == null)

            {

                frm = ChildWinManagement.LoadMdiForm(Portal.gc.MainDialog, formType) as DockContent;

            }

  


            frm.Show(this.dockPanel);

            frm.BringToFront();

            return frm;

        }

  


        public void CloseAllDocuments()

        {

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)

            {

                foreach (Form form in MdiChildren)

                {

                    form.Close();

                }

            }

            else

            {

                IDockContent[] documents = dockPanel.DocumentsToArray();

                foreach (IDockContent content in documents)

                {

                    content.DockHandler.Close();

                }

            }

        } 

 

最后呈上该控件文件，大家可以下来玩玩。

<http://files.cnblogs.com/wuhuacong/WeifenLuo.WinFormsUI.Docking.rar>

  

