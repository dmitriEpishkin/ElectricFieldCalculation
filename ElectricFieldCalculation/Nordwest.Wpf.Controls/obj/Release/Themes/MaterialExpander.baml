   M S B A M L   `   `   `  ���� c  _Nordwest.Wpf.Controls, Version=1.2.6697.40983, Culture=neutral, PublicKeyToken=cf3c2579607da4a5=#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   Rotate0 �� Angle  Rotate90  Rotate90Back  	Rotate180  Rotate180Back-    'Nordwest.Wpf.Controls.MultiplyConverter  MultiplyConverter  Data  ExpandArrow3   -Nordwest.Wpf.Controls.DirectionalToggleButton �� 
TargetType �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState	   
Storyboard
 �� TargetProperty �� 
TargetName  EnabledTextColor	 �� To g� Duration g� 	BeginTime 	 EnabledIconColor 
 DisabledTextColor  DisabledIconColor  Shadow1BlurRadius	 W� To W� EasingFunction4  .System.Windows.Media.Animation.ExponentialEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode  Shadow1Depth  Shadow1Color  Shadow3BlurRadius  Shadow3Depth  Shadow3Color ��Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect  ShadowDirection   	Direction   Color   
BlurRadius   ShadowDepth �Name � ColumnDefinitions F�Name  EnabledIconBrush J� RenderTransformOrigin � LayoutTransform ��Name �� Triggers P� Property  P� Value! �� 
TargetName,   &Nordwest.Wpf.Controls.MaterialExpander"  Name# K� ZIndex$   Header%   HeaderTemplate&   ExpandDirection'   	Direction( d� 	IsChecked) �� RelativeSource* �� Path+ �� Mode
, � Tag- `� 	Converter. �� ElementName/   ExpandedSize  Shadow10 ��Name"	  System.Windows.Input.Cursors1 	  SizeNS2 � Cursor3 N� EnterActions4   Exponent5 N� ExitActions6 	  SizeWE�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     0controls#clr-namespace:Nordwest.Wpf.Controls  1system&clr-namespace:System;assembly=mscorlib    �� $ Material.xaml?�%P  &	        &	      &	      &	 *     &	 9     &	 I     &	 N     &	 �     7  '  �     7  7 	 7 
 7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7   7  7   '  {	    7  7  7  7  7  7  7  7  �� $ 0Q��� $ 90Q��� $	 -90Q��� $	 180Q��� $
 -180Q�   �84M24.88 12.88l-6.88 6.88-6.88-6.88-2.12 2.12 9 9 9-9z�F� 8��  
���   ��� "  .���  � �� �� "  .R��    CommonStates��.   
Normal��.	 �� .���� $4
 .(TextBlock.Foreground).(SolidColorBrush.Color)�$ contentPresenter��8   $ 0:0:0.2D�$ 0:0:0X��� $)
 #(Path.Fill).(SolidColorBrush.Color)�$ Arrow��8  $ 0:0:0.2D�$ 0:0:0X�  	MouseOver��  Pressed��  Disabled��.	 �� .���� $4
 .(TextBlock.Foreground).(SolidColorBrush.Color)�$ contentPresenter��8  $ 0:0:0.2D�$ 0:0:0X��� $)
 #(Path.Fill).(SolidColorBrush.Color)�$ Arrow��8  $ 0:0:0.2D�$ 0:0:0X�  CheckStates��.   Checked��.	 �� .��W� $3
 -(Border.Effect).(DropShadowEffect.BlurRadius)�$ 
Background��8  $ 0:0:0.2D�$ 0:0:0X�   $ 	EaseInOut=�W� $4
 .(Border.Effect).(DropShadowEffect.ShadowDepth)�$ 
