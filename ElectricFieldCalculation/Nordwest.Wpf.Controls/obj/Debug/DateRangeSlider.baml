   M S B A M L   `   `   `  ����P  LNordwest.Wpf.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Nordwest.Wpf.ControlsNordwest.Wpf.Controls  5      S.clr-namespace:Nordwest.Wpf.Controls.Converters Nordwest.Wpf.Controls.Converters  5      +    %Nordwest.Wpf.Controls.DateRangeSlider   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      5local#clr-namespace:Nordwest.Wpf.Controls     5      E
converters.clr-namespace:Nordwest.Wpf.Controls.Converters     5      -   5          >�Name$
  root��5       � 	Resources 5      >   8Nordwest.Wpf.Controls.Converters.DateTimeDoubleConverter   dtdConverter@   :Nordwest.Wpf.Controls.Converters.TimeSpanToDoubleConverter  timespanToDoubleConverter  simpleSlider �� 
TargetType J� SnapsToDevicePixels � RowDefinitions ��Name ]�Name ]� Thumb n�Name  HorizontalSliderThumbStyle	 �� MergedDictionaries
 �� Source�� 5   
   	 53      �� 54      $
 SliderResources.xaml?�6%   %�  &	        &	      &	 &     7    5      61     5      63   �� 5       " ��63   .R�5!      ��  . #��z��64   #��z��6`   ._�5"      �  5#      �� 5$      $
��Auto �6,   �� 5%      $
��Auto �6,   #��z��6:   �� 5&      $
��Auto �6,   .+�5)      �� $ PART_SelectionRange��6$   ]� 5+      $ 
PART_Track��6    $��1��64    5,      n� 5-   "   $ Thumb��6(   8��  67   5/      50      51      52      56      .��58      � $��Center=�6   
������ .+�59   
   �� $��0,1,0,0q�6   
�������$��Bottom=�6B   $��1��6]   $��Stretch=�6h   $��0,0,0,10q�5:      �� 5<   
   -   5         ��Name$ LowerSlider��5<      $	��Top=�6&   #��[ 5@      $��0,0,10,0q�5A      {�5<   >   ��  �� ElementName$
 root�� �� Path$ IsLowerSliderEnabled� �� Mode$ TwoWay=� � Minimum 5=      �� $
 root��$ Minimum� �� 	Converter# [   � Maximum 5>      �� $
 root��$ Maximum�# [   � Value 5?      �� $
 root��$ 
LowerValue�$ OneWay=�# [   � SmallChange 5B      �� $
 root��$ SmallChange�# [  � LargeChange 5C      �� $
 root��$ LargeChange�# [ �� 5F   
   -   5        $ UpperSlider��5F      #��[ 5J      $��10,0,0,0q�5K      {�5F   &   �� $
 root��$ IsUpperSliderEnabled�$ TwoWay=� 5G      �� $
 root��$ Minimum�# [   5H      �� $
 root��$ Maximum�# [   5I      �� $
 root��$ 
UpperValue�$ OneWay=�# [   5L      �� $
 root��$ SmallChange�# [  5M      �� $
 root��$ LargeChange�# [ 5O      5P      