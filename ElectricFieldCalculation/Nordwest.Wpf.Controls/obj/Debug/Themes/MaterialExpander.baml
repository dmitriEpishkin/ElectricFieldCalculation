   M S B A M L   `   `   `  ����P  LNordwest.Wpf.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   Rotate0 �� Angle  Rotate90  Rotate90Back  	Rotate180  Rotate180Back-    'Nordwest.Wpf.Controls.MultiplyConverter  MultiplyConverter  Data  ExpandArrow3   -Nordwest.Wpf.Controls.DirectionalToggleButton �� 
TargetType �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState	   
Storyboard
 �� TargetProperty �� 
TargetName  EnabledTextColor	 �� To g� Duration g� 	BeginTime 	 EnabledIconColor 
 DisabledTextColor  DisabledIconColor  Shadow1BlurRadius	 W� To W� EasingFunction4  .System.Windows.Media.Animation.ExponentialEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode  Shadow1Depth  Shadow1Color  Shadow3BlurRadius  Shadow3Depth  Shadow3Color ��Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect  ShadowDirection   	Direction   Color   
BlurRadius   ShadowDepth �Name � ColumnDefinitions F�Name  EnabledIconBrush J� RenderTransformOrigin � LayoutTransform ��Name �� Triggers P� Property  P� Value! �� 
TargetName,   &Nordwest.Wpf.Controls.MaterialExpander"  Name# K� ZIndex$   Header%   HeaderTemplate&   ExpandDirection'   	Direction( d� 	IsChecked) �� RelativeSource* �� Path+ �� Mode
, � Tag- `� 	Converter. �� ElementName/   ExpandedSize  Shadow10 ��Name"	  System.Windows.Input.Cursors1 	  SizeNS2 � Cursor3 N� EnterActions4   Exponent5 N� ExitActions6 	  SizeWE�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      0controls#clr-namespace:Nordwest.Wpf.Controls  5      1system&clr-namespace:System;assembly=mscorlib  5        5      �� 5   
   $ Material.xaml?�6   %�  &	        &	      &	 7     &	 T     &	 q     &	 �     &	 �     &	 �     7  '      7  7 	 7 
 7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7   7  7   '  Q    7  7  7  7  7  7  7  7  �� 5      $ 0Q�6&   �� 5	      $ 90Q�6'   �� 5
      $	 -90Q�6+   �� 5      $	 180Q�6(   �� 5      $
 -180Q�6,      5      6!   �5      84M24.88 12.88l-6.88 6.88-6.88-6.88-2.12 2.12 9 9 9-9z�6R   F� 5      8��  
���   �6<   �� 5      "  6   .�5   
   ��  � �� 5      �� 5      "  6"   .R�5      �  5        5      CommonStates��. 5   "     
Normal��.	 5   &   �� .��5   *   �� $4
 .(TextBlock.Foreground).(SolidColorBrush.Color)�69   $ contentPresenter��5   9   8   $ 0:0:0.2D�5   `   $ 0:0:0X�6s   �� 5    *   $)
 #(Path.Fill).(SolidColorBrush.Color)�69   $ Arrow��5!   9   8  $ 0:0:0.2D�5"   `   $ 0:0:0X�6s   5#   '   5$   #     5%   "   	MouseOver��6.     5&   "   Pressed��6.     5'   "   Disabled��.	 5(   &   �� .��5)   *   �� $4
 .(TextBlock.Foreground).(SolidColorBrush.Color)�69   $ contentPresenter��5*   9   8  $ 0:0:0.2D�5+   a   $ 0:0:0X�6t   �� 5,   *   $)
 #(Path.Fill).(SolidColorBrush.Color)�69   $ Arrow��5-   9   8  $ 0:0:0.2D�5.   a   $ 0:0:0X�6t   5/   '   50   #   51        52      CheckStates��. 53   "     Checked��.	 54   &   �� .��55   *   W� $3
 -(Border.Effect).(DropShadowEffect.BlurRadius)�6:   $ 
Background��56   :   8  $ 0:0:0.2D�57   b   $ 0:0:0X�6u    58   .     59   2   $ 	EaseInOut=�6B   5;   +   W� 5<   *   $4
 .(Border.Effect).(DropShadowEffect.ShadowDepth)�6:   $ 
