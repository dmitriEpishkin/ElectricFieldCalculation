   M S B A M L   `   `   `  ����P  LNordwest.Wpf.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Nordwest.Wpf.Controls.MiniMapNordwest.Wpf.Controls.MiniMap  5      =#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  5      I)clr-namespace:Nordwest.Wpf.Controls.ChartNordwest.Wpf.Controls.Chart  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0894    .Nordwest.Wpf.Controls.MiniMap.ResizableControl  �� 
TargetType �� Property �� Value �� 
TargetType �Name � RowDefinitions � ColumnDefinitions ��Name � Cursor	    Content2   ,Nordwest.Wpf.Controls.MiniMap.MiniMapControl8   2Nordwest.Wpf.Controls.MiniMap.ResizableControlHost
   Name    GripSize2   ,Nordwest.Wpf.Controls.MiniMap.MiniMapContent  Name ��Name,   &Nordwest.Wpf.Controls.HorizontalScroll �� RelativeSource �� Mode �� Path ��Name*   $Nordwest.Wpf.Controls.VerticalScroll*   $Nordwest.Wpf.Controls.PaletteControl.   (Nordwest.Wpf.Controls.Chart.ChartControl  Name �� AncestorType5   /Nordwest.Wpf.Controls.Chart.VerticalAxisControl  Name1	   +Nordwest.Wpf.Controls.Chart.AxisControlBase 	  Orientation 	  TextAlignment   AxisAlignment J� SnapsToDevicePixels7
   1Nordwest.Wpf.Controls.Chart.HorizontalAxisControl 
 Name 
  AxisAlignment-   'Nordwest.Wpf.Controls.Chart.ChartCanvas  Name�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      7miniMap+clr-namespace:Nordwest.Wpf.Controls.MiniMap  5      0controls#clr-namespace:Nordwest.Wpf.Controls  5      3chart)clr-namespace:Nordwest.Wpf.Controls.Chart  5      %[  '         '  x    '  �    '  �    '  e    '  �    '  �    �� 5      "    6   .�5   
   ��  � �� 5	      �� 5
      "   6"   .R�5      � $
 grid��6    5      �� 5      $��5 �6,   69   �� 5      6/   �� 5      $��5 �6,   69    5      �� 5      $��5 �6/   6;   �� 5      62   �� 5      $��5 �6/   6;   .+�5      �� $ TopGrip��6$   $��0��65   $��1��6B   
���  ��$ SizeNS��6]   �� 5      $ 
BottomGrip��6$   $��2��68   $��1��6E   
���  ��$ SizeNS��6`   �� 5      $ LeftGrip��6$   $��1��66   $��0��6C   
���  ��$ SizeWE��6^   �� 5      $ 	RightGrip��6$   $��1��67   $��2��6D   
���  ��$ SizeWE��6_   �� 5      $ TopLeftGrip��6$   $��0��69   $��0��6F   
���  ��$ SizeNWSE��6a   �� 5      $ TopRightGrip��6$   $��0��6:   $��2��6G   
���  ��$ SizeNESW��6b   �� 5      $ BottomLeftGrip��6$   $��2��6<   $��0��6I   
���  ��$ SizeNESW��6d   �� 5      $ BottomRightGrip��6$   $��2��6=   $��2��6J   
���  ��$ SizeNWSE��6e   �� 5      $��1��6,   $��1��69   #��z	 6I   5      5       5"      5#      �� 5$      "   6   .�5%   
   ��  � �� 5&      �� 5'      "  6"   .R�5(        |�.  ��.  .��5)         $
 Resizer��63   $	��100��6D   $	��100��6P   $ 5Q�6]   $��10��6j   $��10��6y   	 5*        5+   "   $ Content��69   5-      5.      5/      51      �� 52   
    � �� .  6&   �� 53   
    � |� .  6&   54      �� 55      "   6   .�56   
   ��  � �� 57      �� 58      "  6"   .R�59      �� $	 Map��6   .+�5:      �� $ ViewportRectangle��6$   
