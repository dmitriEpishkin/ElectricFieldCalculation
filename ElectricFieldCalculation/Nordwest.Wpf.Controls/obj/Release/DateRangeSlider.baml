   M S B A M L   `   `   `  ���� c  _Nordwest.Wpf.Controls, Version=1.2.6697.40983, Culture=neutral, PublicKeyToken=cf3c2579607da4a5=#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  S.clr-namespace:Nordwest.Wpf.Controls.Converters Nordwest.Wpf.Controls.Converters  +    %Nordwest.Wpf.Controls.DateRangeSlider   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5local#clr-namespace:Nordwest.Wpf.Controls     E
converters.clr-namespace:Nordwest.Wpf.Controls.Converters     -     >�Name$
  root�� � 	Resources >   8Nordwest.Wpf.Controls.Converters.DateTimeDoubleConverter   dtdConverter@   :Nordwest.Wpf.Controls.Converters.TimeSpanToDoubleConverter  timespanToDoubleConverter  simpleSlider �� 
TargetType J� SnapsToDevicePixels � RowDefinitions ��Name ]�Name ]� Thumb n�Name  HorizontalSliderThumbStyle	 �� MergedDictionaries
 �� Source�� 	 �� $
 SliderResources.xaml?�%�   &	        &	      &	 
     7      �� " ��.R���  . #��z��#��z��._��  �� $
��Auto ��� $
��Auto �#��z���� $
��Auto �.+��� $ PART_SelectionRange��]� $ 
PART_Track��$��1�� n� $ Thumb��8��  .��� $��Center=�
������ .+��� $��0,1,0,0q�
�������$��Bottom=�$��1��$��Stretch=�$��0,0,0,10q��� -    ��Name$ LowerSlider��$	��Top=�#��[ $��0,0,10,0q�{���  �� ElementName$
 root�� �� Path$ IsLowerSliderEnabled� �� Mode$ TwoWay=� � Minimum �� $
 root��$ Minimum� �� 	Converter# [   � Maximum �� $
 root��$ Maximum�# [   � Value �� $
 root��$ 
LowerValue�$ OneWay=�# [   � SmallChange �� $
 root��$ SmallChange�# [  � LargeChange �� $
 root��$ LargeChange�# [ �� -   $ UpperSlider��#��[ $��10,0,0,0q�{��� $
 root��$ IsUpperSliderEnabled�$ TwoWay=� �� $
 root��$ Minimum�# [   �� $
 root��$ Maximum�# [   �� $
 root��$ 
UpperValue�$ OneWay=�# [   �� $
 root��$ SmallChange�# [  �� $
 root��$ LargeChange�# [ 