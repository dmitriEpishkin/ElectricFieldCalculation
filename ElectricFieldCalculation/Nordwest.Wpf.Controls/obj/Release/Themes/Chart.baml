   M S B A M L   `   `   `  ���� c  _Nordwest.Wpf.Controls, Version=1.2.6697.40983, Culture=neutral, PublicKeyToken=cf3c2579607da4a5M+clr-namespace:Nordwest.Wpf.Controls.MiniMapNordwest.Wpf.Controls.MiniMap  =#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  I)clr-namespace:Nordwest.Wpf.Controls.ChartNordwest.Wpf.Controls.Chart  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0894    .Nordwest.Wpf.Controls.MiniMap.ResizableControl  �� 
TargetType �� Property �� Value �� 
TargetType �Name � RowDefinitions � ColumnDefinitions ��Name � Cursor	    Content2   ,Nordwest.Wpf.Controls.MiniMap.MiniMapControl8   2Nordwest.Wpf.Controls.MiniMap.ResizableControlHost
   Name    GripSize2   ,Nordwest.Wpf.Controls.MiniMap.MiniMapContent  Name ��Name,   &Nordwest.Wpf.Controls.HorizontalScroll �� RelativeSource �� Mode �� Path ��Name*   $Nordwest.Wpf.Controls.VerticalScroll*   $Nordwest.Wpf.Controls.PaletteControl.   (Nordwest.Wpf.Controls.Chart.ChartControl  Name �� AncestorType5   /Nordwest.Wpf.Controls.Chart.VerticalAxisControl  Name1	   +Nordwest.Wpf.Controls.Chart.AxisControlBase 	  Orientation 	  TextAlignment   AxisAlignment J� SnapsToDevicePixels7
   1Nordwest.Wpf.Controls.Chart.HorizontalAxisControl 
 Name 
  AxisAlignment-   'Nordwest.Wpf.Controls.Chart.ChartCanvas  Name�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     7miniMap+clr-namespace:Nordwest.Wpf.Controls.MiniMap  0controls#clr-namespace:Nordwest.Wpf.Controls  3chart)clr-namespace:Nordwest.Wpf.Controls.Chart  %�  '         '  �    '  _    '  �    '  �    '  O	    '  �	    �� "    .���  � �� �� "   .R�� $
 grid�� �� $��5 ��� �� $��5 � �� $��5 ��� �� $��5 �.+��� $ TopGrip��$��0��$��1��
���  ��$ SizeNS���� $ 
BottomGrip��$��2��$��1��
���  ��$ SizeNS���� $ LeftGrip��$��1��$��0��
���  ��$ SizeWE���� $ 	RightGrip��$��1��$��2��
���  ��$ SizeWE���� $ TopLeftGrip��$��0��$��0��
���  ��$ SizeNWSE���� $ TopRightGrip��$��0��$��2��
���  ��$ SizeNESW���� $ BottomLeftGrip��$��2��$��0��
���  ��$ SizeNESW���� $ BottomRightGrip��$��2��$��2��
���  ��$ SizeNWSE���� $��1��$��1��#��z	 �� "   .���  � �� �� "  .R�  |�.  ��.  .��   $
 Resizer��$	��100��$	��100��$ 5Q�$��10��$��10��	   $ Content����  � �� .  ��  � |� .  �� "   .���  � �� �� "  .R��� $	 Map��.+��� $ ViewportRectangle��
��� ���$��3�����	#01ffffff�� "   .���  � �� �� "  .R��  �� ����  �� $ TemplatedParent=�$ ActualHeight��� �� ����  �� $ TemplatedParent=�$ ActualHeight��� ����  �� $ TemplatedParent=�$ ActualHeight��� ����  �� $ TemplatedParent=�$ ActualHeight�.+��� $ 
LessButton��$��0���� $ 
MoreButton��$��2���� $ ZoomInButton��$��3���� $ ZoomOutButton��$��4���� $ Track��$��1��.+�� $ Slider�� �� $��12 ��� �� $��12 �.+��� $ MinGrip��$��0��
���   �$��1��
����� ��� $ MaxGrip��$��2��
���   �$��1��
����� ��� $ BodyGrip��$��1��
���   �$��1��
��������� "   .���  � �� �� "  .R��  �� ����  �� $ TemplatedParent=�$ ActualWidth��� ����  �� $ TemplatedParent=�$ ActualWidth��� ����  �� $ TemplatedParent=�$ ActualWidth��� �� ����  �� $ TemplatedParent=�$ ActualWidth�.+��� $ 
LessButton��$��2���� $ 
MoreButton��$��4���� $ ZoomInButton��$��0���� $ ZoomOutButton��$��1���� $ Track��$��3��.+�� $ Slider�� �� $��12 ��� �� $��12 �.+��� $ MinGrip��$��0��
���   �$��1��
����� ��� $ MaxGrip��$��2��
���   �$��1��
����� ��� $ BodyGrip��$��1��
���   �$��1��
��������� "   .���  � �� �� "  .R�� .+�  $ PaletteChart���� "   .���  � �� �� "  .R��  �� ����  �� $ FindAncestor=�$ chart:ChartControlL�$ TopAxisSize��� �� ����  �� $ FindAncestor=�$ chart:ChartControlL�$ BottomAxisSize� �� ����  �� $ FindAncestor=�$ chart:ChartControlL�$ LeftAxisSize��� $
��200* ��� ����  �� $ FindAncestor=�$ chart:ChartControlL�$ RightAxisSize�.+�  $ LeftAxis��$��1��$ Vertical=�$ 
MiddleLeft=�$ Right=� .   $ 	RightAxis��$��1��$��2��$ Vertical=�$ MiddleRight=�$
 Left=� . 
  $ TopAxis��$��1��$ 	TopCenter=�$ Bottom=� . 
  $ 
BottomAxis��$��1��$��2��$ BottomCenter=� .   $ ChartCanvas��$��1��$��1��$��Stretch=�$��Stretch=