��� ���$��3��6O   ���	#01ffffff6c   5;      5<      5>      5?      �� 5@      "   6   .�5A   
   ��  � �� 5B      �� 5C      "  6"   .R�5D      �  5E      �� 5F      ��6/   ��  �� $ TemplatedParent=�$ ActualHeight��� 5G      �� 5H      ��6/   ��  �� $ TemplatedParent=�$ ActualHeight��� 5I      ��6/   ��  �� $ TemplatedParent=�$ ActualHeight��� 5J      ��6/   ��  �� $ TemplatedParent=�$ ActualHeight�.+�5L      �� $ 
LessButton��6!   $��0��65   6G   �� 5M      $ 
MoreButton��6!   $��2��65   6G   �� 5N      $ ZoomInButton��6!   $��3��67   6I   �� 5O      $ ZoomOutButton��6!   $��4��68   6J   �� 5P      $ Track��6!   $��1��60   .+�5Q      � $ Slider��6#    5R   "   �� 5S   &   $��12 �67   �� 5T   &   �� 5U   &   $��12 �67   .+�5W   "   �� $ MinGrip��6,   $��0��6=   
���   �$��1��6\   
����� �6p   �� 5X   "   $ MaxGrip��6,   $��2��6=   
���   �$��1��6\   
����� �6p   �� 5Y   "   $ BodyGrip��6,   $��1��6>   
���   �$��1��6]   
�������6q   5Z      5[      5\      5]      5_      5`      �� 5a      "   6   .�5b   
   ��  � �� 5c      �� 5d      "  6"   .R�5e      �  5f      �� 5g      ��6,   ��  �� $ TemplatedParent=�$ ActualWidth��� 5h      ��6,   ��  �� $ TemplatedParent=�$ ActualWidth��� 5i      ��6,   ��  �� $ TemplatedParent=�$ ActualWidth��� 5j      �� 5k      ��6,   ��  �� $ TemplatedParent=�$ ActualWidth�.+�5m      �� $ 
LessButton��6!   $��2��65   6D   �� 5n      $ 
MoreButton��6!   $��4��65   6D   �� 5o      $ ZoomInButton��6!   $��0��67   6F   �� 5p      $ ZoomOutButton��6!   $��1��68   6G   �� 5q      $ Track��6!   $��3��60   .+�5r      � $ Slider��6#    5s   "   �� 5t   &   $��12 �64   �� 5u   &   �� 5v   &   $��12 �64   .+�5x   "   �� $ MinGrip��6,   $��0��6=   
���   �$��1��6Y   
����� �6m   �� 5y   "   $ MaxGrip��6,   $��2��6=   
���   �$��1��6Y   
����� �6m   �� 5z   "   $ BodyGrip��6,   $��1��6>   
���   �$��1��6Z   
�������6n   5{      5|      5}      5~      5�      5�      �� 5�      "   6   .�5�   
   ��  � �� 5�      �� 5�      "  6"   .R�5�      � .+�5�        $ PaletteChart��6-   5�      5�      5�      5�      �� 5�      "   6   .�5�   
   ��  � �� 5�      �� 5�      "  6"   .R�5�      �  5�      �� 5�      ��6,   ��  �� $ FindAncestor=�$ chart:ChartControlL�$ TopAxisSize��� 5�      �� 5�      ��6,   ��  �� $ FindAncestor=�$ chart:ChartControlL�$ BottomAxisSize� 5�      �� 5�      ��6/   ��  �� $ FindAncestor=�$ chart:ChartControlL�$ LeftAxisSize��� 5�      $
��200* �6/   �� 5�      ��6/   ��  �� $ FindAncestor=�$ chart:ChartControlL�$ RightAxisSize�.+�5�        $ LeftAxis��64   $��1��6F   $ Vertical=�6S   $ 
MiddleLeft=�6j   $ Right=�6�    . 6�     5�      $ 	RightAxis��64   $��1��6G   $��2��6T   $ Vertical=�6d   $ MiddleRight=�6{   $
 Left=�6�    . 6�   
  5�      $ TopAxis��66   $��1��6G   $ 	TopCenter=�6W   $ Bottom=�6q    . 6�   
  5�      $ 
BottomAxis��66   $��1��6J   $��2��6Z   $ BottomCenter=�6g    . 6�     5�      $ ChartCanvas��6,   $��1��6A   $��1��6Q   $��Stretch=�6^   $��Stretch=�6|   5�      5�      5�      5�      5�      