Background��5=   :   8  $ 0:0:0.2D�5>   ]   $ 0:0:0X�6p    5?   .     5@   2   $ 	EaseInOut=�6B   5B   +   �� 5C   *   $.
 ((Border.Effect).(DropShadowEffect.Color)�69   $ 
Background��5D   9   8  $ 0:0:0.2D�5E   \   $ 0:0:0X�6o   W� 5F   *   $4
 .(Path.RenderTransform).(RotateTransform.Angle)�6:   $ Arrow��5G   :   $ 0X�5H   :   $ 0:0:0.5D�6A   $ 0:0:0X�6T    5I   .     5J   2   $ 	EaseInOut=�6B   5L   +   5M   '   5N   #     5O   "   	Unchecked��.	 5P   &   �� .��5Q   *   W� $3
 -(Border.Effect).(DropShadowEffect.BlurRadius)�6:   $ 
Background��5R   :   8  $ 0:0:0.2D�5S   b   $ 0:0:0X�6u    5T   .     5U   2   $ 	EaseInOut=�6B   5W   +   W� 5X   *   $4
 .(Border.Effect).(DropShadowEffect.ShadowDepth)�6:   $ 
Background��5Y   :   8  $ 0:0:0.2D�5Z   ]   $ 0:0:0X�6p    5[   .     5\   2   $ 	EaseInOut=�6B   5^   +   �� 5_   *   $.
 ((Border.Effect).(DropShadowEffect.Color)�69   $ 
Background��5`   9   8 	 $ 0:0:0.2D�5a   \   $ 0:0:0X�6o   W� 5b   *   $4
 .(Path.RenderTransform).(RotateTransform.Angle)�6:   $ Arrow��5c   :   $	 180X�5d   :   $ 0:0:0.5D�6C   $ 0:0:0X�6V    5e   .     5f   2   $ 	EaseInOut=�6B   5h   +   5i   '   5j   #   5k      .+�5m      �� $ 
Background��6!   
������� 5n        5o   "   8 
 8  8  8  6�   ._�5r      � $ ContentGrid��6#    5s   "   �� 5t   &   $
��Auto �67   �� 5u   &   $
��Auto �67   .+�5w   "   F� $ Arrow��6'   8�� 8�� $
��Left=�5y   '   $��Center=�5z   '   $��Center=�5{   '   $��36��5|   '   $��36��5}   '   $ 0.5,0.5&�5~   '   8  z�5�   &   �� 5�   *   $ 0Q�6:   5�   #   �� 5�   "   $ contentPresenter��5�   5   $��1��6%   #��z��5�   %   #��z��5�   %   #��z��5�   %   #��z��5�   %   #��z��5�   %   5�      5�      5�       5�      P� 5�       �@ 	Direction$  Right=�67   . �5�      �� $! ContentGrid��6%   8   �@�LayoutTransform6Y   �� 5�      $! Arrow��6%   8   �@�LayoutTransform6S   5�      P� 5�       �@ 	Direction$
  Left=�67   . �5�      �� $! ContentGrid��6%   8   �@�LayoutTransform6Y   5�      P� 5�       �@ 	Direction$  Up=�67   . �5�      �� $! Arrow��6%   8   �@�LayoutTransform6S   5�      5�      5�      5�      �� 5�      "  6   .�5�   
   ��  �@ 
IsExpanded .  6'   �� 5�   
    � ��$ Stretch=�67   �� 5�   
    � ��$ Stretch=�65   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      ]� .+�5�        $" ExpanderButton��6;   $	��Top=�5�   '   $# 1��5�   '   #��z$ 5�   '   $��	10,0,10,0q�5�   '   #��z% 5�   '   #' z& 5�   '   $��Center=�5�   G   $
��Left=�5�   G   ( 5�   '   �� ) �� *TemplatedParent+$* 
IsExpanded�$+ TwoWay=�5�      �� 5�      $ Border��6!   $# 0��5�   !   #��z��5�   !   #��z��5�   !   #��z��5�   !   $��Stretch=�5�   !   , 5�      X�5�   "   0.0Q�65   ��5�      `� 5�   "   8-   .1�5�   &   �� $. Border��6.   $	* Tag�6C   �� 5�   &   $* ExpandedSize�6`   ) 6.   �� *TemplatedParent+6`   5�   #   ._�5�      � $ MainGrid��6#   $��Bottom=�5�   #   #��z/ 5�   #   .+�5�   "   �� $ ContentPresenter��5�   )   $��	10,0,10,0q�5�   )   
�������8  ._�5�   &   �� #��z��67   #��z��5�   7   #��z��5�   7   #��z��5�   7   #��z��5�   7   5�   #   �� 5�   "   $0 Resizer��6,   $��10��5�   ,   $��0,0,0,-5q�5�   ,   $��Stretch=�5�   ,   $��Bottom=�5�   ,   #2 Z1 5�   ,   
������ 5�   ,   5�      5�      5�       5�      P� 5�       �@ 
IsExpanded  . 3 5�      �� 5�   "   .��5�   &   �� .��5�   *   �� $	
 Tag�65   $ Border��6J   .��5�   .   W� $ 1X�6>   $ 0:0:0.5D�6E    5�   2     5�   6   $ EaseOut=�6F   $4 3Q�6[   5�   /   5�   +   5�   '   5�   #   5 5�      �� 5�   "   .��5�   &   �� .��5�   *   �� $	
 Tag�65   $ Border��6J   .��5�   .   W� $ 0X�6>   $ 0:0:0.25D�6E    5�   2     5�   6   $ EaseIn=�6F   $4 3Q�6Z   5�   /   5�   +   5�   '   5�   #   5�      P� 5�       �@ ExpandDirection$  Right=�6=   . �5�      �� $! Border��6%   8   �@�LayoutTransform6T   �� 5�      $! ContentPresenter��6%   8   �@�LayoutTransform6^   �� 5�      $! ExpanderButton��6%    � ��$ 	0,10,0,10q�6S   �� 5�      $! ExpanderButton��6%    � ��$
 Left=�6[   �� 5�      $! Resizer��6%   # Z6 6L    �@�Cursor5�      P� 5�       �@ ExpandDirection$  Up=�6=   . �5�      �� $! Border��6%   8   �@�LayoutTransform6T   �� 5�      $! ContentPresenter��6%   8   �@�LayoutTransform6^   �� 5�      $! ExpanderButton��6%    � ��$ 	10,0,10,0q�6S   �� 5�      $! ExpanderButton��6%    � ��$ Bottom=�6[   5�      P� 5       �@ ExpandDirection$
  Left=�6=   . �5     �� $! Border��6%   8   �@�LayoutTransform6T   �� 5     $! ContentPresenter��6%   8   �@�LayoutTransform6^   �� 5     $! ExpanderButton��6%    � ��$ 	0,10,0,10q�6S   �� 5     $! ExpanderButton��6%    � ��$ Right=�6[   �� 5     $! Resizer��6%   # Z6 6L    �@�Cursor5     5     5
     5     5     