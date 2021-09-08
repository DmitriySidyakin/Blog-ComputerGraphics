# Компьютерная графика

Приложения на C# для объяснения компьютерной графики

## Решение на C# для Кривых Безье

Приложение генерирует кадры для анимации в текущем каталоге исполняемого файла. Приложение написано на C# WPF.
Размер кадра: 400 на 200 точек. Не учитывается уровень дискретизации t, далее в статье написано как рассчитать шаг для Кривых Безье старше 1-го порядка на примере кривых 1-го порядка. 

Подкаталог: https://github.com/DmitriySidyakin/Blog-ComputerGraphics/tree/master/Bezier/BezierSolution

Русскоязычное Описание алгоритма: [Мой Блог: Кривые Безье](https://designermanuals.blogspot.com/2019/12/KryvyeBezier.html)

### Проект рендерит следующие картинки:

![4 points Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b3a.gif)

![4 points De Casteljau's Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b3adc.gif)

![5 points Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b4a.gif)

![5 points De Casteljau's Bezier Curve](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/b4adc.gif)

Изображения генерируются покадрово, кадры находятся в корне приложения.

## Решение на C# для начала Движка для трёхмерной визуализации функциональных повержностей

Приложение представлет собой Движок для рендеринга функциональных объектов на процессоре, без испльзования Видеокарты. Всё построение основано на хранении цветов в 3-х мерном массиве, который представляет собой мир. Главное правильно направить камеру.

Подкаталог: https://github.com/DmitriySidyakin/Blog-ComputerGraphics/tree/master/3DSamles/

Русскоязычное Описание алгоритма: [Мой Блог: 3D графика](https://designermanuals.blogspot.com/2021/01/3D.html)

### Проект рендерит следующие картинки:

![Cube](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/cube.png)

![Bezier Surface](https://github.com/DmitriySidyakin/Blog-ComputerGraphics/blob/master/Documentations/img/BezierSurface.png)

Изображения генерируются в корне приложения.

# Примечание
Приложение позволяет отобразить любые объекты без прозрачности. Они не отделены от примеров.
Рендеринг одного среднего кадра длится порядка месяца для 3D-графики. Для Кривых Безье, рендеринг порядка часа.

Маловероятно, что приложения будут развиваться, но Вы можете их оптимизировать, ускорить, и отталкиваться от них.
