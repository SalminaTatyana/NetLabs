   M S B A M L   `   `   `  ���� ?  ;Lab2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:Lab2Lab2      Lab2.MainWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $localclr-namespace:Lab2       � Title$  
MainWindow��$	��450��$	��800��.���  � RowDefinitions �� $
��345* ��� $��2* ��� $	��87* � � ColumnDefinitions �� $
��623* ��� $
��177* �.+��� $��
ISO 8859-5��$
��Left=�$��	10,10,0,0q�$	��Top=��� -   $��inText��$
��Left=�$��	10,41,0,0q�$��Введите текст�� �� TextWrapping$
 Wrap=�$	��320��$	��192��$	��Top=��� -   $��Закодировать��$
��Left=�$��335,273,0,0q�$	��Top=��� -   $��outText��$��451,41,10,10q�$��2��$��3���� -   $	��txt��$
��Left=�$��335,298,0,0q�$��81��$��23��$	��Top=��� -   $	��bin��$
��Left=�$��335,326,0,0q�$��81��$��24��$	��Top=�$��3���� -   $ ��Раскодировать��$
��Left=�$��	335,8,0,0q�$	��Top=�$��2���� -   $��histoty��$
��Left=�$��10,268,0,10q�$	��320��$��3���� $��История ввода��$
��Left=�$��
10,238,0,0q�$	��Top=��� -   $��	portLocal��$
��Left=�$��
333,20,0,0q�$��55555��$
 Wrap=�$	��Top=�$	��112���� -	   $��saveUDPDataBtn��$��Подключиться��$
��Left=�$��336,148,0,0q�${�False��$	��Top=�$	��110���� -
   $��ip��$
��Left=�$��335,125,0,0q�$��	127.0.0.1��${�False��$
 Wrap=�$	��Top=�$	��112���� -   $
��port��$
��Left=�$��
335,83,0,0q�$��60000��$
 Wrap=�${�False��$	��Top=�$	��112���� $!��Локальный порт��$
��Left=�$��
334,-3,0,0q�$	��Top=��� $!��Удаленный порт��$
��Left=�$��
335,59,0,0q�$	��Top=�$��24���� $��Ip адрес��$
��Left=�$��336,101,0,0q�$	��Top=�$��24���� -   $��sendUDPDataBtn��$��Отправить��$
��Left=�$��336,173,0,0q�$	��Top=�$	��110��${�False���� -   $��saveLokalPort��$A��;Слушать локальный порт                 ��$
��Left=�$��
450,18,0,0q�$	��Top=�$	��163��