Background��8  $ 0:0:0.2D�$ 0:0:0X�   $ 	EaseInOut=��� $.
 ((Border.Effect).(DropShadowEffect.Color)�$ 
Background��8  $ 0:0:0.2D�$ 0:0:0X�W� $4
 .(Path.RenderTransform).(RotateTransform.Angle)�$ Arrow��$ 0X�$ 0:0:0.5D�$ 0:0:0X�   $ 	EaseInOut=�  	Unchecked��.	 �� .��W� $3
 -(Border.Effect).(DropShadowEffect.BlurRadius)�$ 
Background��8  $ 0:0:0.2D�$ 0:0:0X�   $ 	EaseInOut=�W� $4
 .(Border.Effect).(DropShadowEffect.ShadowDepth)�$ 
Background��8  $ 0:0:0.2D�$ 0:0:0X�   $ 	EaseInOut=��� $.
 ((Border.Effect).(DropShadowEffect.Color)�$ 
Background��8 	 $ 0:0:0.2D�$ 0:0:0X�W� $4
 .(Path.RenderTransform).(RotateTransform.Angle)�$ Arrow��$	 180X�$ 0:0:0.5D�$ 0:0:0X�   $ 	EaseInOut=�.+��� $ 
Background��
�������   8 
 8  8  8  ._�� $ ContentGrid�� �� $
��Auto ��� $
��Auto �.+�F� $ Arrow��8�� 8�� $
��Left=�$��Center=�$��Center=�$��36��$��36��$ 0.5,0.5&�8  z��� $ 0Q��� $ contentPresenter��$��1��#��z��#��z��#��z��#��z��#��z�� P�  �@ 	Direction$  Right=�. ��� $! ContentGrid��8   �@�LayoutTransform�� $! Arrow��8   �@�LayoutTransformP�  �@ 	Direction$
  Left=�. ��� $! ContentGrid��8   �@�LayoutTransformP�  �@ 	Direction$  Up=�. ��� $! Arrow��8   �@�LayoutTransform�� "  .���  �@ 
IsExpanded .  ��  � ��$ Stretch=���  � ��$ Stretch=���  � �� �� "  .R�]� .+�  $" ExpanderButton��$	��Top=�$# 1��#��z$ $��	10,0,10,0q�#��z% #' z& $��Center=�$
��Left=�( �� ) �� *TemplatedParent+$* 
IsExpanded�$+ TwoWay=��� $ Border��$# 0��#��z��#��z��#��z��$��Stretch=�, X�0.0Q���`� 8-   .1��� $. Border��$	* Tag��� $* ExpandedSize�) �� *TemplatedParent+._�� $ MainGrid��$��Bottom=�#��z/ .+��� $ ContentPresenter��$��	10,0,10,0q�
�������8  ._��� #��z��#��z��#��z��#��z��#��z���� $0 Resizer��$��10��$��0,0,0,-5q�$��Stretch=�$��Bottom=�#2 Z1 
������  P�  �@ 
IsExpanded  . 3 �� .���� .���� $	
 Tag�$ Border��.��W� $ 1X�$ 0:0:0.5D�   $ EaseOut=�$4 3Q�5 �� .���� .���� $	
 Tag�$ Border��.��W� $ 0X�$ 0:0:0.25D�   $ EaseIn=�$4 3Q�P�  �@ ExpandDirection$  Right=�. ��� $! Border��8   �@�LayoutTransform�� $! ContentPresenter��8   �@�LayoutTransform�� $! ExpanderButton�� � ��$ 	0,10,0,10q��� $! ExpanderButton�� � ��$
 Left=��� $! Resizer��# Z6  �@�CursorP�  �@ ExpandDirection$  Up=�. ��� $! Border��8   �@�LayoutTransform�� $! ContentPresenter��8   �@�LayoutTransform�� $! ExpanderButton�� � ��$ 	10,0,10,0q��� $! ExpanderButton�� � ��$ Bottom=�P�  �@ ExpandDirection$
  Left=�. ��� $! Border��8   �@�LayoutTransform�� $! ContentPresenter��8   �@�LayoutTransform�� $! ExpanderButton�� � ��$ 	0,10,0,10q��� $! ExpanderButton�� � ��$ Right=��� $! Resizer��# Z6  �@�Cursor