   M S B A M L   `   `   `  ����P  LNordwest.Wpf.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Nordwest.Wpf.Controls.ChartNordwest.Wpf.Controls.Chart  5      .    (Nordwest.Wpf.Controls.Chart.ChartControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8my)clr-namespace:Nordwest.Wpf.Controls.Chart     5      -   5        .��5      � -   5        $
��grid��5        � RowDefinitions  5	   
   �� 5
      $��30 �6   �� 5      �� 5      $��30 �6    � ColumnDefinitions 5   
   �� 5      $��50 �6   �� 5      $
��200* �6   �� 5      $��50 �6   .+�5   
   -   'Nordwest.Wpf.Controls.Chart.ChartCanvas  -   5          Name$ chartCanvas��5   p   $��1��6   $��1��6)   $��Stretch=�66   $��Stretch=�6T    � UseLayoutRounding . 6�   5   /Nordwest.Wpf.Controls.Chart.VerticalAxisControl  5   
   -   5          Name$ leftAxis��5   .   $��1��6!   1   +Nordwest.Wpf.Controls.Chart.AxisControlBase   Orientation$ Vertical=�6@      TextAlignment$ 
MiddleLeft=�6W      AxisAlignment$ Right=�6r    J� SnapsToDevicePixels .  . 6�     5   
   -   5        $ 	rightAxis��5   .   $��1��6!   $ Vertical=�6A   $ MiddleRight=�6X   $
 Left=�6t    . $��2��6�    . 6�   7   1Nordwest.Wpf.Controls.Chart.HorizontalAxisControl  5   
   -   5        	  Name$	 topAxis��5   @   $��0��6#   $��1��60   
   AxisAlignment$
 Bottom=�6Q   $ 	TopCenter=�6h    .  . 6�     5   
   -   5        $	 
bottomAxis��5   @   $��2��6#   $��1��60   $ BottomCenter=�6T    .  . 6�   5      5      