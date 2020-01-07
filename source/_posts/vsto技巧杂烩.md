---
title: vsto技巧杂烩
date: 2018/3/11 22:49:20
tags:
---


在excel开发中一些常用的方法。

excel的索引是从1开始的，单元格个行列，worksheet顺序等等

获取选择的单元格

Excel.Range r = (Excel.Range)Application.ActiveCell;

  


  


获取指定单元格

wks是当前活动的Worksheet

Excel.Range cell1 = (Excel.Range)wks.Cells[next_row, 1];

  


删除一行

Excel.Range sel_row_range = (Excel.Range)wks.Rows["2:2", Type.Missing];

//删除第二行

sel_row_range.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

  


设置range颜色

Excel.Range range = this.Cells[1, 1];

range.Interior.Color = 0x00ff00;

  


  


0x0000ff 红色

0x00ff00 绿色

0xff0000 蓝色

  


设置单元格计算函数

//显示计算结果的单元格

Excel.Range math_range = get_Range("A5","A5");

//设置单元格表达式

math_range.FormulaR1C1 = "=CalculateArea(R[-2]C[-1]:R[-2]C)";

R[-2]C[-1]:R[-2]C

意思是，从当前单元格开始算行数为当前行-2，列数为当前列-1，到达单元格为当前行-2，和当前列相等

  


设置单元格类型

//设置单元格类型为文本

cell2.NumberFormatLocal = "@";

  


设置公式

例 E3=B3*D3

Excel.Range r = (Excel.Range)wks.cells[3,5];

r.FormulaR1C1 = "=RC[-3]*RC[-1]";

  


设置区域字体

private void SetRangeFont(Excel.Range range, string font_name, int font_size)

{

Excel.Font target_font = range.Font;

target_font.Name = font_name;

target_font.Size = font_size;

target_font.Strikethrough = false;

target_font.Superscript = false;

target_font.Subscript = false;

target_font.OutlineFont = false;

target_font.Shadow = false;

target_font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleNone;

target_font.ThemeColor = Excel.XlThemeColor.xlThemeColorLight1;

target_font.TintAndShade = 0;

target_font.ThemeFont = Excel.XlThemeFont.xlThemeFontNone;

}

  


设置选择单元格

Excel.Range range = this.Cells[1, 1];

range.Select();

  


  


设置选择行

Excel.Range range = (Excel.Range)this.Rows[1, missing];

range.Select();

  


通过代号获取range

Excel.Range range = get_Range("A1", "B3");

设置Range的详细属性，包括颜色，是否边框为黑色实线等

  


private void SetRangeColor(Excel.Range range,Excel.XlThemeColor color)

{

range.Interior.Pattern = Excel.XlPattern.xlPatternSolid;

range.Interior.PatternColorIndex = Excel.XlPattern.xlPatternAutomatic;

range.Interior.ThemeColor = color;

range.Interior.TintAndShade = 0.89999;

range.Interior.PatternTintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

range.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;

  


  


range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeLeft].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlEdgeTop].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeTop].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeRight].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlInsideVertical].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;

range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].ColorIndex = 0;

range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].TintAndShade = 0;

range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;

}

  


隐藏

Excel.Range child_range = (Excel.Range)wks.Rows["3:3", Type.Missing];

child_range.EntireRow.Hidden = true;

  


另存并加载到当前

wk.SaveAs(file_path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

  


  


另存不加载到当前

wk.SaveCopyAs(file_path);

  


合并单元格

private void MergeCells(string col_name, int start_row, int end_row)

{

Excel.Worksheet wks = m_excell_app.ActiveSheet as Excel.Worksheet;

string str1 = col_name + start_row.ToString();

string str2 = col_name + end_row.ToString();

Excel.Range r1 = (Excel.Range)wks.get_Range(str1, str2);

r1.Merge(Type.Missing);

}

  


设置居中，靠左

range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

  


设置上锁区域

Excel.Worksheet wks = m_excell_app.ActiveSheet as Excel.Worksheet;

foreach (Excel.AllowEditRange aer in wks.Protection.AllowEditRanges)

{

aer.Delete();

}

wks.Protection.AllowEditRanges.Add("s", (Excel.Range)wks.Columns["C:H", Type.Missing], Type.Missing);

wks.Protection.AllowEditRanges.Add("s1", (Excel.Range)wks.Columns["X:AR", Type.Missing], Type.Missing);

int i = 2;

foreach (Excel.Range e in wks.UsedRange.Rows)

{

Excel.Range cell = (Excel.Range)wks.Cells[e.Row, 17];

if (cell.Value2 == null)

{

wks.Protection.AllowEditRanges.Add("s" \+ i.ToString(), cell, Type.Missing);

i++;

}

else

{

try

{

Convert.ToDouble(cell.FormulaR1C1.ToString());

wks.Protection.AllowEditRanges.Add("s" \+ i.ToString(), cell, Type.Missing);

i++;

}

catch (Exception ex)

{

  


  


}

}

}

wks.Protect(Type.Missing, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false);\

=======

移动worksheet

https://docs.microsoft.com/sr-latn-rs/visualstudio/vsto/how-to-programmatically-move-worksheets-within-workbooks?view=vs-2019  

