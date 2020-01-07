---
title: C#treeview控件部分节点添加checkbox
date: 2017/3/21 5:00:56
tags:
---


[C# treeview控件部分节点添加checkbox](http://www.cnblogs.com/wpcnblog/p/6362666.html)

本文转载自：<http://www.cnblogs.com/xiaolifeidao/p/3178569.html>

一、先初始化treeview

this.treeView1.CheckBoxes = true;

this.treeView1.ShowLines = false;

this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;

  


this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);

二、实现委托事件

private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)

{

//隐藏节点前的checkbox

if (e.Node.ImageIndex==6)

HideCheckBox(this.treeView1, e.Node);

e.DrawDefault = true;

}

三、隐藏的实现

private const int TVIF_STATE = 0x8;

private const int TVIS_STATEIMAGEMASK = 0xF000;

private const int TV_FIRST = 0x1100;

private const int TVM_SETITEM = TV_FIRST + 63;

private void HideCheckBox(TreeView tvw, TreeNode node)

{

  


TVITEM tvi = new TVITEM();

  


tvi.hItem = node.Handle;

  


tvi.mask = TVIF_STATE;

  


tvi.stateMask = TVIS_STATEIMAGEMASK;

  


tvi.state = 0;

  


SendMessage(tvw.Handle, TVM_SETITEM,IntPtr.Zero, ref tvi);

  


}

  


[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]

  


private struct TVITEM

{

public int mask;

public IntPtr hItem;

public int state;

public int stateMask;

[MarshalAs(UnmanagedType.LPTStr)]

public string lpszText;

public int cchTextMax;

public int iImage;

public int iSelectedImage; public int cChildren; public IntPtr lParam;

}

  


[DllImport("user32.dll", CharSet = CharSet.Auto)]

  


private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

  

